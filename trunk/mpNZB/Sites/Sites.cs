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

    private string _SiteName = String.Empty;
    public string SiteName
    {
      get { return _SiteName; }
      set { _SiteName = value; }
    }

    private string _FeedName = String.Empty;
    public string FeedName
    {
      get { return _FeedName; }
      set { _FeedName = value; }
    }

    private string _FeedURL = String.Empty;
    public string FeedURL
    {
      get { return _FeedURL; }
      set { _FeedURL = value; }
    }

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
          List<mpFunctions.MenuItem> _Items = new List<mpFunctions.MenuItem>();

          foreach (XmlNode nodeItem in xmlDoc.SelectNodes("sites/site"))
          {
            if (_Search)
            {
              if (nodeItem.SelectSingleNode("search") != null)
              {
                _Items.Add(new mpFunctions.MenuItem(nodeItem.Attributes["name"].InnerText, String.Empty));
              }
            }
            else
            {
              if (nodeItem.SelectNodes("feeds").Count != 0)
              {
                _Items.Add(new mpFunctions.MenuItem(nodeItem.Attributes["name"].InnerText, String.Empty));
              }
            }
          }

          mpFunctions.MenuItem _Item = MP.Menu(_Items, "Select Site");
          if (_Item != null)
          {
            SiteName = _Item.Label;
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing XML.");
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
          List<mpFunctions.MenuItem> _Items = new List<mpFunctions.MenuItem>();

          foreach (XmlNode nodeItem in xmlDoc.SelectNodes("sites/site[@name='" + SiteName + "']/feeds/feed"))
          {
            _Items.Add(new mpFunctions.MenuItem(nodeItem.Attributes["name"].InnerText, nodeItem.InnerText));
          }

          mpFunctions.MenuItem _Item = MP.Menu(_Items, "Select Feed");
          if (_Item != null)
          {
            FeedName = _Item.Label;
            FeedURL = _Item.Path.Replace("[MAX]", MaxResults.ToString());
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing XML.");
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
            GUIPropertyManager.SetProperty("#Status", "Error parsing XML.");
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
          if (xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size") != null)
          {
            switch (int.Parse(xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["type"].InnerText))
            {
              case 1:
                strSize = ((xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size[@attribute]") != null) ? _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["attribute"].InnerText].InnerText : _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["element"].InnerText].InnerText);
                if (xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size/regex") != null)
                {
                  Match mSize = Regex.Match(strSize, xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size/regex").Attributes["input"].InnerText);
                  if (mSize.Success) { strSize = mSize.Groups[1].Value + " " + mSize.Groups[2].Value; }
                }
                strSize = Regex.Replace(strSize.Replace("Byte", "B").Replace("KiB", "KB").Replace("MiB", "MB").Replace("GiB", "GB").Replace("TiB", "TB"), "[(.|,)]", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0].ToString());
                break;
              case 2:
                double dblSize = ((xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size[@attribute]") != null) ? double.Parse(_Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["attribute"].InnerText].InnerText) : double.Parse(_Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["element"].InnerText].InnerText));
                if (dblSize > 0) { dblSize = ((dblSize / 1024) / 1024); }
                strSize = ((dblSize < 1024) ? ((dblSize < 1) ? (dblSize * 1024).ToString("N2") + " KB" : dblSize.ToString("N2") + " MB") : (dblSize / 1024).ToString("N2") + " GB");
                break;
            }
          }

          string strURL = ((xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/url[@attribute]") != null) ? _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/url").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/url").Attributes["attribute"].InnerText].InnerText : _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/url").Attributes["element"].InnerText].InnerText);
          if (xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/url/regex") != null)
          {
            foreach (XmlNode _RegEx in xmlDoc.SelectNodes("sites/site[@name='" + SiteName + "']/item/url/regex"))
            {
              strURL = Regex.Replace(strURL, _RegEx.Attributes["input"].InnerText, _RegEx.Attributes["replacement"].InnerText);
            }
          }

          MP.ListItem(_List, ((xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title[@attribute]") != null) ? _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title").Attributes["attribute"].InnerText].InnerText : _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title").Attributes["element"].InnerText].InnerText).Replace("&quot;", "\"").Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">"), strSize, strURL, int.Parse(xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item").Attributes["type"].InnerText));
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing XML.");
        }
      }
      catch (Exception e) { MP.Error(e); }
    }

    #endregion

  }
}