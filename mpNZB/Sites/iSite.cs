using System.Xml;

using MediaPortal.GUI.Library;

namespace mpNZB.Sites
{
  interface iSite
  {
    string SiteName
    {
      get;
    }
    string FeedName
    {
      get;
      set;
    }
    string FeedURL
    {
      get;
      set;
    }

    void SetFeed();
    void Search();
    void AddItem(XmlNode node, GUIListControl lstList);
  }
}