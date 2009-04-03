using System;
using System.Xml;

using MediaPortal.GUI.Library;

namespace mpNZB.Sites
{
  class TvNZB:iSite
  {

    #region Definitions

    public string SiteName
    {
      get { return "TvNZB"; }
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

    #region Functions

    private mpFunctions Dialogs = new mpFunctions();

    public void SetFeed()
    {
      FeedName = Dialogs.Menu(new string[] { "New Files", "All Files", "Old Files" }, "Select Feed");

      if (FeedName.Length > 0)
      {
        Dialogs.Wait(true);
        switch (FeedName)
        {
          case "New Files": FeedURL = "http://www.tvnzb.com/tvnzb_new.rss"; break;
          case "All Files": FeedURL = "http://www.tvnzb.com/tvnzb.rss"; break;
          case "Old Files": FeedURL = "http://www.tvnzb.com/tvnzb_old.rss"; break;
        }
        Dialogs.Wait(false);
      }      
    }

    public void Search()
    {
    }

    public void AddItem(XmlNode Node, GUIListControl lstList)
    {
      Dialogs.AddItem(lstList, Node["title"].InnerText, String.Empty, Node["link"].InnerText, 1);
    }

    #endregion

    #region Cookie

    public string Cookie()
    {
      return String.Empty;
    }

    #endregion

  }
}