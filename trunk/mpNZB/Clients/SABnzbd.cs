using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;

using MediaPortal.GUI.Library;

namespace mpNZB.Clients
{
  class SABnzbd:iClient
  {

    #region Definitions

    public string strClientIP;
    public string strClientPort;

    public bool bolCategorySelect;
    public bool bolAuthorization;

    public string strUsername;
    public string strPassword;

    private mpFunctions Dialogs = new mpFunctions();

    public SABnzbd(string _strClientIP, string _strClientPort, bool _bolCategorySelect, bool _bolAuthorization, string _strUsername, string _strPassword)
    {
      strClientIP = _strClientIP;
      strClientPort = _strClientPort;

      bolCategorySelect = _bolCategorySelect;
      bolAuthorization = _bolAuthorization;
      
      strUsername = _strUsername;
      strPassword = _strPassword;
    }

    #endregion

    #region Functions

    private string fncSendURL(string strURL)
    {
      string strResponse = String.Empty;
      try
      {
        WebClient webClient = new WebClient();
        strResponse = webClient.DownloadString(strURL);
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

      return strResponse;
    }

    private string fncSendFile(string strURL, Sites.iSite Site, bool IsZip)
    {
      string strResponse = String.Empty;

      try
      {
        string strTempFile = String.Empty;
        string strContentType = String.Empty;

        // Set File Type
        if (IsZip)
        {
            strTempFile = Path.GetTempPath() + Path.GetRandomFileName() + ".zip";
            strContentType = "application/zip";
        }
        else
        {
          strTempFile = Path.GetTempPath() + Path.GetRandomFileName() + ".nzb";
          strContentType = "application/x-nzb";
        }

        // Download File
        WebClient webClient = new WebClient();
        webClient.Headers.Set("Cookie", Site.Cookie());
        webClient.DownloadFile(strURL, strTempFile);

        if (strTempFile.Length > 0)
        {
          // Setup Request Information
          string strBoundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
          byte[] byteNZB = File.ReadAllBytes(strTempFile);
          byte[] byteBoundary = System.Text.Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");
          byte[] byteHeader = System.Text.Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"name\" ;filename=\"" + Path.GetFileName(strTempFile) + "\"\r\nContent-Type: " + strContentType + "\r\n\r\n");

          // Create Web Request
          HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create(fncCreateURL("/sabnzbd/", "api?mode=addfile", "&name=" + strTempFile, bolCategorySelect));
          webReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
          webReq.Method = "POST";
          webReq.KeepAlive = true;
          webReq.ContentLength = byteNZB.Length + byteHeader.Length + (byteBoundary.Length * 2) + 2;

          // Send Stream
          Stream reqStream = webReq.GetRequestStream();
          reqStream.Write(byteBoundary, 0, byteBoundary.Length);
          reqStream.Write(byteHeader, 0, byteHeader.Length);
          reqStream.Write(byteNZB, 0, byteNZB.Length);
          byteBoundary = System.Text.Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "--\r\n");
          reqStream.Write(byteBoundary, 0, byteBoundary.Length);
          reqStream.Close();

          // Get Response
          WebResponse webResp = webReq.GetResponse();
          Stream streamResp = webResp.GetResponseStream();
          StreamReader responseReader = new StreamReader(streamResp);
          strResponse = responseReader.ReadToEnd();
          webResp.Close();
        }
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

      return strResponse;
    }

