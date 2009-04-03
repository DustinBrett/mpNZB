using System;
using System.Timers;
using System.Xml;

using MediaPortal.GUI.Library;
using MediaPortal.Profile;

namespace mpNZB
{
  public class mpNZB : GUIWindow, ISetupForm
  {

    #region SkinControlAttribute

    [SkinControlAttribute(1)]
    protected GUIButtonControl btnRefreshFeed = null;
    [SkinControlAttribute(2)]
    protected GUIButtonControl btnSelectFeed = null;
    [SkinControlAttribute(3)]
    protected GUIButtonControl btnSearch = null;
    [SkinControlAttribute(4)]
    protected GUIButtonControl btnJobQueue = null;
    [SkinControlAttribute(5)]
    protected GUIToggleButtonControl btnPause = null;

    [SkinControlAttribute(50)]
    protected GUIListControl lstItems = null;

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
      return 3847;
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

    #region Definitions
    
    private Sites.iSite Site;
    private Clients.iClient Client;
    private Clients.statusTimer Status = new Clients.statusTimer();

    private mpFunctions Dialogs = new mpFunctions();

    #endregion

    #region On<Action>

    public override bool Init()
    {
      // Set "Status" label
      GUIPropertyManager.SetProperty("#Status", "Idle.");

      // Load skin
      return Load(GUIGraphicsContext.Skin + @"\mpNZB.xml");
    }

    public void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
    {
      Client.Status(Status, btnPause);
    }

    protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
    {
      if (control == btnRefreshFeed)
      {
        Dialogs.Wait(true);
        fncReadRSS(Site.FeedURL, lstItems);
        Dialogs.Wait(false);
      }
      if (control == btnSelectFeed) { fncSelectFeed(false); }
      if (control == btnSearch)     { fncSelectFeed(true); }
      if (control == btnJobQueue)   { Client.Queue(lstItems, this); }
      if (control == btnPause)      { Client.Pause(btnPause.Selected, Status); }
      if (control == lstItems)
      {
        switch (lstItems.ListItems[lstItems.SelectedListItemIndex].ItemId)
        {

          // ID Types
          // ==============================
          // 1 = Direct NZB Link
          // 2 = Job Queue Item
          // 3 = Download File Link
          // 4 = Direct Newzbin ID Link
          // ==============================

          case 1:
          case 3:
          case 4:
            Client.Download(lstItems.ListItems[lstItems.SelectedListItemIndex], Site, Status);
            break;
          case 2:
            Client.Delete(lstItems.ListItems[lstItems.SelectedListItemIndex], lstItems, this);
            break;
        }
      }
      base.OnClicked(controlId, control, actionType);
    }
    
    protected override void OnPageLoad()
    {
      // Disable "Refresh Feed" button
      btnRefreshFeed.Disabled = true;

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
            Client = new Clients.SABnzbd(mpSettings.GetValue("#Client", "Host"), mpSettings.GetValue("#Client", "Port"), mpSettings.GetValueAsBool("#Client", "CatSelect", false), mpSettings.GetValueAsBool("#Client", "Auth", false), mpSettings.GetValue("#Client", "Username"), mpSettings.GetValue("#Client", "Password"));
            if (Client.Version().Length == 0)
            {
              Dialogs.OK("SABnzbd connection failed.", "Client Status");
              return;
            }
            break;
          }
      }
      // ##################################################

      // Unload Settings
      mpSettings.Dispose();

      // Start Timer
      // ##################################################
      Status.KeepAlive = false;
      Status.tmrTimer = new Timer();
      Status.tmrTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
      Status.tmrTimer.Interval = (mpSettings.GetValueAsInt("#Plugin", "UpdateFrequency", 1) * 1000);
      Status.tmrTimer.Enabled = true;
      // ##################################################

      // Update Status
      Client.Status(Status, btnPause);
    }

    protected override void OnPageDestroy(int newWindowId)
    {
      Status.KeepAlive = Status.tmrTimer.Enabled;
      base.OnPageDestroy(newWindowId);
    }

    #endregion

    #region Functions

    private void fncReadRSS(string strURL, GUIListControl lstItemList)
    {
      try
      {
        // Load RSS feed
        // ##################################################
        XmlDocument xmlDoc = new XmlDocument();
        XmlTextReader xmlTextReader = new XmlTextReader(strURL);
        xmlDoc.Load(xmlTextReader);
        // ##################################################

        // Read RSS feed
        // ##################################################
        if ((xmlDoc.ChildNodes[1].Name == "rss") && (xmlDoc.ChildNodes[1].Attributes["version"].InnerText == "2.0"))
        {
          lstItemList.Clear();

          XmlNodeList nodeList = xmlDoc.SelectNodes("rss/channel/item");         

          foreach (XmlNode nodeItem in nodeList)
          {
            Site.AddItem(nodeItem, lstItemList);
          }

          GUIPropertyManager.SetProperty("#Status", "Item Count (" + nodeList.Count + ")");
        }
        else
        {
          GUIPropertyManager.SetProperty("#Status", "RSS parsing error.");
        }
        // ##################################################
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
          this.LooseFocus();
          lstItemList.Focus = true;
        }
        // ##################################################
      }
    }

    private void fncSelectFeed(bool bolSearch)
    {
      // Create Feed List
      // ##################################################
      Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");
      string strList = mpSettings.GetValue("#Lists", (bolSearch ? "SearchList" : "FeedList"));
      if (strList.Length == 0)
      {
        GUIPropertyManager.SetProperty("#Status", "Site list is empty.");
        return;
      }
      string[] strSites = strList.Split((char)0);
      mpSettings.Dispose();      
      // ##################################################

      // Select Site
      // ##################################################
      switch (Dialogs.Menu(strSites, "Select Site"))
      {        
        case "Newzbin":   Site = new Sites.Newzbin();   break;
        case "NZBIndex":  Site = new Sites.NZBIndex();  break;
        case "NZBMatrix": Site = new Sites.NZBMatrix(); break;
        case "NZBsRus":   Site = new Sites.NZBsRus();   break;
        case "TvNZB":     Site = new Sites.TvNZB();     break;
      }
      // ##################################################

      // SetFeed or Search
      // ##################################################
      if (bolSearch)
      {
        Site.Search();
      }
      else
      {
        Site.SetFeed();
      }
      // ##################################################

      // Update List
      // ##################################################
      if (Site.FeedURL.Length > 0)
      {
        Dialogs.Wait(true);
        fncReadRSS(Site.FeedURL, lstItems);
        btnRefreshFeed.Disabled = false;
        GUIPropertyManager.SetProperty("#PageTitle", Site.SiteName + " - " + Site.FeedName);
        Dialogs.Wait(false);
      }
      // ##################################################
    }

    #endregion

  }
}