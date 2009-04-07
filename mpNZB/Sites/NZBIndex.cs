using System;
using System.Xml;

using MediaPortal.GUI.Library;

namespace mpNZB.Sites
{
  class NZBIndex:iSite
  {
    
    #region Definitions

    public string SiteName
    {
      get { return "NZBIndex"; }
    }

    private string _FeedName = String.Empty;
    public string FeedName
    {
      get { return _FeedName; }
      set { _FeedName = value; }
    }

    private string _FeedURL = String.Empty;
    public string FeedURL
    {
      get { return _FeedURL; }
      set { _FeedURL = value; }
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

    #endregion    

    #region Functions

    private mpFunctions Dialogs = new mpFunctions();

    public void SetFeed()
    {
    }

    public void Search()
    {
      FeedName = Dialogs.Keyboard();
      if (FeedName.Length > 0)
      {
        FeedURL = "http://www.nzbindex.com/rss/?q=" + FeedName.Replace(" ", "+") + "&max=250";
      }
    }

    public void AddItem(XmlNode Node, GUIListControl lstList)
    {
      double dblSize = 0;
      double.TryParse(Node["enclosure"].Attributes["length"].InnerText, out dblSize);
      if (dblSize > 0) { dblSize = ((dblSize / 1024) / 1024); }

      Dialogs.AddItem(lstList, Node["title"].InnerText, ((dblSize >= 1024) ? String.Format("{0:#,##0.00 GB}", dblSize / 1024) : String.Format("{0:#,##0.00 MB}", dblSize)), Node["enclosure"].Attributes["url"].InnerText, 1);
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