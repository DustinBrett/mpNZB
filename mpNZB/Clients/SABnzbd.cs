using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Timers;
using System.Xml;

using MediaPortal.GUI.Library;

namespace mpNZB.Clients
{
  class SABnzbd : iClient
  {

    #region Init

    private bool _Visible;
    public bool Visible
    {
      get { return _Visible; }
      set { _Visible = value; }
    }

    private bool _Paused;
    public bool Paused
    {
      get { return _Paused; }
      set { _Paused = value; }
    }

    public int ActiveView
    {
        get;
        set;
    }

    private string IP = String.Empty;
    private string Port = String.Empty;
    private string Username = String.Empty;
    private string Password = String.Empty;
    private string APIKey = String.Empty;

    private bool CatSelect = false;
    private bool Auth = false;
    private bool Notifications = false;
    private int AutoHideSeconds = 0;

    public SABnzbd(string strIP, string strPort, bool bolCatSelect, bool bolAuth, string strUsername, string strPassword, string strAPIKey, int intUpdateFreq, bool bolNotifications, int intAutoHideSeconds)
    {
      IP = strIP;
      Port = strPort;
      
      Username = strUsername;
      Password = strPassword;
      APIKey = strAPIKey;

      CatSelect = bolCatSelect;
      Auth = bolAuth;

      Notifications = bolNotifications;
      AutoHideSeconds = intAutoHideSeconds;

      ActiveView = (int)mpNZB.ClientView.None;
      /*
      tmrStatus = new Timer();
      tmrStatus.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
      tmrStatus.Interval = (intUpdateFreq * 1000);
      tmrStatus.Enabled = true;
      */
    }

    #endregion
/*
    #region Timer

    private Timer tmrStatus = new Timer();

    private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
    {
      Status();
       
      // Update queue
      if (ActiveView == (int)mpNZB.ClientView.Queue)
      {
          Queue(
      }
    }

    #endregion
*/
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
          string strCategory = String.Empty;
          if (_CatSelect) { strCategory = SelectCategory(); }

          strResult = "http://" + IP + ":" + Port + "/sabnzbd/" + _Mode + ((APIKey.Length > 0) ? "&apikey=" + APIKey : String.Empty) + (((Auth) && ((Username.Length > 0) && (Password.Length > 0))) ? "&" + "ma_username=" + Username + "&" + "ma_password=" + Password : String.Empty) + ((_Command.Length > 0) ? "&" + _Command : String.Empty) + ((strCategory.Length > 0) ? "&" + "cat=" + strCategory : String.Empty);
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

    private string SelectPriority()
    {
        string strResult = String.Empty;

        List<GUIListItem> Priorities = new List<GUIListItem>();
        Priorities.Add(new GUIListItem("High"));
        Priorities.Add(new GUIListItem("Normal"));
        Priorities.Add(new GUIListItem("Low"));

        GUIListItem _Item = MP.Menu(Priorities, "Select Priority");
        switch (_Item.Label)
        {
            case "High":
                strResult = "1";
                break;
            case "Normal":
                strResult = "0";
                break;
            case "Low":
                strResult = "-1";
                break;
        }

        return strResult;
    }

    #endregion

    #region History

    XmlNodeList _History;

    bool bolHaltNotify = false;

    private void HistoryCheck()
    {
      lock (this)
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new XmlTextReader(CreateURL("api?mode=history", "output=xml", false)));

        XmlNodeList History = xmlDoc.SelectNodes("history/slots/slot");

        if (_History != null)
        {
          foreach(XmlNode Current in History)
          {
            if ((!CompareItem(Current)) && ((Current["status"].InnerText == "Completed") || (Current["status"].InnerText == "Failed")))
            {
              //MP.OK("Name: " + Current["name"].InnerText + Environment.NewLine + ((Current["fail_message"].InnerText.Length > 0) ? "Fail message: " + Current["fail_message"].InnerText + Environment.NewLine : String.Empty) + "Size: " + Current["size"].InnerText + Environment.NewLine + "Category: " + Current["category"].InnerText, Current["status"].InnerText);
                MP.Notify("Name: " + Current["name"].InnerText + Environment.NewLine + ((Current["fail_message"].InnerText.Length > 0) ? "Fail message: " + Current["fail_message"].InnerText + Environment.NewLine : String.Empty) + "Size: " + Current["size"].InnerText + Environment.NewLine + "Category: " + Current["category"].InnerText, Current["status"].InnerText, AutoHideSeconds);
            }
          }
        }

        _History = History;

