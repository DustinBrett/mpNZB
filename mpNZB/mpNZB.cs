using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using MediaPortal.GUI.Library;
using MediaPortal.Profile;
using MediaPortal.Configuration;
using System.Timers;

namespace mpNZB
{
  [PluginIcons("mpNZB.Resources.logo_enabled.png", "mpNZB.Resources.logo_disabled.png")]
  public class mpNZB : GUIWindow, ISetupForm
  {
    public const int WINDOW_ID = 3847;
    public enum ClientView
    {
        None = 0,
        Queue,
        History,
        Feeds,
        Groups,
        Search
    }

    #region SkinControlAttribute

    [SkinControlAttribute(1)]
    protected GUIButtonControl btnRefresh = null;
    [SkinControlAttribute(2)]
    protected GUIButtonControl btnFeeds = null;
    [SkinControlAttribute(3)]
    protected GUIButtonControl btnGroups = null;
    [SkinControlAttribute(4)]
    protected GUIButtonControl btnSearch = null;
    [SkinControlAttribute(5)]
    protected GUIButtonControl btnJobQueue = null;
    [SkinControlAttribute(7)]
    protected GUIButtonControl btnHistory = null;    

    [SkinControlAttribute(6)]
    protected GUIToggleButtonControl btnPause = null;

    [SkinControlAttribute(50)]
    protected GUIListControl lstItems = null;

    #endregion

    #region Timer

    private Timer tmrStatus = new Timer();
    private Timer tmrPause = null;

    private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
    {
      Client.Status();
       
      // Update queue
      if (Client.ActiveView == (int)mpNZB.ClientView.Queue)
      {
          Client.Queue(lstItems, this, false);
      }
    }

    // Unpause queue after timeout
    private void OnPauseTimer(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (Client.Paused)
        {
            Client.Pause(false);
            btnPause.Selected = false;
        }

        tmrPause.Stop();
        tmrPause = null;
    }

    #endregion

    #region ISetupForm Members

    // Returns the name of the plugin which is shown in the plugin menu
    public string PluginName()
    {
      return "mpNZB";
    }

    // Returns the description of the plugin is shown in the plugin menu
    public string Description()
    {
      return "NZB Downloader";
    }

    // Returns the author of the plugin which is shown in the plugin menu
    public string Author()
    {
      return "Dustin Brett";
    }

    // show the setup dialog
    public void ShowPlugin()
    {
      frmSetup frmSetupDialog = new frmSetup();
      frmSetupDialog.Show();
    }

    // Indicates whether plugin can be enabled/disabled
    public bool CanEnable()
    {
      return true;
    }

    // Get Windows-ID
    public int GetWindowId()
    {
      // WindowID of windowplugin belonging to this setup
      // enter your own unique code
      return WINDOW_ID;
    }

    // Indicates if plugin is enabled by default;
    public bool DefaultEnabled()
    {
      return true;
    }

    // indicates if a plugin has it's own setup screen
    public bool HasSetup()
    {
      return true;
    }

    /// <summary>
    /// If the plugin should have it's own button on the main menu of MediaPortal then it
    /// should return true to this method, otherwise if it should not be on home
    /// it should return false
    /// </summary>
    /// <param name="strButtonText">text the button should have</param>
    /// <param name="strButtonImage">image for the button, or empty for default</param>
    /// <param name="strButtonImageFocus">image for the button, or empty for default</param>
    /// <param name="strPictureImage">subpicture for the button or empty for none</param>
    /// <returns>true : plugin needs it's own button on home
    /// false : plugin does not need it's own button on home</returns>

    public bool GetHome(out string strButtonText, out string strButtonImage,
      out string strButtonImageFocus, out string strPictureImage)
    {
      // ##################################################
      // Load button text
      // ##################################################
      Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");
      strButtonText = mpSettings.GetValue("#Plugin", "DisplayName");
      if (strButtonText.Length == 0) { strButtonText = "mpNZB"; }
      mpSettings.Dispose();
      // ##################################################

      strButtonImage = String.Empty;
      strButtonImageFocus = String.Empty;
      strPictureImage = String.Empty;
      return true;
    }

    // With GetID it will be an window-plugin / otherwise a process-plugin
    // Enter the id number here again
    public override int GetID
    {
      get
      {
        return 3847;
      }

      set
      {
      }
    }

    #endregion

    #region Definition

    private Sites Site;
    private Clients.iClient Client;

    private mpFunctions MP = new mpFunctions();

    #endregion

    #region On<Action>

