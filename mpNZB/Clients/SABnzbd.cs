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

    private bool _InPlugin;
    public bool InPlugin
    {
      get { return _InPlugin; }
      set { _InPlugin = value; }
    }

    private string IP = String.Empty;
    private string Port = String.Empty;
    private string Username = String.Empty;
    private string Password = String.Empty;

    private bool CatSelect = false;
    private bool Auth = false;
    private bool Notifications = false;

    public SABnzbd(string strIP, string strPort, bool bolCatSelect, bool bolAuth, string strUsername, string strPassword, int intUpdateFreq, bool bolNotifications)
    {
      IP = strIP;
      Port = strPort;
      
      Username = strUsername;
      Password = strPassword;

      CatSelect = bolCatSelect;
      Auth = bolAuth;

      Notifications = bolNotifications;

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
          strResult = "http://" + IP + ":" + Port + "/sabnzbd/" + _Mode + (((Auth) && ((Username.Length > 0) && (Password.Length > 0))) ? "&" + "ma_username=" + Username + "&" + "ma_password=" + Password : String.Empty) + ((_Command.Length > 0) ? "&" + _Command : String.Empty) + ((_CatSelect) ? "&" + "cat=" + SelectCategory() : String.Empty);
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

    #region Jobs

    List<string> Jobs = new List<string>();

    private void JobCompare(XmlDocument xmlDoc)
    {
      List<string> _Jobs = new List<string>();

      foreach (XmlNode nodeItem in xmlDoc.SelectNodes("queue/jobs/job"))
      {
        _Jobs.Add(nodeItem.SelectSingleNode("filename").InnerText + " (" + nodeItem.SelectSingleNode("id").InnerText + ")");
      }

      if (_Jobs.Count < Jobs.Count)
      {
        foreach (string Job in Jobs)
        {
          if (!(_Jobs.Contains(Job)))
          {
            Jobs.Remove(Job);
            if (Notifications) { MP.OK(Job, "Download Complete"); }
            return;
          }
        }
      }

      Jobs = _Jobs;
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

          if ((intJobCount == 0) || (strPause == "True") || ((InPlugin == false) && (Notifications == false))) { tmrStatus.Enabled = false; }

          if (InPlugin)
          {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            GUIPropertyManager.SetProperty("#Paused", strPause);
            GUIPropertyManager.SetProperty("#KBps", ((intJobCount != 0) ? double.Parse(xmlDoc.SelectSingleNode("queue/kbpersec").InnerText, nfi).ToString("N2") : (0.0).ToString("N2")) + " KB/s");
            GUIPropertyManager.SetProperty("#MBStatus", double.Parse(xmlDoc.SelectSingleNode("queue/mbleft").InnerText, nfi).ToString("N2") + " / " + double.Parse(xmlDoc.SelectSingleNode("queue/mb").InnerText, nfi).ToString("N2") + " MB");
            GUIPropertyManager.SetProperty("#JobCount", intJobCount.ToString());
            GUIPropertyManager.SetProperty("#DiskSpace1", double.Parse(xmlDoc.SelectSingleNode("queue/diskspace1").InnerText, nfi).ToString("N2") + " GB");
            GUIPropertyManager.SetProperty("#DiskSpace2", double.Parse(xmlDoc.SelectSingleNode("queue/diskspace2").InnerText, nfi).ToString("N2") + " GB");
            GUIPropertyManager.SetProperty("#TimeLeft", xmlDoc.SelectSingleNode("queue/timeleft").InnerText + " S");
          }

          JobCompare(xmlDoc);
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
          string strJobInfo;

          foreach (XmlNode nodeItem in xmlDoc.SelectNodes("queue/jobs/job"))
          {
            strJobInfo = "ID: " + nodeItem.SelectSingleNode("id").InnerText + Environment.NewLine + "Filename: " + nodeItem.SelectSingleNode("filename").InnerText + Environment.NewLine + "Message ID: " + nodeItem.SelectSingleNode("msgid").InnerText;

            MP.ListItem(_List, nodeItem.SelectSingleNode("filename").InnerText, double.Parse(nodeItem.SelectSingleNode("mbleft").InnerText, nfi).ToString("N2") + " / " + double.Parse(nodeItem.SelectSingleNode("mb").InnerText, nfi).ToString("N2") + " MB", strJobInfo, DateTime.Now, 0, nodeItem.SelectSingleNode("id").InnerText, 3);
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

    public void QueueItem(GUIListControl _List, GUIWindow _GUI)
    {
      List<GUIListItem> _Items = new List<GUIListItem>();

      if (_List.SelectedListItemIndex != 0)
      {
        _Items.Add(new GUIListItem("Move Up"));
        if (_List.Count > 1)
        {
          _Items.Add(new GUIListItem("Move to Top"));
        }
      }
      if (_List.SelectedListItemIndex != (_List.Count - 1))
      {
        _Items.Add(new GUIListItem("Move Down"));
        if (_List.Count > 1)
        {
          _Items.Add(new GUIListItem("Move to Bottom"));
        }
      }
      _Items.Add(new GUIListItem("Delete Job"));
      _Items.Add(new GUIListItem("Change Category"));

      GUIListItem _Option = MP.Menu(_Items, "Job Options");
      if (_Option != null)
      {
        switch (_Option.Label)
        {
          case "Move Up":
            SendURL(CreateURL("queue/switch?uid1=" + _List.ListItems[_List.SelectedListItemIndex].Path + "&uid2=" + _List.ListItems[_List.SelectedListItemIndex - 1].Path, String.Empty, false));
            break;
          case "Move Down":
            SendURL(CreateURL("queue/switch?uid1=" + _List.ListItems[_List.SelectedListItemIndex].Path + "&uid2=" + _List.ListItems[_List.SelectedListItemIndex + 1].Path, String.Empty, false));
            break;
          case "Move to Top":
            SendURL(CreateURL("queue/switch?uid1=" + _List.ListItems[_List.SelectedListItemIndex].Path + "&uid2=" + _List.ListItems[0].Path, String.Empty, false));
            break;
          case "Move to Bottom":
            SendURL(CreateURL("queue/switch?uid1=" + _List.ListItems[_List.SelectedListItemIndex].Path + "&uid2=" + _List.ListItems[_List.Count - 1].Path, String.Empty, false));
            break;
          case "Delete Job":
            Jobs.Remove(_List.ListItems[_List.SelectedListItemIndex].Label + " (" + _List.ListItems[_List.SelectedListItemIndex].Path + ")");
            SendURL(CreateURL("queue/delete?uid=" + _List.ListItems[_List.SelectedListItemIndex].Path, String.Empty, false));
            break;
          case "Change Category":
            SendURL(CreateURL("queue/change_cat?nzo_id=" + _List.ListItems[_List.SelectedListItemIndex].Path + "&cat=" + SelectCategory(), String.Empty, false));
            break;
        }
        Queue(_List, _GUI);
      }
    }

    public void Download(GUIListItem _Item)
    {
      if (MP.YesNo("Title: " + _Item.Label, "Download file?"))
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