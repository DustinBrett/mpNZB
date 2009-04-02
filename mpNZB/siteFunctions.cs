using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

using MediaPortal.GUI.Library;
using MediaPortal.Profile;

namespace mpNZB.Sites
{
  class siteFunctions
  {
    // Newzbin
    public string Newzbin_NzbSessionID = String.Empty;
    public string Newzbin_NzbSmoke = String.Empty;

    // NZBMatrix
    public string NZBMatrix_uid = String.Empty;
    public string NZBMatrix_pass = String.Empty;

    // NZBsRus
    public string NZBsRus_uid = String.Empty;
    public string NZBsRus_pass = String.Empty;

    // Search Data
    public int intResultLimit = 250;
    public int intPageNumber = 0;
    public string strSearchString = String.Empty;

    // Current List
    public string strSite;
    public string strFeedURL;

    private mpFunctions Dialogs = new mpFunctions();

    public bool Cookie()
    {
      bool bolNoCookie = false;
      switch (strSite)
      {
        case "NZBsRus":
          if ((NZBsRus_uid.Length == 0) || (NZBsRus_pass.Length == 0))
          {
            bolNoCookie = true;
          }
          break;
        case "NBZMatrix":
          if ((NZBMatrix_uid.Length == 0) || (NZBMatrix_pass.Length == 0))
          {
            bolNoCookie = true;
          }
          break;
        case "Newzbin":
          if ((Newzbin_NzbSmoke.Length == 0) || (Newzbin_NzbSessionID.Length == 0))
          {
            bolNoCookie = true;
          }
          break;
      }
      if (bolNoCookie)
      {
        if (GetCookie())
        {
          return true;
        }
      }
      else
      {
        if (CheckCookie())
        {
          return true;
        }
      }
      GUIPropertyManager.SetProperty("#Status", "Unable to retrieve cookies.");
      return false;
    }
    
    private bool CheckCookie()
    {
      string htmlLine;
      WebClient webClient = new WebClient();
      Stream htmlStream;
      StreamReader htmlReader;

      switch (strSite)
      {
        case "Newzbin":
          webClient.Headers.Set("Cookie", "NzbSmoke=" + Newzbin_NzbSmoke + "; NzbSessionID=" + Newzbin_NzbSessionID);
          htmlStream = webClient.OpenRead("http://www.newzbin.com/account/");
          htmlReader = new StreamReader(htmlStream);
          while ((htmlLine = htmlReader.ReadLine()) != null)
          {
            if (htmlLine.ToUpper().Contains("My Account"))
            {
              htmlReader.Close();
              htmlReader.Dispose();
              return true;
            }
          }
          break;
        case "NZBMatrix":
          webClient.Headers.Set("Cookie", "uid=" + NZBMatrix_uid + "; pass=" + NZBMatrix_pass);
          htmlStream = webClient.OpenRead("http://nzbmatrix.com/account.php");
          htmlReader = new StreamReader(htmlStream);
          while ((htmlLine = htmlReader.ReadLine()) != null)
          {
            if (htmlLine.ToUpper().Contains("User CP"))
            {
              htmlReader.Close();
              htmlReader.Dispose();
              return true;
            }
          }
          break;
        case "NZBsRus":
          webClient.Headers.Set("Cookie", "uid=" + NZBsRus_uid + "; pass=" + NZBsRus_pass);
          htmlStream = webClient.OpenRead("http://www.nzbsrus.com/my.php");
          htmlReader = new StreamReader(htmlStream);
          while ((htmlLine = htmlReader.ReadLine()) != null)
          {
            if (htmlLine.ToUpper().Contains("private page"))
            {
              htmlReader.Close();
              htmlReader.Dispose();
              return true;
            }
          }
          break;
      }

      return GetCookie();
    }