    public override bool Init()
    {
      // Load skin
      return Load(GUIGraphicsContext.Skin + @"\mpNZB.xml");
    }

    protected override void OnClicked(int controlId, GUIControl control, MediaPortal.GUI.Library.Action.ActionType actionType)
    {
      if (control == btnRefresh)
      {
        GUIPropertyManager.SetProperty("#Status", "Processing...");
        GUIWindowManager.Process();

        ReadRSS(Site.FeedURL, lstItems, Site.Username, Site.Password);
      }
      if (control == btnFeeds) { SelectSite("Feeds"); Client.ActiveView = (int)ClientView.Feeds; }
      if (control == btnGroups) { SelectSite("Groups"); Client.ActiveView = (int)ClientView.Groups; }
      if (control == btnSearch) { SelectSite("Search"); Client.ActiveView = (int)ClientView.Search; }
      if (control == btnJobQueue) { Client.Queue(lstItems, this, true); Client.ActiveView = (int)ClientView.Queue; }
      if (control == btnHistory) { Client.History(lstItems, this); Client.ActiveView = (int)ClientView.History; }      
      if (control == btnPause)    { Client.Pause(btnPause.Selected); }
      if (control == lstItems)
      {
        switch (lstItems.ListItems[lstItems.SelectedListItemIndex].ItemId)
        {

          // ID Types
          // ====================
          // 1 = NZB
          // 2 = Newzbin ID
          // 3 = Queue Item
          // 4 = History Item
          // ====================

          case 1:
          case 2:
            Client.Download(lstItems.ListItems[lstItems.SelectedListItemIndex]);
            break;
          case 3:
            Client.QueueItem(lstItems, this);
            break;
          case 4:
            MP.Text(lstItems.ListItems[lstItems.SelectedListItemIndex].DVDLabel, "Job History");
            break;
        }
      }
      base.OnClicked(controlId, control, actionType);
    }

    public override void OnAction(MediaPortal.GUI.Library.Action action)
    {
        // Context menu on pause button
        if (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_CONTEXT_MENU
            && btnPause.IsFocused)
        {
            List<GUIListItem> _Items = new List<GUIListItem>();

            _Items.Add(new GUIListItem("5 minutes"));
            _Items.Add(new GUIListItem("10 minutes"));
            _Items.Add(new GUIListItem("15 minutes"));
            _Items.Add(new GUIListItem("30 minutes"));
            _Items.Add(new GUIListItem("45 minutes"));
            _Items.Add(new GUIListItem("60 minutes"));
            _Items.Add(new GUIListItem("90 minutes"));
            _Items.Add(new GUIListItem("2 hours"));

            GUIListItem _Menu = MP.Menu(_Items, "Pause for ...");
            if (_Menu != null)
            {
                int minutesToPause = 0;

                switch (_Menu.Label)
                {
                    case "5 minutes":
                        minutesToPause = 5;
                        break;

                    case "10 minutes":
                        minutesToPause = 10;
                        break;

                    case "15 minutes":
                        minutesToPause = 15;
                        break;

                    case "30 minutes":
                        minutesToPause = 35;
                        break;

                    case "45 minutes":
                        minutesToPause = 45;
                        break;

                    case "60 minutes":
                        minutesToPause = 60;
                        break;

                    case "90 minutes":
                        minutesToPause = 90;
                        break;

                    case "2 hours":
                        minutesToPause = 120;
                        break;
                }

                PauseForTimespan(minutesToPause);
            }
        }

        base.OnAction(action);
    }
    
