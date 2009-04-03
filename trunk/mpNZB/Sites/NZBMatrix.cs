using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

using MediaPortal.GUI.Library;
using MediaPortal.Profile;

namespace mpNZB.Sites
{
  class NZBMatrix:iSite
  {    

    #region Definitions

    public string SiteName
    {
      get { return "NZBMatrix"; }
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

    public NZBMatrix()
    {
      Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");

      uid = mpSettings.GetValue("#Cookies", "NZBMatrix_uid");
      pass = mpSettings.GetValue("#Cookies", "NZBMatrix_pass");
      Username = mpSettings.GetValue("#Sites", "Newzbin_username");
      Password = mpSettings.GetValue("#Sites", "Newzbin_password");

      mpSettings.Dispose();
    }

    #endregion

    #region Functions

    private mpFunctions Dialogs = new mpFunctions();

    public void SetFeed()
    {
      FeedName = Dialogs.Menu(new string[] { "All", "Movies", "TV", "Documentaries", "Games", "Apps", "Music", "Anime", "Other" }, "Select Feed");
      if (FeedName.Length > 0)
      {
        Dialogs.Wait();
        if (Cookie() == String.Empty) { return; }
        FeedURL = "http://nzbmatrix.com/rss.php" + ((FeedName == "All") ? String.Empty : "?cat=" + FeedName.ToLower());
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
      Dialogs.AddItem(lstList, Node["title"].InnerText, strTemp.Substring(intSizePOS, strTemp.IndexOf("<", intSizePOS) - intSizePOS), Node["link"].InnerText.Replace("nzb-details.php", "nzb-download.php").Replace("&hit=1", String.Empty), 3);
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
      Stream htmlStream = webClient.OpenRead("http://nzbmatrix.com/account.php");
      StreamReader htmlReader = new StreamReader(htmlStream);
      while ((htmlLine = htmlReader.ReadLine()) != null)
      {
        if (htmlLine.ToUpper().Contains("User CP".ToUpper()))
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
        HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create("http://nzbmatrix.com/account-login.php");
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
                mpSaveSettings.SetValue("#Cookies", "NZBMatrix_uid", uid);
                break;
              case "pass":
                pass = Cookies.Value;
                mpSaveSettings.SetValue("#Cookies", "NZBMatrix_pass", pass);
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