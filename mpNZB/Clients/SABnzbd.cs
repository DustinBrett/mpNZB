using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Timers;
using System.Xml;

using MediaPortal.GUI.Library;

namespace mpNZB.Clients
{
  class SABnzbd : iClient
  {

    #region Init

    private bool _PluginVisible = true;
    public bool PluginVisible
    {
      get { return _PluginVisible; }
      set { _PluginVisible = value; }
    }

    private string _IP = String.Empty;
    public string IP
    {
      get { return _IP; }
      set { _IP = value; }
    }

    private string _Port = String.Empty;
    public string Port
    {
      get { return _Port; }
      set { _Port = value; }
    }

    private string _Username = String.Empty;
    public string Username
    {
      get { return _Username; }
      set { _Username = value; }
    }

    private string _Password = String.Empty;
    public string Password
    {
      get { return _Password; }
      set { _Password = value; }
    }

    private bool CatSelect;
    private bool Auth;

    public SABnzbd(string strIP, string strPort, bool bolCatSelect, bool bolAuth, string strUsername, string strPassword, int intUpdateFreq)
    {
      IP = strIP;
      Port = strPort;

      Username = strUsername;
      Password = strPassword;

      CatSelect = bolCatSelect;
      Auth = bolAuth;

      tmrStatus = new Timer();
      tmrStatus.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
      tmrStatus.Interval = (intUpdateFreq * 1000);
      tmrStatus.Enabled = true;
    }

    #endregion

    #region Timer

    private Timer tmrStatus = new Timer();

    private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
    {
      Status();
    }

    #endregion

    #region Functions

    private mpFunctions MP = new mpFunctions();

    private string SendURL(string _URL)
    {
      string strResult = String.Empty;

      try
      {
        WebClient webClient = new WebClient();
        strResult = webClient.DownloadString(_URL);
      }
      catch (Exception e) { MP.Error(e); }

      return strResult;
    }

    private string CreateURL(string _Mode, string _Command, bool _CatSelect)
    {
      string strResult = String.Empty;

      try
      {
        if ((IP.Length != 0) && (Port.Length != 0))
        {
          strResult = "http://" + IP + ":" + Port + "/sabnzbd/" + _Mode + (((Auth) && ((Username.Length != 0) && (Password.Length != 0))) ? "&" + "ma_username=" + Username + "&" + "ma_password=" + Password : String.Empty) + ((_Command.Length != 0) ? "&" + _Command : String.Empty) + ((_CatSelect) ? "&" + "cat=" + SelectCategory() : String.Empty);
        }
      }
      catch (Exception e) { MP.Error(e); }

      return strResult;
    }

    private string SelectCategory()
    {
      string strResult = String.Empty;

      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new XmlTextReader(CreateURL("api?mode=get_cats", "output=xml", false)));

        if (xmlDoc.SelectSingleNode("categories") != null)
        {
          List<GUIListItem> Categories = new List<GUIListItem>();

          foreach (XmlNode nodeItem in xmlDoc.SelectSingleNode("categories").ChildNodes)
          {
            Categories.Add(new GUIListItem(nodeItem.InnerText));
          }

          GUIListItem _Item = MP.Menu(Categories, "Select Category");
          if (_Item != null)
          {
            strResult = _Item.Label;
          }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing XML");
        }
      }
      catch (Exception e) { MP.Error(e); }

