using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

using MediaPortal.GUI.Library;
using MediaPortal.Profile;

namespace mpNZB
{
  class Sites
  {   

    #region Definitions

    public string SiteName = String.Empty;
    public string FeedName = String.Empty;
    public string FeedURL = String.Empty;

    private int MaxResults;

    #endregion    

    #region Init

    public Sites(bool _Search)
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Plugins) + @"\Windows\Sites.xml");

        if (xmlDoc.SelectSingleNode("sites/site") != null)
        {
          List<GUIListItem> _Items = new List<GUIListItem>();

          foreach (XmlNode nodeItem in xmlDoc.SelectNodes("sites/site"))
          {
            if (_Search)
            {
              if (nodeItem.SelectSingleNode("search") != null)
              {
                _Items.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText));
              }
            }
            else
            {
              if (nodeItem.SelectNodes("feeds").Count != 0)
              {
                _Items.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText));
              }
            }
          }

          GUIListItem _Item = MP.Menu(_Items, "Select Site");
          if (_Item != null)
          {
            SiteName = _Item.Label;
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing XML");
        }

        Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");
        MaxResults = mpSettings.GetValueAsInt("#Sites", "MaxResults", 50);
        mpSettings.Dispose();
      }
      catch (Exception e) { MP.Error(e); }
    }

    #endregion

    #region Commands

    private mpFunctions MP = new mpFunctions();

    public void SetFeed()
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Plugins) + @"\Windows\Sites.xml");

        if (xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/feeds/feed") != null)
        {
          List<GUIListItem> _Items = new List<GUIListItem>();
          GUIListItem Item;

          foreach (XmlNode nodeItem in xmlDoc.SelectNodes("sites/site[@name='" + SiteName + "']/feeds/feed"))
          {
            Item = new GUIListItem(nodeItem.Attributes["name"].InnerText);
            Item.Path = nodeItem.InnerText;
            _Items.Add(Item);
          }

          GUIListItem _Item = MP.Menu(_Items, "Select Feed");
          if (_Item != null)
          {
            FeedName = _Item.Label;
            FeedURL = _Item.Path.Replace("[MAX]", MaxResults.ToString());
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing XML");
        }
      }
      catch (Exception e) { MP.Error(e); }
    }

    public void SetSearch()
    {
      try
      {
        FeedName = MP.Keyboard();
        if (FeedName.Length > 0)
        {
          XmlDocument xmlDoc = new XmlDocument();
          xmlDoc.Load(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Plugins) + @"\Windows\Sites.xml");

          if (xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/search/url") != null)
          {
            FeedURL = xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/search/url").InnerText.Replace("[QUERY]", FeedName.Replace(" ", "+")).Replace("[MAX]", MaxResults.ToString());
          }
          else
          {
            GUIPropertyManager.SetProperty("#Status", "Error parsing XML");
          }
        }
      }
      catch (Exception e) { MP.Error(e); }
    }

    public void AddItem(XmlNode _Node, GUIListControl _List)
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Plugins) + @"\Windows\Sites.xml");

        if (xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item") != null)
        {
          string strSize = String.Empty;
          double dblSize = 0;

          if (xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size") != null)
          {
            switch (int.Parse(xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["type"].InnerText))
            {               
              case 1:
                if (xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size/regex") != null)
                {
                  Match mSize = Regex.Match(((xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size[@attribute]") != null) ? _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["attribute"].InnerText].InnerText : _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["element"].InnerText].InnerText), xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size/regex").Attributes["input"].InnerText);
                  if ((mSize.Success) && (mSize.Groups[1].Value.Length > 0) && (mSize.Groups[2].Value.Length > 0))
                  {
                    dblSize = double.Parse(Regex.Replace(mSize.Groups[1].Value, "[(.|,)]", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0].ToString()));
                    switch (mSize.Groups[2].Value.Replace("KiB", "KB").Replace("MiB", "MB").Replace("GiB", "GB").Replace("TiB", "TB"))
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
                dblSize = ((xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size[@attribute]") != null) ? double.Parse(_Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["attribute"].InnerText].InnerText) : double.Parse(_Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["element"].InnerText].InnerText));
                break;
            }

            if (dblSize == 0) { strSize = String.Empty; }
            else if (dblSize < 1024) { strSize = dblSize.ToString("N2") + " B"; }
            else if (dblSize < 1048576) { strSize = (dblSize / 1024).ToString("N2") + " KB"; }
            else if (dblSize < 1073741824) { strSize = ((dblSize / 1024) / 1024).ToString("N2") + " MB"; }
            else if (dblSize < 1099511627776) { strSize = (((dblSize / 1024) / 1024) / 1024).ToString("N2") + " GB"; }
            else if (dblSize < 1125899906842624) { strSize = ((((dblSize / 1024) / 1024) / 1024) / 1024).ToString("N2") + " TB"; }
          }

          string strURL = ((xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/url[@attribute]") != null) ? _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/url").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/url").Attributes["attribute"].InnerText].InnerText : _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/url").Attributes["element"].InnerText].InnerText);
          if (xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/url/regex") != null)
          {
            foreach (XmlNode _RegEx in xmlDoc.SelectNodes("sites/site[@name='" + SiteName + "']/item/url/regex"))
            {
              strURL = Regex.Replace(strURL, _RegEx.Attributes["input"].InnerText, _RegEx.Attributes["replacement"].InnerText);
            }
          }

          MP.ListItem(_List, ((xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title[@attribute]") != null) ? _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title").Attributes["attribute"].InnerText].InnerText : _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title").Attributes["element"].InnerText].InnerText).Replace("&quot;", "\"").Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">"), strSize, DateTime.ParseExact(_Node["pubDate"].InnerText.Replace("GMT", "+0000"), "ddd, dd MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture), (long)dblSize, strURL, int.Parse(xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item").Attributes["type"].InnerText));
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing XML");
        }
      }
      catch (Exception e) { MP.Error(e); }
    }

    #endregion

  }
}