    protected override void OnPageLoad()
    {
      try
      {
        // Set "Status" label
        GUIPropertyManager.SetProperty("#Status", "Idle");

        // Disable "Refresh Feed" button
        btnRefresh.Disabled = true;

        // Load Settings
        Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");

        // Setup Page Title
        // ##################################################
        string strPageTitle = mpSettings.GetValue("#Plugin", "DisplayName");
        GUIPropertyManager.SetProperty("#PageTitle", ((strPageTitle.Length == 0) ? "mpNZB" : strPageTitle));
        // ##################################################

        // Setup Client
        // ##################################################
        switch (mpSettings.GetValue("#Client", "Grabber"))
        {
          case "SABnzbd":
            {
              Client = new Clients.SABnzbd(mpSettings.GetValue("#Client", "Host"), mpSettings.GetValue("#Client", "Port"), mpSettings.GetValueAsBool("#Client", "CatSelect", false), mpSettings.GetValueAsBool("#Client", "Auth", false), mpSettings.GetValue("#Client", "Username"), mpSettings.GetValue("#Client", "Password"), mpSettings.GetValue("#Client", "APIKey"), mpSettings.GetValueAsInt("#Plugin", "UpdateFrequency", 1), mpSettings.GetValueAsBool("#Plugin", "Notifications", false), mpSettings.GetValueAsInt("#Plugin", "AutoHideSeconds", 0));

              string strVersion = Client.Version();

              if (strVersion.Length != 0)
              {
                GUIPropertyManager.SetProperty("#Status", "Idle (" + strVersion + ")");
              }
              else
              {
                GUIPropertyManager.SetProperty("#Status", "SABnzbd connection failed");
                return;
              }

              break;
            }
        }

        
        tmrStatus = new Timer();
        tmrStatus.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
        tmrStatus.Interval = (mpSettings.GetValueAsInt("#Plugin", "UpdateFrequency", 1) * 1000);
        tmrStatus.Enabled = true;
        
        // ##################################################

        // Unload Settings
        mpSettings.Dispose();
          

        // Update Status
        Client.Visible = true;
        Client.Status();
        if (Client.Paused) { btnPause.Selected = true; }

        // Load queue
        Client.Queue(lstItems, this, true); 
        Client.ActiveView = (int)ClientView.Queue;

        // Start with parameter
        // Working at the moment:
        // "search:string to search"
        string loadParam = null;

        // check if running version of mediaportal supports loading with parameter and handle _loadParameter
        System.Reflection.FieldInfo fi = typeof(GUIWindow).GetField("_loadParameter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (fi != null)
        {
            loadParam = (string)fi.GetValue(this);
        }

        if (!string.IsNullOrEmpty(loadParam)) 
        {
            Log.Debug("[MPNZB] Loading with param: " + loadParam);
            string search = Regex.Match(loadParam, "search:([^|]*)").Groups[1].Value;
            if (!string.IsNullOrEmpty(search))
            {
                SelectSite("Search", search);
            }
        }

      }
      catch (Exception e) { MP.Error(e); }
    }

    protected override void OnPageDestroy(int newWindowId)
    {
      base.OnPageDestroy(newWindowId);
      Client.Visible = false;
      Client.ActiveView = (int)ClientView.None;
    }

    protected override void OnShowContextMenu()
    {
      if ((lstItems.Count > 0) && (lstItems.ListItems[lstItems.SelectedListItemIndex].ItemId <= 3) && !btnPause.IsFocused)
      {
        List<GUIListItem> _Items = new List<GUIListItem>();

        if ((lstItems.ListItems[lstItems.SelectedListItemIndex].ItemId == 1) || (lstItems.ListItems[lstItems.SelectedListItemIndex].ItemId == 2))
        {
          _Items.Add(new GUIListItem("Sort By"));
        }

        if (lstItems.ListItems[lstItems.SelectedListItemIndex].DVDLabel.Length > 0)
        {
          _Items.Add(new GUIListItem("Information"));
        }

        if (lstItems.ListItems[lstItems.SelectedListItemIndex].FileInfo.Name.Length > 0)
        {
          _Items.Add(new GUIListItem("View NFO"));
        }

        GUIListItem _Menu = MP.Menu(_Items, "Menu");
        if (_Menu != null)
        {
          switch (_Menu.Label)
          {
            case "Sort By":
              List<GUIListItem> _NDS = new List<GUIListItem>();

              _NDS.Add(new GUIListItem("Name"));
              _NDS.Add(new GUIListItem("Date"));
              _NDS.Add(new GUIListItem("Size"));

              GUIListItem _Method = MP.Menu(_NDS, "Sort By");
              if (_Method != null)
              {
                List<GUIListItem> _AscDes = new List<GUIListItem>();

                _AscDes.Add(new GUIListItem("Ascending"));
                _AscDes.Add(new GUIListItem("Descending"));

                GUIListItem _Order = MP.Menu(_AscDes, "Sort Order");
                if (_Order != null)
                {
                  switch (_Method.Label)
                  {
                    case "Name":
                      lstItems.ListItems.Sort(delegate(GUIListItem _Item1, GUIListItem _Item2) { return ((_Order.Label == "Ascending") ? _Item1.Label.CompareTo(_Item2.Label) : _Item2.Label.CompareTo(_Item1.Label)); });
                      break;
                    case "Date":
                      lstItems.ListItems.Sort(delegate(GUIListItem _Item1, GUIListItem _Item2) { return ((_Order.Label == "Ascending") ? _Item1.FileInfo.CreationTime.CompareTo(_Item2.FileInfo.CreationTime) : _Item2.FileInfo.CreationTime.CompareTo(_Item1.FileInfo.CreationTime)); });
                      break;
                    case "Size":
                      lstItems.ListItems.Sort(delegate(GUIListItem _Item1, GUIListItem _Item2) { return ((_Order.Label == "Ascending") ? _Item1.FileInfo.Length.CompareTo(_Item2.FileInfo.Length) : _Item2.FileInfo.Length.CompareTo(_Item1.FileInfo.Length)); });
                      break;
                  }
                  GUIPropertyManager.SetProperty("#Status", "Sorted by " + _Method.Label + " (" + _Order.Label + ")");
                }
              }
              break;
            case "Information":
              MP.Text(lstItems.ListItems[lstItems.SelectedListItemIndex].DVDLabel, ((lstItems.ListItems[lstItems.SelectedListItemIndex].ItemId == 3) ? "Job Information" : "NZB Information"));
              break;
            case "View NFO":
              MP.Text(new WebClient().DownloadString(lstItems.ListItems[lstItems.SelectedListItemIndex].FileInfo.Name), "View NFO");
              break;
          }
        }
      }
    }

    #endregion

    #region Functions

    private void ReadRSS(List<string> _URL, GUIListControl _List, string _Username, string _Password)
    {
      try
      {
        int intItemCount = 0;
        _List.Clear();

        foreach (string URL in _URL)
        {
          HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(URL);
          webReq.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");

          if ((_Username.Length > 0) && (_Password.Length > 0)) { webReq.Credentials = new NetworkCredential(_Username, _Password); }

          HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();
          XmlDocument xmlDoc = new XmlDocument();
          xmlDoc.Load(((webResp.ContentEncoding.ToLower().Contains("gzip")) ? new GZipStream(webResp.GetResponseStream(), CompressionMode.Decompress, false) : webResp.GetResponseStream()));
          webResp.Close();

          if (xmlDoc.SelectSingleNode("rss[@version='2.0']") != null)
          {
            XmlNodeList xmlNodes = xmlDoc.SelectNodes("rss/channel/item");
            foreach (XmlNode xmlNode in xmlNodes)
            {
              Site.AddItem(xmlNode, _List);
              intItemCount += 1;
            }
          }
          else
          {
            GUIPropertyManager.SetProperty("#Status", "Error parsing XML");
          }
        }

        _List.ListItems.Sort(delegate(GUIListItem _Item1, GUIListItem _Item2) { return _Item2.FileInfo.CreationTime.CompareTo(_Item1.FileInfo.CreationTime); });
        GUIPropertyManager.SetProperty("#Status", "Found " + intItemCount.ToString() + " Items");
      }
      catch (Exception e) { MP.Error(e); }
      finally
      {
        if (_List.Count > 0)
        {
          this.LooseFocus();
          _List.Focus = true;
        }
      }
    }

    private void SelectSite(string strType) 
    {
        SelectSite(strType, null);
    }

    private void SelectSite(string strType, string parameter)
    {
      try
      {
        Site = new Sites(strType);

        if (Site.SiteName.Length != 0)
        {
          switch (strType)
          {
            case "Feeds":
              Site.SetFeed();
              break;
            case "Groups":
              Site.SetGroup();
              break;
            case "Search":
              Site.SetSearch(parameter);
              break;
          }

          if (Site.FeedURL.Count > 0)
          {
            GUIPropertyManager.SetProperty("#Status", "Processing...");
            GUIWindowManager.Process();

            ReadRSS(Site.FeedURL, lstItems, Site.Username, Site.Password);

            btnRefresh.Disabled = false;
            GUIPropertyManager.SetProperty("#PageTitle", Site.SiteName + " [" + Site.FeedName + "]");
          }
        }
      }
      catch (Exception e) { MP.Error(e); }
    }

    /// <summary>
    /// Pause Queue for x minutes
    /// </summary>
    /// <param name="minutes">Minutes to pause the queue</param>
    private void PauseForTimespan(int minutes)
    {
        if (tmrPause != null)
        {
            tmrPause.Stop();
            tmrPause = null;
        }

        // Pause client
        Client.Pause(true);
        btnPause.Selected = true;

        // Wait x minutes, then resume
        tmrPause = new Timer();
        tmrPause.Elapsed += new System.Timers.ElapsedEventHandler(OnPauseTimer);
        tmrPause.Interval = minutes * 1000 * 60;
        tmrPause.Enabled = true;
    }

    #endregion

  }
}