    private string fncCreateURL(string strPath, string strMode, string strOptions, bool bolCatSelect)
    {
      string strResponse = String.Empty;
      try
      {
        if ((strClientIP.Length > 0) && (strClientPort.Length > 0))
        {
          string strAuthenticate = String.Empty;
          if ((bolAuthorization) && (strUsername.Length > 0) && (strPassword.Length > 0))
          {
            strAuthenticate = "&" + "ma_username=" + strUsername + "&" + "ma_password=" + strPassword;
          }

          string strCategory = String.Empty;
          if (bolCatSelect)
          {
            strCategory = "&cat=" + Categories();
          }

          strResponse = "http://" + strClientIP + ":" + strClientPort + strPath + strMode + strOptions + strAuthenticate + strCategory;
        }
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

      return strResponse;
    }

    #endregion

    #region Commands

    public void Status(statusTimer Status, GUIToggleButtonControl btnButton)
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        XmlTextReader xmlTextReader = new XmlTextReader(fncCreateURL("/sabnzbd/", "api?mode=qstatus", "&output=xml", false));
        xmlDoc.Load(xmlTextReader);

        if (xmlDoc.ChildNodes[1].Name == "queue")
        {
          int intJobCount;
          int.TryParse(xmlDoc["queue"]["noofslots"].InnerText, out intJobCount);

          if (intJobCount == 0)
          {
            Status.tmrTimer.Enabled = false;

            if (Status.KeepAlive)
            {
              Status.KeepAlive = false;
              Dialogs.OK("Downloads complete.", "mNZB Status");
              return;
            }
          }

          if (xmlDoc["queue"]["paused"].InnerText == "True")
          {
            Status.tmrTimer.Enabled = false;
            btnButton.Selected = true;
          }

          if (!(Status.KeepAlive))
          {           
            CultureInfo ciClone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            ciClone.NumberFormat.NumberDecimalSeparator = ".";

            double dblKBps = 0;
            if (intJobCount > 0)
            {
              dblKBps = 0.0;
            }
            dblKBps = double.Parse(xmlDoc["queue"]["kbpersec"].InnerText, ciClone);

            double dblMBLeft = double.Parse(xmlDoc["queue"]["mbleft"].InnerText, ciClone);
            double dblMBTotal = double.Parse(xmlDoc["queue"]["mb"].InnerText, ciClone);
            double dblDiskSpace1 = double.Parse(xmlDoc["queue"]["diskspace1"].InnerText, ciClone);
            double dblDiskSpace2 = double.Parse(xmlDoc["queue"]["diskspace2"].InnerText, ciClone);

            GUIPropertyManager.SetProperty("#Paused", xmlDoc["queue"]["paused"].InnerText);
            GUIPropertyManager.SetProperty("#KBps", String.Format("{0:#,##0.00 KB/s}", dblKBps));
            GUIPropertyManager.SetProperty("#MBStatus", String.Format("{0:#,##0.00}", dblMBLeft) + " / " + String.Format("{0:#,##0.00 MB}", dblMBTotal));
            GUIPropertyManager.SetProperty("#JobCount", intJobCount.ToString());
            GUIPropertyManager.SetProperty("#DiskSpace1", String.Format("{0:#,##0.00 GB}", dblDiskSpace1));
            GUIPropertyManager.SetProperty("#DiskSpace2", String.Format("{0:#,##0.00 GB}", dblDiskSpace2));
            GUIPropertyManager.SetProperty("#TimeLeft", xmlDoc["queue"]["timeleft"].InnerText + " s");
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing status.");
        }
      }
      catch (Exception e)
      {
        Status.tmrTimer.Enabled = false;
        Log.Info("Data: " + e.Data);
        Log.Info("HelpLink: " + e.HelpLink);
        Log.Info("InnerException: " + e.InnerException);
        Log.Info("Message: " + e.Message);
        Log.Info("Source: " + e.Source);
        Log.Info("StackTrace: " + e.StackTrace);
        Log.Info("TargetSite: " + e.TargetSite);
      }
    }

    private string Categories()
    {
      string strCatList = String.Empty;
      string strResult = String.Empty;
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        XmlTextReader xmlTextReader = new XmlTextReader(fncCreateURL("/sabnzbd/", "api?mode=get_cats", "&output=xml", false));
        xmlDoc.Load(xmlTextReader);

        if (xmlDoc.ChildNodes[1].Name == "categories")
        {
          foreach (XmlNode nodeItem in xmlDoc.ChildNodes[1].ChildNodes)
          {
            strCatList += nodeItem.InnerText + (char)0;
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing categories.");
        }
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
        if (strCatList.Length > 0)
        {
          string[] strCategories = strCatList.Substring(0, strCatList.Length - 1).Split((char)0);
          strResult = Dialogs.Menu(strCategories, "Select Category");
        }
      }

      return strResult;
    }