    private bool GetCookie()
    {
      // Create URL
      string strLoginURL = String.Empty;
      switch (strSite)
      {
        case "Newzbin":
          strLoginURL = "http://www.newzbin.com/account/login/";
          break;
        case "NZBMatrix":
          strLoginURL = "http://nzbmatrix.com/account-login.php";
          break;
        case "NZBsRus":
          strLoginURL = "http://www.nzbsrus.com/takelogin.php";
          break;
      }

      if (strLoginURL.Length > 0)
      {
        Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");
        string strUsername = mpSettings.GetValue("#Sites", strSite + "_username");
        string strPassword = mpSettings.GetValue("#Sites", strSite + "_password");
        mpSettings.Dispose();

        if ((strUsername.Length > 0) && (strPassword.Length > 0))
        {
          HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create(strLoginURL);
          webReq.CookieContainer = new CookieContainer();
          webReq.Method = "POST";
          webReq.ContentType = "application/x-www-form-urlencoded";

          // Create POST data
          byte[] byteArray = Encoding.UTF8.GetBytes("username=" + strUsername + "&password=" + strPassword);

          webReq.ContentLength = byteArray.Length;
          webReq.AllowAutoRedirect = false;

          // Get the request stream.
          Stream dataStream = webReq.GetRequestStream();
          dataStream.Write(byteArray, 0, byteArray.Length);
          dataStream.Close();

          // Get the cookie.
          HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();

          // Process cookies
          if (webResp.Cookies.Count > 0)
          {
            Settings mpSaveSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");
            foreach (Cookie Cookies in webResp.Cookies)
            {
              switch (strSite)
              {
                case "Newzbin":
                  {
                    switch (Cookies.Name)
                    {
                      case "NzbSessionID":
                        Newzbin_NzbSessionID = Cookies.Value;
                        mpSaveSettings.SetValue("#Cookies", "Newzbin_NzbSessionID", Newzbin_NzbSessionID);
                        break;
                      case "NzbSmoke":
                        Newzbin_NzbSmoke = Cookies.Value;
                        mpSaveSettings.SetValue("#Cookies", "Newzbin_NzbSmoke", Newzbin_NzbSmoke);
                        break;
                    }
                  }
                  break;
                case "NZBMatrix":
                  {
                    switch (Cookies.Name)
                    {
                      case "uid":
                        NZBMatrix_uid = Cookies.Value;
                        mpSaveSettings.SetValue("#Cookies", "NZBMatrix_uid", NZBMatrix_uid);
                        break;
                      case "pass":
                        NZBMatrix_pass = Cookies.Value;
                        mpSaveSettings.SetValue("#Cookies", "NZBMatrix_pass", NZBMatrix_pass);
                        break;
                    }
                  }
                  break;
                case "NZBsRus":
                  {
                    switch (Cookies.Name)
                    {
                      case "uid":
                        NZBsRus_uid = Cookies.Value;
                        mpSaveSettings.SetValue("#Cookies", "NZBsRus_uid", NZBsRus_uid);
                        break;
                      case "pass":
                        NZBsRus_pass = Cookies.Value;
                        mpSaveSettings.SetValue("#Cookies", "NZBsRus_pass", NZBsRus_pass);
                        break;
                    }
                  }
                  break;
              }
            }
            webResp.Close();
            mpSaveSettings.Dispose();
            return true;
          }

          webResp.Close();
        }
      }
      return false;
    }

    public string fncNZBsRusFeed(string strFeed, string strUID, string strPass)
    {
      string strResult = String.Empty;

      WebClient webClient = new WebClient();
      webClient.Headers.Set("Cookie", "uid=" + strUID + "; pass=" + strPass);
      string htmlCode = webClient.DownloadString("http://www.nzbsrus.com/rss.php");

      int intStartIndex = 0;
      int intStopIndex = 0;

      switch (strFeed)
      {
        case "Main RSS Feed":
          intStartIndex = htmlCode.IndexOf(">http://www.nzbsrus.com/rssfeed.php?i") + 1;
          intStopIndex = htmlCode.IndexOf("</a>", intStartIndex);
          strResult = htmlCode.Substring(intStartIndex, intStopIndex - intStartIndex).Replace("&amp;", "&");
          break;
        case "User's Category Selection RSS":
          intStartIndex = htmlCode.IndexOf(">http://www.nzbsrus.com/rssfeed.php?c") + 1;
          intStopIndex = htmlCode.IndexOf("</a>", intStartIndex);
          strResult = htmlCode.Substring(intStartIndex, intStopIndex - intStartIndex).Replace("&amp;", "&");
          break;
      }

      return strResult;
    }