      return strResult;
    }

    #endregion

    #region Commands

    public void Status()
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new XmlTextReader(CreateURL("api?mode=qstatus", "output=xml", false)));

        if (xmlDoc.SelectSingleNode("queue") != null)
        {
          int intJobCount = int.Parse(xmlDoc.SelectSingleNode("queue/noofslots").InnerText);
          string strPause = xmlDoc.SelectSingleNode("queue/paused").InnerText;

          if ((intJobCount == 0) || (strPause == "True") || (PluginVisible = false)) { tmrStatus.Enabled = false; }

          NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

          GUIPropertyManager.SetProperty("#Paused", strPause);
          GUIPropertyManager.SetProperty("#KBps", ((intJobCount != 0) ? double.Parse(xmlDoc.SelectSingleNode("queue/kbpersec").InnerText, nfi).ToString("N2") : (0.0).ToString("N2")) + " KB/s");
          GUIPropertyManager.SetProperty("#MBStatus", double.Parse(xmlDoc.SelectSingleNode("queue/mbleft").InnerText, nfi).ToString("N2") + " / " + double.Parse(xmlDoc.SelectSingleNode("queue/mb").InnerText, nfi).ToString("N2") + " MB");
          GUIPropertyManager.SetProperty("#JobCount", intJobCount.ToString());
          GUIPropertyManager.SetProperty("#DiskSpace1", double.Parse(xmlDoc.SelectSingleNode("queue/diskspace1").InnerText, nfi).ToString("N2") + " GB");
          GUIPropertyManager.SetProperty("#DiskSpace2", double.Parse(xmlDoc.SelectSingleNode("queue/diskspace2").InnerText, nfi).ToString("N2") + " GB");
          GUIPropertyManager.SetProperty("#TimeLeft", xmlDoc.SelectSingleNode("queue/timeleft").InnerText + " S");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing XML");
        }
      }
      catch (Exception e) { MP.Error(e); }
    }

    public void Queue(GUIListControl _List, GUIWindow _GUI)
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new XmlTextReader(CreateURL("api?mode=qstatus", "output=xml", false)));

        if (xmlDoc.SelectSingleNode("queue") != null)
        {
          _List.Clear();

          NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

          foreach (XmlNode nodeItem in xmlDoc.SelectNodes("queue/jobs/job"))
          {
            MP.ListItem(_List, nodeItem.SelectSingleNode("filename").InnerText, double.Parse(nodeItem.SelectSingleNode("mbleft").InnerText, nfi).ToString("N2") + " / " + double.Parse(nodeItem.SelectSingleNode("mb").InnerText, nfi).ToString("N2") + " MB", DateTime.Now, 0, nodeItem.SelectSingleNode("id").InnerText, 3);
          }

          GUIPropertyManager.SetProperty("#Status", "Queue Loaded");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing XML");
        }
      }
      catch (Exception e) { MP.Error(e); }
      finally
      {
        if (_List.Count > 0)
        {
          _GUI.LooseFocus();
          _List.Focus = true;
        }
      }
    }

    public void Delete(GUIListControl _List, GUIWindow _GUI)
    {
      if (MP.YesNo("Delete file?", _List.ListItems[_List.SelectedListItemIndex].Label))
      {
        SendURL(CreateURL("queue/delete?uid=" + _List.ListItems[_List.SelectedListItemIndex].Path, String.Empty, false));
        if (!(SendURL(CreateURL("api?mode=qstatus", "output=xml", false)).Contains(_List.ListItems[_List.SelectedListItemIndex].Label)))
        {
          Queue(_List, _GUI);
          GUIPropertyManager.SetProperty("#Status", "Job deleted");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error deleting job");
        }
      }
    }

    public void Download(GUIListItem _Item)
    {
      if (MP.YesNo("Download file?", _Item.Label))
      {
        string strResult = String.Empty;

        switch (_Item.ItemId)
        {
          case 1:
            strResult = SendURL(CreateURL("api?mode=addurl", "name=" + _Item.Path.Replace("&", "%26").Replace("=", "%3D").Replace("?", "%3F"), CatSelect));
            break;
          case 2:
            strResult = SendURL(CreateURL("api?mode=addid", "name=" + _Item.Path, CatSelect));
            break;
        }

        if (strResult == "ok\n")
        {
          tmrStatus.Enabled = true;
          GUIPropertyManager.SetProperty("#Status", "Downloading");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error downloading");
        }
      }
    }

    public void Pause(bool _Pause)
    {
      if (_Pause)
      {
        if (SendURL(CreateURL("api?mode=pause", String.Empty, false)) == "ok\n")
        {
          GUIPropertyManager.SetProperty("#Status", "Queue paused");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error pausing queue");
        }
      }
      else
      {
        if (SendURL(CreateURL("api?mode=resume", String.Empty, false)) == "ok\n")
        {
          tmrStatus.Enabled = true;
          GUIPropertyManager.SetProperty("#Status", "Queue resumed");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error resuming queue");
        }
      }
    }

    public string Version()
    {
      string strResult = String.Empty;

      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new XmlTextReader(CreateURL("api?mode=version", "output=xml", false)));

        if (xmlDoc.SelectSingleNode("versions/version") != null)
        {
          strResult = xmlDoc.SelectSingleNode("versions/version").InnerText;
        }
      }
      catch (Exception e) { MP.Error(e); }

      return strResult;
    }

    #endregion

  }  
}