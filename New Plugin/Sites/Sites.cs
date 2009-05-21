using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

using MediaPortal.Configuration;
using MediaPortal.GUI.Library;

namespace mpNZB
{
  class Sites
  {

    #region Declarations

    public string SiteName = String.Empty;
    public string FeedName = String.Empty;
    public string FeedURI = String.Empty;
    public string Cookies = String.Empty;

    Functions MP = new Functions();

    #endregion

    #region Initialization

    public Sites(string _Type)
    {
      SetSite(_Type);
    }

    #endregion

    #region Dialogs

    public void SetSite(string _Type)
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Config.GetFolder(Config.Dir.Plugins) + @"\Windows\Sites.xml");

        if (xmlDoc.SelectSingleNode("sites/site") != null)
        {
          List<GUIListItem> Sites = new List<GUIListItem>();

          foreach (XmlNode nodeItem in xmlDoc.SelectNodes("sites/site"))
          {
            switch (_Type)
            {
              case "Feeds":
                if (nodeItem.SelectSingleNode("feeds/feed") != null) Sites.Add(new GUIListItem(nodeItem.Attributes["id"].InnerText));
                break;
              case "Groups":
                if (nodeItem.SelectSingleNode("groups") != null) Sites.Add(new GUIListItem(nodeItem.Attributes["id"].InnerText));
                break;
              case "Search":
                if (nodeItem.SelectSingleNode("search") != null) Sites.Add(new GUIListItem(nodeItem.Attributes["id"].InnerText));
                break;
            }
          }

          if (Sites.Count > 0)
          {
            GUIListItem Site = MP.Menu(Sites, "Select Site");
            if (Site != null)
            {
              SiteName = Site.Label;
            }
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Sites missing.");
        }
      }
      catch (Exception ex) { MP.Error(ex); }
    }

    public void SetFeed()
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Config.GetFolder(Config.Dir.Plugins) + @"\Windows\Sites.xml");

        if (xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/feeds/feed") != null)
        {
          List<GUIListItem> Feeds = new List<GUIListItem>();

          foreach (XmlNode nodeItem in xmlDoc.SelectNodes("sites/site[@id='" + SiteName + "']/feeds/feed"))
          {
            Feeds.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText, String.Empty, nodeItem.InnerText, false, null));
          }

          if (Feeds.Count > 0)
          {
            GUIListItem Feed = MP.Menu(Feeds, "Select Feed");
            if (Feed != null)
            {
              FeedName = Feed.Label;
              FeedURI = Feed.Path.Replace("[MAX]", MP.GetValueAsInt("#Sites", "MaxResults", 50).ToString());
            }
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Feeds missing.");
        }
      }
      catch (Exception ex) { MP.Error(ex); }
    }

    public void SetGroup()
    {
      try
      {
        XmlDocument xmlSettings = new XmlDocument();
        xmlSettings.Load(Config.GetFolder(Config.Dir.Config) + @"\mpNZB.xml");

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Config.GetFolder(Config.Dir.Plugins) + @"\Windows\Sites.xml");

        if ((xmlSettings.SelectSingleNode("profile/section[@name='#Groups']/entry") != null) && (xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/group") != null))
        {
          List<GUIListItem> Groups = new List<GUIListItem>();

          foreach (XmlNode nodeItem in xmlSettings.SelectNodes("profile/section[@name='#Groups']/entry"))
          {
            Groups.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText));
          }

          if (Groups.Count > 0)
          {
            GUIListItem Group = MP.Menu(Groups, "Select Group");
            if (Group != null)
            {
              FeedName = Group.Label;
              FeedURI = xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/group").InnerText.Replace("[GROUP]", FeedName).Replace("[MAX]", MP.GetValueAsInt("#Sites", "MaxResults", 50).ToString());
            }
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Groups missing.");
        }
      }
      catch (Exception ex) { MP.Error(ex); }
    }

    public void SetSearch()
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Config.GetFolder(Config.Dir.Plugins) + @"\Windows\Sites.xml");

        if (xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/search") != null)
        {
          FeedName = MP.Keyboard();
          if (FeedName.Length > 0)
          {
            FeedURI = xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/search").InnerText.Replace("[QUERY]", FeedName.Replace(" ", "+")).Replace("[MAX]", MP.GetValueAsInt("#Sites", "MaxResults", 50).ToString());
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Search missing.");
        }
      }
      catch (Exception ex) { MP.Error(ex); }
    }
    
    #endregion

    #region Items

    public void AddItem(XmlNode _Node, GUIListControl _List)
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Config.GetFolder(Config.Dir.Plugins) + @"\Windows\Sites.xml");

        #region Title

        string strTitle = String.Empty;
        if (xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/title") != null)
        {
          strTitle = ((xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/title[@attribute]") != null) ? _Node[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/title").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/title").Attributes["attribute"].InnerText].InnerText : _Node[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/title").Attributes["element"].InnerText].InnerText).Replace("&quot;", "\"").Replace("&amp;", "&").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">");
        }

        #endregion

        #region Size

        string strSize = String.Empty;
        double dblSize = 0;
        if (xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/size") != null)
        {
          switch (int.Parse(xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/size").Attributes["type"].InnerText))
          {
            case 1:
              if (xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/size/regex") != null)
              {
                Match regSize = Regex.Match(((xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/size[@attribute]") != null) ? _Node[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/size").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/size").Attributes["attribute"].InnerText].InnerText : _Node[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/size").Attributes["element"].InnerText].InnerText), @"\w{4}:\s(\d+[,|.]\d+)\s(\w{2,4})");
                if ((regSize.Success) && (regSize.Groups[1].Value.Length > 0) && (regSize.Groups[2].Value.Length > 0))
                {
                  dblSize = double.Parse(Regex.Replace(regSize.Groups[1].Value, "[(.|,)]", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0].ToString()));
                  switch (regSize.Groups[2].Value.Replace("KiB", "KB").Replace("MiB", "MB").Replace("GiB", "GB").Replace("TiB", "TB"))
                  {
                    case "KB":
                      dblSize = (dblSize * 1024);
                      break;
                    case "MB":
                      dblSize = ((dblSize * 1024) * 1024);
                      break;
                    case "GB":
                      dblSize = (((dblSize * 1024) * 1024) * 1024);
                      break;
                    case "TB":
                      dblSize = ((((dblSize * 1024) * 1024) * 1024) * 1024);
                      break;
                  }
                }
              }
              break;
            case 2:
              dblSize = ((xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/size[@attribute]") != null) ? double.Parse(_Node[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/size").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/size").Attributes["attribute"].InnerText].InnerText) : double.Parse(_Node[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/size").Attributes["element"].InnerText].InnerText));
              break;
          }

          if (dblSize == 0) { strSize = String.Empty; }
          else if (dblSize < 1024) { strSize = dblSize.ToString("N2") + " B"; }
          else if (dblSize < 1048576) { strSize = (dblSize / 1024).ToString("N2") + " KB"; }
          else if (dblSize < 1073741824) { strSize = ((dblSize / 1024) / 1024).ToString("N2") + " MB"; }
          else if (dblSize < 1099511627776) { strSize = (((dblSize / 1024) / 1024) / 1024).ToString("N2") + " GB"; }
          else if (dblSize < 1125899906842624) { strSize = ((((dblSize / 1024) / 1024) / 1024) / 1024).ToString("N2") + " TB"; }
        }

        #endregion

        #region URL

        string strURL = String.Empty;
        if (xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/url") != null)
        {
          strURL = ((xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/url[@attribute]") != null) ? _Node[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/url").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/url").Attributes["attribute"].InnerText].InnerText : _Node[xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/url").Attributes["element"].InnerText].InnerText);
          if (xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item/url/regex") != null)
          {
            foreach (XmlNode Replacement in xmlDoc.SelectNodes("sites/site[@id='" + SiteName + "']/item/url/replace"))
            {
              strURL = strURL.Replace(Replacement.Attributes["old"].InnerText, Replacement.Attributes["new"].InnerText);
            }
          }
        }

        #endregion

        #region Description

        string strDescription = String.Empty;
        if (_Node["description"] != null)
        {
          strDescription = _Node["description"].InnerText;
          if (Regex.IsMatch(strDescription, @"<[^>]*>"))
          {
            strDescription = Regex.Replace(strDescription, "(\n|\r)", String.Empty, RegexOptions.Multiline);
            strDescription = Regex.Replace(strDescription, "< *br */*>", "\n", RegexOptions.IgnoreCase & RegexOptions.Multiline);
            strDescription = Regex.Replace(strDescription, "< */ *li */*>", "\n", RegexOptions.IgnoreCase & RegexOptions.Multiline);
            strDescription = Regex.Replace(strDescription, "  +", " ", RegexOptions.Multiline);
            strDescription = Regex.Replace(strDescription, "<[^>]*>", String.Empty, RegexOptions.Multiline);
            strDescription = strDescription.Replace("&quot;", "\"").Replace("&amp;", "&").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">");
          }
        }

        #endregion

        MP.ListItem(_List, strTitle, strSize, strURL, int.Parse(xmlDoc.SelectSingleNode("sites/site[@id='" + SiteName + "']/item").Attributes["type"].InnerText), strDescription, DateTime.ParseExact(_Node["pubDate"].InnerText.Replace("GMT", "+0000"), "ddd, dd MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture), (long)dblSize);
      }
      catch (Exception ex) { MP.Error(ex); }
    }

    #endregion

  }
}