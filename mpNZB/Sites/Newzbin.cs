using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

using MediaPortal.GUI.Library;
using MediaPortal.Profile;

namespace mpNZB.Sites
{
  class Newzbin:iSite
  {   

    #region Definitions

    public string SiteName
    {
      get { return "Newzbin"; }
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

    private string NzbSmoke;
    private string NzbSessionID;

    public Newzbin()
    {
      Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");

      NzbSmoke = mpSettings.GetValue("#Cookies", "Newzbin_NzbSmoke");
      NzbSessionID = mpSettings.GetValue("#Cookies", "Newzbin_NzbSessionID");
      Username = mpSettings.GetValue("#Sites", "Newzbin_username");
      Password = mpSettings.GetValue("#Sites", "Newzbin_password");

      mpSettings.Dispose();
    }

    #endregion

    #region Functions

    private mpFunctions Dialogs = new mpFunctions();

    public void SetFeed()
    {
      FeedName = Dialogs.Menu(new string[] { "Everything", "Unknown", "Anime", "Apps", "Books", "Consoles", "Discussions", "Emulation", "Games", "Misc", "Movies", "Music", "PDA", "Resources", "TV" }, "Select Feed");
      if (FeedName.Length > 0)
      {
        if (Cookie() == String.Empty) { return; }
        FeedURL = "http://www.newzbin.com/browse/category/p/" + FeedName.ToLower() + "/?feed=rss" + "&COOKIE:NzbSmoke=" + NzbSmoke + ";NzbSessionID=" + NzbSessionID;
      }
    }

    public void Search()
    {
      FeedName = Dialogs.Keyboard();
      if (FeedName.Length > 0)
      {
        if (Cookie() == String.Empty) { return; }
        FeedURL = "http://www.newzbin.com/search/query/?q=" + FeedName + "&searchaction=Go&feed=rss" + "&COOKIE:NzbSmoke=" + NzbSmoke + ";NzbSessionID=" + NzbSessionID;
      }
    }

    public void AddItem(XmlNode Node, GUIListControl lstList)
    {
      double dblSize = 0;

      double.TryParse(Node["report:size"].InnerText, out dblSize);
      if (dblSize > 0) { dblSize = ((dblSize / 1024) / 1024); }

      Dialogs.AddItem(lstList, Node["title"].InnerText, ((dblSize != 0.0) ? dblSize.ToString().Substring(0, dblSize.ToString().IndexOf(".") + 3) : "0.0") + " MB", Node["report:id"].InnerText, 4);
    }

    #endregion

    #region Cookie

    public string Cookie()
    {
      if ((NzbSmoke.Length == 0) || (NzbSessionID.Length == 0))
      {
        if (GetCookie())
        {
          return "NzbSmoke=" + NzbSmoke + "; NzbSessionID=" + NzbSessionID;
        }
      }
      else if (CheckCookie())
      {
        return "NzbSmoke=" + NzbSmoke + "; NzbSessionID=" + NzbSessionID;
      }
      return String.Empty;
    }

    private bool CheckCookie()
    {
      string htmlLine;
      WebClient webClient = new WebClient();

      webClient.Headers.Set("Cookie", "NzbSmoke=" + NzbSmoke + "; NzbSessionID=" + NzbSessionID);
      Stream htmlStream = webClient.OpenRead("http://www.newzbin.com/account/");
      StreamReader htmlReader = new StreamReader(htmlStream);
      while ((htmlLine = htmlReader.ReadLine()) != null)
      {
        if (htmlLine.ToUpper().Contains("My Account".ToUpper()))
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
        HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create("http://www.newzbin.com/account/login/");
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
              case "NzbSessionID":
                NzbSessionID = Cookies.Value;
                mpSaveSettings.SetValue("#Cookies", "Newzbin_NzbSessionID", NzbSessionID);
                break;
              case "NzbSmoke":
                NzbSmoke = Cookies.Value;
                mpSaveSettings.SetValue("#Cookies", "Newzbin_NzbSmoke", NzbSmoke);
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