using System;
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

    #endregion    

    #region Init

    private string uid;
    private string pass;
    private string Username;
    private string Password;

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

    public void SetFeed()
    {
      FeedName = Dialogs.Menu(new string[] { "Main RSS Feed", "User's Category Selection RSS" }, "Select Feed");
      if (FeedName.Length > 0)
      {
        Dialogs.Wait();
        if (!(Cookie())) { return; }
        FeedURL = URLGrab();
        Dialogs.bolWaiting = false;
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
      Dialogs.AddItem(lstList, Node["title"].InnerText, strTemp.Substring(intSizePOS, strTemp.IndexOf("(", intSizePOS) - intSizePOS), Node["link"].InnerText, 1);
    }

    #endregion

    #region Special Functions

    private string URLGrab()
    {
      WebClient webClient = new WebClient();
      webClient.Headers.Set("Cookie", "uid=" + uid + "; pass=" + pass);
      string htmlCode = webClient.DownloadString("http://www.nzbsrus.com/rss.php");

      int intStartIndex = 0;
      switch (FeedName)
      {
        case "Main RSS Feed":
          intStartIndex = htmlCode.IndexOf(">http://www.nzbsrus.com/rssfeed.php?i") + 1;
          break;
        case "User's Category Selection RSS":
          intStartIndex = htmlCode.IndexOf(">http://www.nzbsrus.com/rssfeed.php?c") + 1;
          break;
      }

      if (intStartIndex > 0)
      {
        int intStopIndex = htmlCode.IndexOf("</a>", intStartIndex);
        return htmlCode.Substring(intStartIndex, intStopIndex - intStartIndex).Replace("&amp;", "&");
      }

      return String.Empty;
    }

    #endregion

    #region Cookie

    private bool Cookie()
    {
      if ((uid.Length == 0) || (pass.Length == 0))
      {
        if (GetCookie())
        {
          return true;
        }
      }
      else if (CheckCookie())
      {
        return true;
      }
      return false;
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