using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

using MediaPortal.GUI.Library;
using MediaPortal.Profile;

namespace mpNZB.Sites
{
  class NZBsRus:iSite
  {   

    #region Definitions

    public string SiteName
    {
      get { return "NZBsRus"; }
    }

    private string _FeedName;
    public string FeedName
    {
      get { return _FeedName; }
      set { _FeedName = value; }
    }

    private string _FeedURL;
    public string FeedURL
    {
      get { return _FeedURL; }
      set { _FeedURL = value; }
    }

    private string _Username;
    public string Username
    {
      get { return _Username; }
      set { _Username = value; }
    }

    private string _Password;
    public string Password
    {
      get { return _Password; }
      set { _Password = value; }
    }

    #endregion    

    #region Init

    private string uid;
    private string pass;

    public NZBsRus()
    {
      Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");

      uid = mpSettings.GetValue("#Cookies", "NZBsRus_uid");
      pass = mpSettings.GetValue("#Cookies", "NZBsRus_pass");
      Username = mpSettings.GetValue("#Sites", "Newzbin_username");
      Password = mpSettings.GetValue("#Sites", "Newzbin_password");

      mpSettings.Dispose();
    }

    #endregion

    #region Functions

    private mpFunctions Dialogs = new mpFunctions();

    private string h = String.Empty;

    public void SetFeed()
    {
      FeedName = Dialogs.Menu(new string[] { "Main RSS Feed", "Anime", "Apps - 0-Day", "Apps - MAC", "Apps - Misc", "Apps - PC iSO", "Console - Gamecube", "Console - PS2", "Console - PS3", "Console - Wii", "Console - XBOX", "Console - XBOX 360", "eBooks/ Tutorials", "Games - Misc", "Games - PC DOX", "Games - PC iSO", "Handheld - DS", "Handheld - PSP", "Movies - DVDr", "Movies - HD", "Movies - Misc", "Movies - XviD", "Music - MP3", "Music - Video", "Phone", "TV - DVDr", "TV - HD", "TV - Misc", "TV - XviD" }, "Select Feed");
      if (FeedName.Length > 0)
      {
        if (Cookie() == String.Empty) { return; }

        if (h.Length == 0)
        {
          WebClient webClient = new WebClient();
          webClient.Headers.Set("Cookie", "uid=" + uid + "; pass=" + pass);
          string htmlCode = webClient.DownloadString("http://www.nzbsrus.com/rss.php");

          int intStartIndex = htmlCode.IndexOf(";h=") + 3;
          if (intStartIndex > 0)
          {
            int intStopIndex = htmlCode.IndexOf("\"", intStartIndex);
            h = htmlCode.Substring(intStartIndex, intStopIndex - intStartIndex);
          }
        }

        int intID = 0;
        switch (FeedName)
        {
          case "Anime": intID = 3; break;
          case "Apps - 0-Day": intID = 6; break;
          case "Apps - MAC": intID = 93; break;
          case "Apps - Misc": intID = 9; break;
          case "Apps - PC iSO": intID = 12; break;
          case "Console - Gamecube": intID = 87; break;
          case "Console - PS2": intID = 30; break;
          case "Console - PS3": intID = 89; break;
          case "Console - Wii": intID = 92; break;
          case "Console - XBOX": intID = 39; break;
          case "Console - XBOX 360": intID = 88; break;
          case "eBooks/ Tutorials": intID = 15; break;
          case "Games - Misc": intID = 24; break;
          case "Games - PC DOX": intID = 96; break;
          case "Games - PC iSO": intID = 27; break;
          case "Handheld - DS": intID = 21; break;
          case "Handheld - PSP": intID = 33; break;
          case "Movies - DVDr": intID = 45; break;
          case "Movies - HD": intID = 90; break;
          case "Movies - Misc": intID = 48; break;
          case "Movies - XviD": intID = 51; break;
          case "Music - MP3": intID = 54; break;
          case "Music - Video": intID = 60; break;
          case "Phone": intID = 99; break;
          case "TV - DVDr": intID = 69; break;
          case "TV - HD": intID = 91; break;
          case "TV - Misc": intID = 72; break;
          case "TV - XviD": intID = 75; break;
        }

        FeedURL = "http://www.nzbsrus.com/rssfeed.php" + ((intID > 0) ? "?cat=" + intID.ToString() + "&" : "?") + "i=" + uid + "&h=" + h;
      }
    }

    public void Search()
    {
    }    

    public void AddItem(XmlNode Node, GUIListControl lstList)
    {
      string strTemp = Node["description"].InnerText.Replace(" ", String.Empty);
      string strSizeText = "<b>Size:</b>".ToLower();
      int intSizePOS = strTemp.ToLower().IndexOf(strSizeText.ToLower()) + strSizeText.Length;

      Dialogs.AddItem(lstList, Node["title"].InnerText, strTemp.Substring(intSizePOS, strTemp.IndexOf("(", intSizePOS) - intSizePOS).Replace("GiB", " GB").Replace("MiB", " MB").Replace("KiB", " KB").Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0].ToString()).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0].ToString()), Node["link"].InnerText, 1);
    }

    #endregion

    #region Cookie

    public string Cookie()
    {
      if ((uid.Length == 0) || (pass.Length == 0))
      {
        if (GetCookie())
        {
          return "uid=" + uid + "; pass=" + pass;
        }
      }
      else if (CheckCookie())
      {
        return "uid=" + uid + "; pass=" + pass;
      }
      return String.Empty;
    }

    private bool CheckCookie()
    {
      string htmlLine;
      WebClient webClient = new WebClient();

      webClient.Headers.Set("Cookie", "uid=" + uid + "; pass=" + pass);
      Stream htmlStream = webClient.OpenRead("http://www.nzbsrus.com/my.php");
      StreamReader htmlReader = new StreamReader(htmlStream);
      while ((htmlLine = htmlReader.ReadLine()) != null)
      {
        if (htmlLine.ToUpper().Contains("private page".ToUpper()))
        {
          htmlReader.Close();
          htmlReader.Dispose();
          return true;
        }
      }
      htmlReader.Close();
      htmlReader.Dispose();

      return GetCookie();
    }

    private bool GetCookie()
    {
      if ((Username.Length > 0) && (Password.Length > 0))
      {
        HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create("http://www.nzbsrus.com/takelogin.php");
        webReq.CookieContainer = new CookieContainer();
        webReq.Method = "POST";
        webReq.ContentType = "application/x-www-form-urlencoded";

        // Create POST data
        byte[] byteArray = Encoding.UTF8.GetBytes("username=" + Username + "&password=" + Password);

        webReq.ContentLength = byteArray.Length;
        webReq.AllowAutoRedirect = false;

        // Get the request stream
        Stream dataStream = webReq.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        // Get the cookies
        HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();

        // Process cookies
        if (webResp.Cookies.Count > 0)
        {
          Settings mpSaveSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");
          foreach (Cookie Cookies in webResp.Cookies)
          {
            switch (Cookies.Name)
            {
              case "uid":
                uid = Cookies.Value;
                mpSaveSettings.SetValue("#Cookies", "NZBsRus_uid", uid);
                break;
              case "pass":
                pass = Cookies.Value;
                mpSaveSettings.SetValue("#Cookies", "NZBsRus_pass", pass);
                break;
            }
          }
          webResp.Close();
          mpSaveSettings.Dispose();
          return true;
        }
      }
      return false;
    }

    #endregion

  }
}