    public void Queue(GUIListControl lstItemList, GUIWindow GUI)
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        XmlTextReader xmlTextReader = new XmlTextReader(fncCreateURL("/sabnzbd/", "api?mode=qstatus", "&output=xml", false));
        xmlDoc.Load(xmlTextReader);

        if (xmlDoc.ChildNodes[1].Name == "queue")
        {
          lstItemList.Clear();

          XmlNodeList nodeList = xmlDoc.SelectNodes("queue/jobs/job");

          double dblMBLeft = 0;
          double dblMBTotal = 0;

          CultureInfo ciClone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
          ciClone.NumberFormat.NumberDecimalSeparator = ".";

          foreach (XmlNode nodeItem in nodeList)
          {
            dblMBLeft = double.Parse(xmlDoc["queue"]["mbleft"].InnerText, ciClone);
            dblMBTotal = double.Parse(xmlDoc["queue"]["mb"].InnerText, ciClone);

            Dialogs.AddItem(lstItemList, nodeItem["filename"].InnerText, String.Format("{0:#,##0.00}", dblMBLeft) + " / " + String.Format("{0:#,##0.00 MB}", dblMBTotal), nodeItem["id"].InnerText, 2);
          }

          GUIPropertyManager.SetProperty("#Status", "Job Count (" + nodeList.Count + ")");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing queue.");
        }
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
        if (lstItemList.Count > 0)
        {
          GUI.LooseFocus();
          lstItemList.Focus = true;
        }
      }
    }

    public void Delete(GUIListItem lstItem, GUIListControl lstItemList, GUIWindow GUI)
    {
      if (Dialogs.YesNo("Delete file?", lstItem.Label))
      {
        fncSendURL(fncCreateURL("/sabnzbd/queue/", "delete?uid=", lstItem.Path, false)).Contains(lstItem.Path);
        if (!(fncSendURL(fncCreateURL("/sabnzbd/", "api?mode=qstatus", "&output=xml", false)).Contains(lstItem.Label)))
        {
          Queue(lstItemList, GUI);
          GUIPropertyManager.SetProperty("#Status", "Job deleted.");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error deleting job.");
        }
      }
    }

    public void Download(GUIListItem lstItem, Sites.iSite Site, Clients.statusTimer Status)
    {
      if (Dialogs.YesNo("Download file?", lstItem.Label))
      {
        string strResult = String.Empty;

        switch (lstItem.ItemId)
        {
          case 1:
            strResult = fncSendURL(fncCreateURL("/sabnzbd/", "api?mode=addurl", "&name=" + lstItem.Path.Replace("?", "%3F").Replace("=", "%3D").Replace("&", "%26"), bolCategorySelect));
            break;
          case 3:
            strResult = fncSendFile(lstItem.Path, Site, true);
            break;
          case 4:
            strResult = fncSendURL(fncCreateURL("/sabnzbd/", "api?mode=addid", "&name=" + lstItem.Path, bolCategorySelect));
            break;
        }
        if (strResult == "ok\n")
        {
          Status.tmrTimer.Enabled = true;
          GUIPropertyManager.SetProperty("#Status", "Downloading NZB.");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error downloading NZB.");
        }
      }
    }

    public void Pause(bool bolPause, Clients.statusTimer Status)
    {
      if (bolPause)
      {
        if (fncSendURL(fncCreateURL("/sabnzbd/", "api?mode=pause", String.Empty, false)) == "ok\n")
        {
          GUIPropertyManager.SetProperty("#Status", "Queue paused.");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error pausing queue.");
        }
      }
      else
      {
        if (fncSendURL(fncCreateURL("/sabnzbd/", "api?mode=resume", String.Empty, false)) == "ok\n")
        {
          Status.tmrTimer.Enabled = true;
          GUIPropertyManager.SetProperty("#Status", "Queue resumed.");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error resuming queue.");
        }
      }
    }

    public string Version()
    {
      string strVersion = String.Empty;

      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        XmlTextReader xmlTextReader = new XmlTextReader(fncCreateURL("/sabnzbd/", "api?mode=version", "&output=xml", false));
        xmlDoc.Load(xmlTextReader);

        strVersion = xmlDoc["versions"]["version"].InnerText;
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

      return strVersion;
    }

    #endregion

  }  
}