    public void fncBinsearch(GUIListControl lstItemList, GUIWindow GUI, GUIButtonControl btnNext)
    {
      try
      {
        lstItemList.Clear();

        // HTTP Request
        HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create("http://binsearch.info/?q=" + strSearchString.Replace(" ", "+") + "&max=" + intResultLimit.ToString() + "&adv_age=&server=&min=" + (intResultLimit * intPageNumber));
        webReq.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");

        // HTTP Response
        HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();
        Stream sResponse = webResp.GetResponseStream();
        if (webResp.ContentEncoding.ToLower().Contains("gzip")) { sResponse = new GZipStream(sResponse, CompressionMode.Decompress); }
        StreamReader sReader = new StreamReader(sResponse, Encoding.Default);
        string strHTML = sReader.ReadToEnd();

        // Parse HTML
        string strID = "<input type=\"checkbox\" name=\"";
        string strDesc = "<span class=\"s\">";
        string strSize = "size: ";
        string strTempSize = String.Empty;

        int intID_POS = strHTML.IndexOf(strID) + strID.Length;
        int intDesc_POS = strHTML.IndexOf(strDesc) + strDesc.Length;
        int intSize_POS = strHTML.IndexOf(strSize) + strSize.Length;
        int intNextID = 0;
        int intCount = 0;

        do
        {
          strTempSize = String.Empty;
          if (intSize_POS > intID_POS)
          {
            intNextID = strHTML.IndexOf(strID, intID_POS) + strID.Length;
            if ((intSize_POS < intNextID) || ((intNextID < intID_POS) && (intSize_POS < strHTML.IndexOf("</table>", intID_POS))))
            {
              strTempSize = strHTML.Substring(intSize_POS, strHTML.IndexOf(",", intSize_POS) - intSize_POS);
              intSize_POS = strHTML.IndexOf(strSize, intSize_POS) + strSize.Length;
            }
          }

          Dialogs.AddItem(lstItemList, strHTML.Substring(intDesc_POS, strHTML.IndexOf("</span>", intDesc_POS) - intDesc_POS).Replace("&quot;", "\"").Replace("&amp;", "\"").Replace("&lt;", "<").Replace("&gt;", ">"), strTempSize, "http://binsearch.info/?action=nzb%26" + strHTML.Substring(intID_POS, strHTML.IndexOf("\"", intID_POS) - intID_POS) + "=1", 1);

          intID_POS = strHTML.IndexOf(strID, intID_POS) + strID.Length;
          intDesc_POS = strHTML.IndexOf(strDesc, intDesc_POS) + strDesc.Length;

          intCount += 1;
        }
        while (intID_POS - strID.Length != -1);

        btnNext.Disabled = (!(strHTML.Contains("min=" + (intResultLimit * (intPageNumber + 1)))));
        GUIPropertyManager.SetProperty("#Status", "Item Count (" + intCount.ToString() + ") (Page: " + (intPageNumber + 1) + ")");
      }
      catch (Exception e)
      {
        Log.Info("Data: " + e.Data);
        Log.Info("HelpLink: " + e.HelpLink);
        Log.Info("InnerException: " + e.InnerException);
        Log.Info("Message: " + e.Message);
        Log.Info("Source: " + e.Source);
        Log.Info("StackTrace: " + e.StackTrace);
        Log.Info("TargetSite: " + e.TargetSite);
      }
      finally
      {
        // Focus on list
        // ##################################################
        if (lstItemList.Count > 0)
        {
          GUI.LooseFocus();
          lstItemList.Focus = true;
        }
        // ##################################################
      }
    }

  }
}