using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
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
    public string SiteCookie = String.Empty;
    public string FeedName = String.Empty;
    public List<string> FeedURL = new List<string>();

    private int MaxResults;
    private bool MPTVSeries;

    #endregion    

    #region Init

    public Sites(string strType)
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
            switch (strType)
            {
              case "Feeds":
                if (nodeItem.SelectNodes("feeds").Count != 0)
                {
                  if (nodeItem.SelectSingleNode("login") != null)
                  {
                    if ((nodeItem["login"].Attributes["username"].InnerText.Length != 0) && (nodeItem["login"].Attributes["password"].InnerText.Length != 0))
                    {
                      _Items.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText));
                    }
                  }
                  else
                  {
                    _Items.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText));
                  }
                }
                break;
              case "Search":
                if (nodeItem.SelectNodes("searches").Count != 0)
                {
                  if (nodeItem.SelectSingleNode("login") != null)
                  {
                    if ((nodeItem["login"].Attributes["username"].InnerText.Length != 0) && (nodeItem["login"].Attributes["password"].InnerText.Length != 0))
                    {
                      _Items.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText));
                    }
                  }
                  else
                  {
                    _Items.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText));
                  }
                }
                break;
              case "Groups":
                if (nodeItem.SelectSingleNode("groups") != null)
                {
                  if (nodeItem.SelectSingleNode("login") != null)
                  {
                    if ((nodeItem["login"].Attributes["username"].InnerText.Length != 0) && (nodeItem["login"].Attributes["password"].InnerText.Length != 0))
                    {
                      _Items.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText));
                    }
                  }
                  else
                  {
                    _Items.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText));
                  }
                }
                break;
            }
          }

          if (_Items.Count > 0)
          {
            GUIListItem _Item = MP.Menu(_Items, "Select Site");
            if (_Item != null)
            {
              SiteName = _Item.Label;

              if (xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/login") != null)
              {
                string strUsername = xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/login").Attributes["username"].InnerText;
                string strPassword = xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/login").Attributes["password"].InnerText;

                if ((strUsername.Length > 0) && (strUsername.Length > 0))
                {
                  SiteCookie = SiteLogin(SiteName, strUsername, strPassword);
                  if (SiteCookie.Length == 0)
                  {
                    GUIPropertyManager.SetProperty("#Status", "Login failure");
                  }
                }
              }
            }
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing XML");
        }

        Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");
        MaxResults = mpSettings.GetValueAsInt("#Sites", "MaxResults", 50);
        if (MaxResults == 0) { MaxResults = 50; }
        MPTVSeries = mpSettings.GetValueAsBool("#Sites", "MPTVSeries", false);
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
        List<GUIListItem> _Items = new List<GUIListItem>();

        GUIListItem Item;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Plugins) + @"\Windows\Sites.xml");

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
          FeedURL.Add(_Item.Path.Replace("[MAX]", MaxResults.ToString()));
        }
      }
      catch (Exception e) { MP.Error(e); }
    }

    public void SetSearch()
    {
      try
      {
        List<GUIListItem> _Items = new List<GUIListItem>();

        _Items.Add(new GUIListItem("New Search"));

        if (MPTVSeries)
        {
          _Items.Add(new GUIListItem("Missing Episodes"));
        }

        GUIListItem Item;

        XmlDocument xmlSettings = new XmlDocument();
        xmlSettings.Load(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");

        foreach (XmlNode nodeItem in xmlSettings.SelectNodes("profile/section[@name='#Searches']/entry"))
        {
          Item = new GUIListItem(nodeItem.Attributes["name"].InnerText);
          Item.Path = nodeItem.InnerText;
          _Items.Add(Item);
        }

        GUIListItem _Item = MP.Menu(_Items, "Select Search");
        if (_Item != null)
        {
          XmlDocument xmlDoc = new XmlDocument();
          xmlDoc.Load(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Plugins) + @"\Windows\Sites.xml");

          _Items = new List<GUIListItem>();

          foreach (XmlNode searchItem in xmlDoc.SelectNodes("sites/site[@name='" + SiteName + "']/searches/search"))
          {
            Item = new GUIListItem(searchItem.Attributes["name"].InnerText);
            Item.Path = searchItem.InnerText;
            _Items.Add(Item);
          }

          GUIListItem _Search = ((_Items.Count == 1) ? _Items[0] : MP.Menu(_Items, "Select Specific Search"));
          if (_Search != null)
          {
            string strSearchURL = _Search.Path;

            switch (_Item.Label)
            {
              case "New Search":
                FeedName = MP.Keyboard();
                if (FeedName.Length > 0)
                {
                  FeedURL.Add(strSearchURL.Replace("[QUERY]", FeedName.Replace(" ", "+")).Replace("[MAX]", MaxResults.ToString()));
                }
                else
                {
                  GUIPropertyManager.SetProperty("#Status", "Search field is blank");
                }
                break;
              case "Missing Episodes":
                MPTVSeries MTS = new MPTVSeries();
                GUIListItem _Series = MP.Menu(MTS.SeriesNames(), "Select Series");
                if (_Series != null)
                {
                  GUIListItem _Episode = MP.Menu(MTS.MissingEpisodes(_Series.Label), "Select Episode");
                  if (_Episode != null)
                  {
                    FeedName = _Series.Label + " - " + _Episode.Label;
                    FeedURL.Add(strSearchURL.Replace("[QUERY]", FeedName.Replace(" ", "+")).Replace("[MAX]", MaxResults.ToString()));
                  }
                }
                break;
              default:
                FeedName = _Item.Label;
                if (_Item.Path.Contains("|"))
                {
                  string[] strSearches = _Item.Path.Split('|');
                  foreach (string strSearch in strSearches)
                  {
                    FeedURL.Add(strSearchURL.Replace("[QUERY]", strSearch.Replace(" ", "+")).Replace("[MAX]", MaxResults.ToString()));
                  }
                }
                else
                {
                  FeedURL.Add(strSearchURL.Replace("[QUERY]", _Item.Path.Replace(" ", "+")).Replace("[MAX]", MaxResults.ToString()));
                }
                break;
            }
          }
        }
      }
      catch (Exception e) { MP.Error(e); }
    }

    public void SetGroup()
    {
      try
      {
        List<GUIListItem> _Items = new List<GUIListItem>();

        XmlDocument xmlSettings = new XmlDocument();
        xmlSettings.Load(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");

        foreach (XmlNode nodeItem in xmlSettings.SelectNodes("profile/section[@name='#Groups']/entry"))
        {
          _Items.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText));
        }

        if (_Items.Count > 0)
        {
          GUIListItem _Item = MP.Menu(_Items, "Select Group");
          if (_Item != null)
          {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Plugins) + @"\Windows\Sites.xml");

            FeedName = _Item.Label;
            FeedURL.Add(xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/groups").InnerText.Replace("[GROUP]", FeedName.Replace(" ", "+")).Replace("[MAX]", MaxResults.ToString()));
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "No groups added");
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

        string strSize = String.Empty;
        double dblSize = 0;

        if (xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size") != null)
        {
          switch (int.Parse(xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["type"].InnerText))
          {               
            case 1:
              Match mSize = Regex.Match(((xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size[@attribute]") != null) ? _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["attribute"].InnerText].InnerText : _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/size").Attributes["element"].InnerText].InnerText), @"(\d+[,|.]\d+)\s?(\w{2,4})");
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

        DateTime dtPubDate = new DateTime();
        if (_Node.SelectSingleNode("pubDate") != null)
        {
          DateTime.TryParseExact(_Node["pubDate"].InnerText.Replace("GMT", "+0000"), "ddd, dd MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtPubDate);
        }

        string strNZBInfo = String.Empty;
        switch (SiteName)
        {
          case "Newzbin":
            if (SiteCookie.Length > 0)
            {
              string Source = String.Empty;
              string VideoFmt = String.Empty;
              string VideoGenre = String.Empty;
              string Language = String.Empty;
              string Subtitles = String.Empty;

              if (_Node["report:attributes"] != null)
              {
                XmlNodeList Attributes = _Node["report:attributes"].ChildNodes;
                if (Attributes.Count > 0)
                {
                  foreach (XmlNode Attribute in Attributes)
                  {
                    switch (Attribute.Attributes[0].InnerText)
                    {
                      case "Source":
                        Source += ((Source.Length > 0) ? ", " : String.Empty) + Attribute.InnerText;
                        break;
                      case "Video Fmt":
                        VideoFmt += ((VideoFmt.Length > 0) ? ", " : String.Empty) + Attribute.InnerText;
                        break;
                      case "Video Genre":
                        VideoGenre += ((VideoGenre.Length > 0) ? ", " : String.Empty) + Attribute.InnerText;
                        break;
                      case "Language":
                        Language += ((Language.Length > 0) ? ", " : String.Empty) + Attribute.InnerText;
                        break;
                      case "Subtitles":
                        Subtitles += ((Subtitles.Length > 0) ? ", " : String.Empty) + Attribute.InnerText;
                        break;
                    }
                  }
                }
              }

              strNZBInfo = ((Source.Length > 0) ? "Source: " + Source + Environment.NewLine : String.Empty) + ((VideoFmt.Length > 0) ? "Video Format: " + VideoFmt + Environment.NewLine : String.Empty) + "Video Genre: " + ((VideoGenre.Length > 0) ? VideoGenre + Environment.NewLine : String.Empty) + ((Language.Length > 0) ? "Language: " + Language + Environment.NewLine : String.Empty) + ((Subtitles.Length > 0) ? "Subtitles: " + Subtitles : String.Empty);
            }
            break;
          case "TvNZB":
          case "MyTvNZB":
            if ((_Node["season"] != null) && (_Node["episode"] != null))
            {
              string Season = _Node["season"].InnerText;
              string Episode = _Node["episode"].InnerText;

              strNZBInfo = ((Season.Length > 0) ? "Season: " + Season + Environment.NewLine : String.Empty) + ((Season.Length > 0) ? "Episode: " + Episode : String.Empty);
            }
            break;
        }

        MP.ListItem(_List, ((xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title[@attribute]") != null) ? _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title").Attributes["element"].InnerText].Attributes[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title").Attributes["attribute"].InnerText].InnerText : _Node[xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item/title").Attributes["element"].InnerText].InnerText).Replace("&quot;", "\"").Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">"), strSize, strNZBInfo, dtPubDate, (long)dblSize, strURL, int.Parse(xmlDoc.SelectSingleNode("sites/site[@name='" + SiteName + "']/item").Attributes["type"].InnerText));
      }
      catch (Exception e) { MP.Error(e); }
    }

    #endregion

    #region Functions

    string SiteLogin(string _Site, string _Username, string _Password)
    {
      string strResult = String.Empty;
      string strURL = String.Empty;

      try
      {       
        switch (_Site)
        {
          case "Newzbin":
            strURL = "http://www.newzbin.com/account/login/";
            break;
        }

        if (strURL.Length > 0)
        {
          Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");

          string strLogin = mpSettings.GetValueAsString("#Sites", _Site + "_Login", String.Empty);
          if (strLogin.Length > 0)
          {
            HttpWebRequest cookieReq = (HttpWebRequest)WebRequest.Create(strURL);
            cookieReq.Headers.Add(HttpRequestHeader.Cookie, strLogin);

            HttpWebResponse cookieResp = (HttpWebResponse)cookieReq.GetResponse();
            string strCookies = cookieResp.Headers[HttpResponseHeader.SetCookie];
            if (strCookies.Length > 0)
            {
              strResult = strLogin;
            }
            cookieResp.Close();
          }

          if (strResult.Length == 0)
          {
            string postString = "username=" + _Username + "&" + "password=" + _Password;

            ASCIIEncoding Encoding = new ASCIIEncoding();
            byte[] postData = Encoding.GetBytes(postString);

            HttpWebRequest cookieReq = (HttpWebRequest)WebRequest.Create(strURL);
            cookieReq.Method = "POST";
            cookieReq.ContentType = "application/x-www-form-urlencoded";
            cookieReq.ContentLength = postData.Length;
            cookieReq.AllowAutoRedirect = false;

            Stream postStream = cookieReq.GetRequestStream();
            postStream.Write(postData, 0, postData.Length);
            postStream.Close();

            HttpWebResponse cookieResp = (HttpWebResponse)cookieReq.GetResponse();
            string strCookies = cookieResp.Headers[HttpResponseHeader.SetCookie];
            if (strCookies.Length > 0)
            {
              switch (_Site)
              {
                case "Newzbin":
                  int NzbSessionID_POS = strCookies.IndexOf("NzbSessionID=") + "NzbSessionID=".Length;
                  string NzbSessionID = strCookies.Substring(NzbSessionID_POS, strCookies.IndexOf(";", NzbSessionID_POS) - NzbSessionID_POS);

                  int NzbSmoke_POS = strCookies.IndexOf("NzbSmoke=") + "NzbSmoke=".Length;
                  string NzbSmoke = strCookies.Substring(NzbSmoke_POS, strCookies.IndexOf(";", NzbSmoke_POS) - NzbSmoke_POS);

                  if ((NzbSessionID.Length > 0) && (NzbSmoke.Length > 0))
                  {
                    strResult = "NzbSessionID=" + NzbSessionID + ";" + "NzbSmoke=" + NzbSmoke;
                    mpSettings.SetValue("#Sites", _Site + "_Login", strResult);
                  }
                  break;
              }
            }
            cookieResp.Close();
          }

          mpSettings.Dispose();
        }
      }
      catch (Exception e) { MP.Error(e); }

      return strResult;
    }

    #endregion

  }
}