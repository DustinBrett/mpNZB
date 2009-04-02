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

    [SkinControlAttribute(60)]
    protected GUIButtonControl btnPrev = null;
    [SkinControlAttribute(61)]
    protected GUIButtonControl btnNext = null;

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

    private mpFunctions Dialogs = new mpFunctions();
    private Sites.siteFunctions Site = new Sites.siteFunctions();
    private Clients.iClient Client;
    private Clients.statusTimer Status = new Clients.statusTimer();

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
        Dialogs.Wait();
        switch (Site.strSite)
        {
          case "Binsearch":
            Site.fncBinsearch(lstItems, this, btnNext);
            break;
          default:
            fncReadRSS(Site.strFeedURL, lstItems);
            break;
        }        
        Dialogs.bolWaiting = false;
      }
      if (control == btnSelectFeed) { fncSelectFeed(); }
      if (control == btnSearch)     { fncSearchFeed(); }
      if (control == btnJobQueue)   { Client.Queue(lstItems, this); }
      if (control == btnPause)      { Client.Pause(btnPause.Selected, Status); }
      if (control == btnPrev)
      {        
        if (Site.intPageNumber == 0) { return; }
        if (Site.intPageNumber > 0) { Site.intPageNumber -= 1; }
        Dialogs.Wait();
        Site.fncBinsearch(lstItems, this, btnNext);
        Dialogs.bolWaiting = false;
      }
      if (control == btnNext)
      {
        Site.intPageNumber += 1;
        Dialogs.Wait();
        Site.fncBinsearch(lstItems, this, btnNext);
        Dialogs.bolWaiting = false;
      }
      if (control == lstItems)
      {
        switch (lstItems.ListItems[lstItems.SelectedListItemIndex].ItemId)
        {
          case 1: Client.Download(lstItems, Site, Status); break;
          case 2: Client.Delete(lstItems, this); break;
        }
      }
      base.OnClicked(controlId, control, actionType);
    }
    
    protected override void OnPageLoad()
    {
      // Disable "Refresh Feed" button
      btnRefreshFeed.Disabled = true;
      btnNext.Visible = false;
      btnPrev.Visible = false;

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

      // Setup Cookies
      // ##################################################
      Site.Newzbin_NzbSessionID = mpSettings.GetValue("#Cookies", "Newzbin_NzbSessionID");
      Site.Newzbin_NzbSmoke = mpSettings.GetValue("#Cookies", "Newzbin_NzbSmoke");
      Site.NZBMatrix_uid = mpSettings.GetValue("#Cookies", "NZBMatrix_uid");
      Site.NZBMatrix_pass = mpSettings.GetValue("#Cookies", "NZBMatrix_pass");
      Site.NZBsRus_uid = mpSettings.GetValue("#Cookies", "NZBsRus_uid");
      Site.NZBsRus_pass = mpSettings.GetValue("#Cookies", "NZBsRus_pass");
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

          string strSize = String.Empty;

          foreach (XmlNode nodeItem in nodeList)
          {
            if (Site.strSite == "Newzbin")
            {
              double dblSize;              
              double.TryParse(nodeItem["report:size"].InnerText, out dblSize);
              strSize =  string.Format("{0:0.00}", Math.Round((dblSize / 1024) / 1024, 2)) + " MB";
            }
            else if (Site.strSite == "NZBMatrix")
            {
              string strTemp = nodeItem["description"].InnerText.Replace(" ", String.Empty);
              string strSizeText = "<b>Size:</b>".ToLower();
              int intSizePOS = strTemp.ToLower().IndexOf(strSizeText) + strSizeText.Length;
              strSize = strTemp.Substring(intSizePOS, strTemp.IndexOf("<", intSizePOS) - intSizePOS);
            }
            else if (Site.strSite == "NZBsRus")
            {
              string strTemp = nodeItem["description"].InnerText.Replace(" ", String.Empty);
              string strSizeText = "<b>Size:</b>".ToLower();
              int intSizePOS = strTemp.ToLower().IndexOf(strSizeText) + strSizeText.Length;
              strSize = strTemp.Substring(intSizePOS, strTemp.IndexOf("(", intSizePOS) - intSizePOS);
            }
            Dialogs.AddItem(lstItemList, nodeItem["title"].InnerText, strSize, nodeItem[((Site.strSite == "Newzbin") ? "report:id" : "link")].InnerText, 1);
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

    private void fncSearchFeed()
    {
      // Create Site List
      // ##################################################
      string strSiteList = "Binsearch";
      string[] strSites = strSiteList.Split((char)0);
      // ##################################################

      // Select Site/Feed
      // ##################################################
      string strResult = String.Empty;
      Site.strSite = String.Empty;
      Site.strSite = Dialogs.Menu(strSites, "Select Site");
      switch (Site.strSite)
      {
        case "Binsearch":
          strResult = Dialogs.Keyboard();
          if (strResult.Length > 0)
          {
            Dialogs.Wait();
            Site.strSearchString = strResult;
            Site.fncBinsearch(lstItems, this, btnNext);            
          }
          break;
      }
      // ##################################################

      // Update List
      // ##################################################
      if (strResult.Length > 0)
      {
        Site.intPageNumber = 0;
        btnRefreshFeed.Disabled = false;
        btnNext.Visible = true;
        btnPrev.Visible = true;
        GUIPropertyManager.SetProperty("#PageTitle", Site.strSite + " - " + strResult);
      }
      // ##################################################

      // Stop Waiting
      Dialogs.bolWaiting = false;
    }

    private void fncSelectFeed()
    {
      // Create Site List
      // ##################################################
      string strSiteList = "TvNZB";
      Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");
      if (mpSettings.GetValueAsBool("#Sites", "Newzbin_auth", false))   { strSiteList += (char)0 + "Newzbin"; }
      if (mpSettings.GetValueAsBool("#Sites", "NZBMatrix_auth", false)) { strSiteList += (char)0 + "NZBMatrix"; }
      if (mpSettings.GetValueAsBool("#Sites", "NZBsRus_auth", false))   { strSiteList += (char)0 + "NZBsRus"; }
      mpSettings.Dispose();
      string[] strSites = strSiteList.Split((char)0);
      // ##################################################

      // Select Site/Feed
      // ##################################################
      string strResult = String.Empty;
      Site.strSite = String.Empty;
      Site.strSite = Dialogs.Menu(strSites, "Select Site");
      switch (Site.strSite)
      {
        case "TvNZB":
          strResult = Dialogs.Menu(new string[] { "New Files", "All Files", "Old Files" }, "Select Feed");
          Dialogs.Wait();
          switch (strResult)
          {
            case "New Files": Site.strFeedURL = "http://www.tvnzb.com/tvnzb_new.rss"; break;
            case "All Files": Site.strFeedURL = "http://www.tvnzb.com/tvnzb.rss"; break;
            case "Old Files": Site.strFeedURL = "http://www.tvnzb.com/tvnzb_old.rss"; break;
          }
          break;
        case "Newzbin":
          strResult = Dialogs.Menu(new string[] { "Everything", "Unknown", "Anime", "Apps", "Books", "Consoles", "Discussions", "Emulation", "Games", "Misc", "Movies", "Music", "PDA", "Resources", "TV" }, "Select Feed");
          if (strResult.Length > 0)
          {
            Dialogs.Wait();
            if (!(Site.Cookie())) { return; }
            Site.strFeedURL = "http://www.newzbin.com/browse/category/p/" + strResult.ToLower() + "/?feed=rss" + "&COOKIE:NzbSmoke=" + Site.Newzbin_NzbSmoke + ";NzbSessionID=" + Site.Newzbin_NzbSessionID;
          }
          break;
        case "NZBMatrix":
          strResult = Dialogs.Menu(new string[] { "All", "Movies", "TV", "Documentaries", "Games", "Apps", "Music", "Anime", "Other" }, "Select Feed");
          if (strResult.Length > 0)
          {
            Dialogs.Wait();
            if (!(Site.Cookie())) { return; }
            Site.strFeedURL = "http://nzbmatrix.com/rss.php" + ((strResult == "All") ? String.Empty : "?cat=" + strResult.ToLower());
          }
          break;
        case "NZBsRus":
          strResult = Dialogs.Menu(new string[] { "Main RSS Feed", "User's Category Selection RSS" }, "Select Feed");
          if (strResult.Length > 0)
          {
            Dialogs.Wait();
            if (!(Site.Cookie())) { return; }
            Site.strFeedURL = Site.fncNZBsRusFeed(strResult, Site.NZBsRus_uid, Site.NZBsRus_pass);
          }
          break;
      }
      // ##################################################

      // Update List
      // ##################################################
      if (strResult.Length > 0)
      {
        GUIPropertyManager.SetProperty("#PageTitle", Site.strSite + " - " + strResult);
        fncReadRSS(Site.strFeedURL, lstItems);
        btnRefreshFeed.Disabled = false;
        btnNext.Visible = false;
        btnPrev.Visible = false;
      }
      // ##################################################

      // Stop Waiting
      Dialogs.bolWaiting = false;
    }

    #endregion

  }
}