        bolHaltNotify = (History.Count == 0);
      }
    }

    private bool CompareItem(XmlNode Current)
    {
      foreach (XmlNode Cached in _History)
      {
        if (Current["nzo_id"].InnerText == Cached["nzo_id"].InnerText)
        {
          if ((Current["status"].InnerText != Cached["status"].InnerText) && ((Current["status"].InnerText == "Completed") || (Current["status"].InnerText == "Failed")))
          {
            //MP.OK("Name: " + Current["name"].InnerText + Environment.NewLine + ((Current["fail_message"].InnerText.Length > 0) ? "Fail message: " + Current["fail_message"].InnerText + Environment.NewLine : String.Empty) + "Size: " + Current["size"].InnerText + Environment.NewLine + "Category: " + Current["category"].InnerText, Current["status"].InnerText);
              MP.Notify("Name: " + Current["name"].InnerText + Environment.NewLine + ((Current["fail_message"].InnerText.Length > 0) ? "Fail message: " + Current["fail_message"].InnerText + Environment.NewLine : String.Empty) + "Size: " + Current["size"].InnerText + Environment.NewLine + "Category: " + Current["category"].InnerText, Current["status"].InnerText, AutoHideSeconds);
          }

          return true;
        }
      }

      return false;
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
          _Paused = bool.Parse(xmlDoc.SelectSingleNode("queue/paused").InnerText);

          if (Visible)
          {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            GUIPropertyManager.SetProperty("#Paused", _Paused.ToString());
            GUIPropertyManager.SetProperty("#KBps", ((intJobCount != 0) ? double.Parse(xmlDoc.SelectSingleNode("queue/kbpersec").InnerText, nfi).ToString("N2") : (0.0).ToString("N2")) + " KB/s");
            GUIPropertyManager.SetProperty("#MBStatus", double.Parse(xmlDoc.SelectSingleNode("queue/mbleft").InnerText, nfi).ToString("N2") + " / " + double.Parse(xmlDoc.SelectSingleNode("queue/mb").InnerText, nfi).ToString("N2") + " MB");
            GUIPropertyManager.SetProperty("#JobCount", intJobCount.ToString());
            GUIPropertyManager.SetProperty("#DiskSpace1", double.Parse(xmlDoc.SelectSingleNode("queue/diskspace1").InnerText, nfi).ToString("N2") + " GB");
            GUIPropertyManager.SetProperty("#DiskSpace2", double.Parse(xmlDoc.SelectSingleNode("queue/diskspace2").InnerText, nfi).ToString("N2") + " GB");
            GUIPropertyManager.SetProperty("#TimeLeft", xmlDoc.SelectSingleNode("queue/timeleft").InnerText + " S");
          }

          if (Notifications) { HistoryCheck(); }

          //if (!Visible && (!Notifications || (bolHaltNotify && ((intJobCount == 0) || _Paused)))) { tmrStatus.Enabled = false; }
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "Error parsing XML");
        }
      }
      catch (Exception e) { MP.Error(e); }
    }

    public void Queue(GUIListControl _List, GUIWindow _GUI, bool refocus)
    {
        int selectedIndex = _List.SelectedListItemIndex;

        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new XmlTextReader(CreateURL("api?mode=queue", "output=xml", false)));

            if (xmlDoc.SelectSingleNode("queue/slots") != null)
            {
                if (refocus)
                {
                    _List.Clear();
                }

                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                string strJobInfo;

                int i = 0;
                foreach (XmlNode nodeItem in xmlDoc.SelectNodes("queue/slots/slot"))
                {
                    strJobInfo = "Status: " + nodeItem["status"].InnerText + Environment.NewLine + "Filename: " + nodeItem["filename"].InnerText + Environment.NewLine + "Priority: " + nodeItem["priority"].InnerText + Environment.NewLine + "Category: " + nodeItem["cat"].InnerText + Environment.NewLine + "Percentage: " + nodeItem["percentage"].InnerText + "%";

                    double mbdone = double.Parse(nodeItem["mb"].InnerText, nfi) - double.Parse(nodeItem["mbleft"].InnerText, nfi);

                    if (_List.Count > i && !refocus)
                    {
                        MP.UpdateListItem(i, _List, nodeItem["filename"].InnerText, ((nodeItem["status"].InnerText == "Paused") ? "Paused" : mbdone.ToString("N2") + " / " + double.Parse(nodeItem["mb"].InnerText, nfi).ToString("N2") + " MB"), strJobInfo, DateTime.Now, 0, String.Empty, nodeItem["nzo_id"].InnerText, 3);
                    }
                    else
                    {
                        MP.ListItem(_List, nodeItem["filename"].InnerText, ((nodeItem["status"].InnerText == "Paused") ? "Paused" : mbdone.ToString("N2") + " / " + double.Parse(nodeItem["mb"].InnerText, nfi).ToString("N2") + " MB"), strJobInfo, DateTime.Now, 0, String.Empty, nodeItem["nzo_id"].InnerText, 3);
                    }

                    ++i;
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
            if (_List.Count > 0 && refocus)
            {
                _GUI.LooseFocus();
                _List.Focus = true;
            }
            else if (_List.IsFocused && _List.Count > selectedIndex)
            {
                _List.SelectedListItemIndex = selectedIndex;
            }
        }
    }

    public void History(GUIListControl _List, GUIWindow _GUI)
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new XmlTextReader(CreateURL("api?mode=history", "output=xml", false)));

        if (xmlDoc.SelectSingleNode("history/slots") != null)
        {
          _List.Clear();

          string strItemInfo = String.Empty;

          foreach (XmlNode nodeItem in xmlDoc.SelectNodes("history/slots/slot"))
          {
            DateTime dtPubDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(nodeItem["completed"].InnerText));

            strItemInfo = String.Empty;

            foreach (XmlNode nodeInfo in nodeItem.SelectNodes("stage_log/slot"))
            {
              strItemInfo += ((strItemInfo.Length > 0) ? Environment.NewLine : String.Empty) + nodeInfo["name"].InnerText + ":" + Environment.NewLine;

              foreach (XmlNode nodeProgress in nodeInfo.SelectNodes("actions/item"))
              {
                strItemInfo += nodeProgress.InnerText + Environment.NewLine;
              }
            }

            MP.ListItem(_List, nodeItem["name"].InnerText, nodeItem["status"].InnerText, strItemInfo, dtPubDate, 0, String.Empty, String.Empty, 4);
          }

          GUIPropertyManager.SetProperty("#Status", "History Loaded");
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

      if (_List.ListItems[_List.SelectedListItemIndex].Label2 == "Paused")
      {
        _Items.Add(new GUIListItem("Resume Job"));
      }
      else
      {
        _Items.Add(new GUIListItem("Pause Job"));
      }

      _Items.Add(new GUIListItem("Change Category"));

      _Items.Add(new GUIListItem("Change Priority"));

      GUIListItem _Option = MP.Menu(_Items, "Job Options");
      if (_Option != null)
      {
        switch (_Option.Label)
        {
          case "Move Up":
            SendURL(CreateURL("api?mode=switch&value=" + _List.ListItems[_List.SelectedListItemIndex].Path + "&value2=" + _List.ListItems[_List.SelectedListItemIndex - 1].Path, String.Empty, false));
            break;
          case "Move Down":
            SendURL(CreateURL("api?mode=switch&value=" + _List.ListItems[_List.SelectedListItemIndex].Path + "&value2=" + _List.ListItems[_List.SelectedListItemIndex + 1].Path, String.Empty, false));
            break;
          case "Move to Top":
            SendURL(CreateURL("api?mode=switch&value=" + _List.ListItems[_List.SelectedListItemIndex].Path + "&value2=" + _List.ListItems[0].Path, String.Empty, false));
            break;
          case "Move to Bottom":
            SendURL(CreateURL("api?mode=switch&value=" + _List.ListItems[_List.SelectedListItemIndex].Path + "&value2=" + _List.ListItems[_List.Count - 1].Path, String.Empty, false));
            break;
          case "Delete Job":
            SendURL(CreateURL("api?mode=queue&name=delete&value=" + _List.ListItems[_List.SelectedListItemIndex].Path, String.Empty, false));
            break;
          case "Pause Job":
            SendURL(CreateURL("api?mode=queue&name=pause&value=" + _List.ListItems[_List.SelectedListItemIndex].Path, String.Empty, false));
            break;
          case "Resume Job":
            SendURL(CreateURL("api?mode=queue&name=resume&value=" + _List.ListItems[_List.SelectedListItemIndex].Path, String.Empty, false));
            break;
          case "Change Category":
            SendURL(CreateURL("api?mode=change_cat&value=" + _List.ListItems[_List.SelectedListItemIndex].Path + "&value2=" + SelectCategory(), String.Empty, false));
            break;
          case "Change Priority":
            SendURL(CreateURL("api?mode=queue&name=priority&value=" + _List.ListItems[_List.SelectedListItemIndex].Path + "&value2=" + SelectPriority(), String.Empty, false));
            break;
        }
        Queue(_List, _GUI, true);
      }
    }

    public void Download(GUIListItem _Item)
    {
      if (MP.YesNo("Title: " + _Item.Label, "Download file?", true))
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

        if (xmlDoc.SelectSingleNode("version") != null)
        {
          strResult = xmlDoc.SelectSingleNode("version").InnerText;
        }
      }
      catch (Exception e) { MP.Error(e); }

      return strResult;
    }

    #endregion

  }
}