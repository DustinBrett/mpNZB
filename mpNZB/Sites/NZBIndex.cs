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
    }

    public void Search()
    {
      FeedName = Dialogs.Keyboard();
      if (FeedName.Length > 0)
      {
        FeedURL = "http://www.nzbindex.com/rss/?q=" + FeedName.Replace(" ", "+") + "&sort=dateTime&max=250";
      }
    }

    public void AddItem(XmlNode Node, GUIListControl lstList)
    {
      string strTemp = Node["description"].InnerText.Replace(" ", String.Empty);
      string strSizeText = "<b>".ToLower();
      int intSizePOS = strTemp.ToLower().IndexOf(strSizeText) + strSizeText.Length;

      Dialogs.AddItem(lstList, Node["title"].InnerText, strTemp.Substring(intSizePOS, strTemp.IndexOf("</b>", intSizePOS) - intSizePOS).Replace("GB", " GB").Replace("MB", " MB"), Node["enclosure"].Attributes["url"].InnerText, 1);
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