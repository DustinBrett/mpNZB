using System;
using System.IO;
using System.Text;
using System.Xml;

using MediaPortal.GUI.Library;

namespace mpNZB
{
  public class mpNZB : GUIWindow, ISetupForm
  {

    #region SkinControlAttribute

    [SkinControlAttribute(1)]
    protected GUIButtonControl buttonRefresh = null;
    [SkinControlAttribute(2)]
    protected GUIButtonControl buttonFeeds = null;
    [SkinControlAttribute(3)]
    protected GUIButtonControl buttonGroups = null;
    [SkinControlAttribute(4)]
    protected GUIButtonControl buttonSearch = null;
    [SkinControlAttribute(5)]
    protected GUIButtonControl buttonClientStatus = null;

    [SkinControlAttribute(50)]
    protected GUIListControl listResults = null;

    #endregion

    #region Declarations

    Sites Site;
    Functions MP = new Functions();

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
      //frmSetup frmSetupDialog = new frmSetup();
      //frmSetupDialog.Show();
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
      return 8357;
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
      strButtonText = MP.GetValueAsString("#Plugin", "DisplayName", String.Empty);
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
        return 8357;
      }

      set
      {
      }
    }

    #endregion

    #region Initialization

    public override bool Init()
    {
      return Load(GUIGraphicsContext.Skin + @"\mpNZB.xml");
    }

    #endregion

    #region Actions

    protected override void OnPageLoad()
    {
      GUIPropertyManager.SetProperty("#Title", MP.GetValueAsString("#Plugin", "DisplayName", String.Empty));
      GUIPropertyManager.SetProperty("#Information", String.Empty);
      GUIPropertyManager.SetProperty("#Status", String.Empty);
    }

    protected override void OnPageDestroy(int newWindowId)
    {
    }

    protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
    {
      if (control == buttonRefresh) ;
      if (control == buttonFeeds) SelectSite("Feeds");
      if (control == buttonGroups) SelectSite("Groups");
      if (control == buttonSearch) SelectSite("Search");
      if (control == buttonClientStatus) ;
      if (control == listResults) GUIPropertyManager.SetProperty("#Information", listResults.ListItems[listResults.SelectedListItemIndex].DVDLabel);

      base.OnClicked(controlId, control, actionType);
    }

    protected override void OnShowContextMenu()
    {
    }

    #endregion

    #region Functions

    private void ReadRSS(string _URI, GUIListControl _List)
    {
      try
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new MemoryStream(ASCIIEncoding.Default.GetBytes(MP.GetURI(_URI))));

        _List.ListItems.Clear();

        foreach (XmlNode xmlNode in xmlDoc.SelectNodes("rss/channel/item"))
        {
          Site.AddItem(xmlNode, _List);
        }

        GUIPropertyManager.SetProperty("#Status", "Found " + _List.Count.ToString() + " Items");
      }
      catch (Exception ex) { MP.Error(ex); }
      finally
      {
        if (_List.Count > 0)
        {
          this.LooseFocus();
          _List.Focus = true;
        }
      }
    }

    private void SelectSite(string _Type)
    {
      try
      {
        Site = new Sites(_Type);

        if (Site.SiteName.Length > 0)
        {
          switch (_Type)
          {
            case "Feeds":
              Site.SetFeed();
              break;
            case "Groups":
              Site.SetGroup();
              break;
            case "Search":
              Site.SetSearch();
              break;
          }

          if (Site.FeedURI.Length > 0)
          {
            GUIPropertyManager.SetProperty("#Status", "Processing...");
            ReadRSS(Site.FeedURI, listResults);
            buttonRefresh.Disabled = false;
          }
        }
      }
      catch (Exception ex) { MP.Error(ex); }
    }

    #endregion

  }
}