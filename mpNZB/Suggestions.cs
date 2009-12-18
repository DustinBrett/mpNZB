using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

using MediaPortal.GUI.Library;

namespace mpNZB
{
  class Suggestions
  {

    public List<GUIListItem> Sites(XmlDocument xmlDoc)
    {
      List<GUIListItem> _Return = new List<GUIListItem>();

      foreach (XmlNode nodeItem in xmlDoc.SelectNodes("sites/suggestion"))
      {
        if (nodeItem.SelectNodes("feeds").Count != 0)
        {
          _Return.Add(new GUIListItem(nodeItem.Attributes["name"].InnerText));
        }
      }

      return _Return;
    }

    public List<GUIListItem> Feeds(XmlDocument xmlDoc, string _Site)
    {
      List<GUIListItem> _Return = new List<GUIListItem>();

      GUIListItem _Item;

      foreach (XmlNode nodeItem in xmlDoc.SelectNodes("sites/suggestion[@name='" + _Site + "']/feeds/feed"))
      {
        _Item = new GUIListItem(nodeItem.Attributes["name"].InnerText);
        _Item.Path = nodeItem.InnerText;

        _Return.Add(_Item);
      }

      return _Return;
    }

    public List<GUIListItem> List(string _Feed)
    {
      List<GUIListItem> _Return = new List<GUIListItem>();

      XmlDocument xmlDoc = new XmlDocument();

      xmlDoc.Load(new XmlTextReader(_Feed));

      if (xmlDoc.SelectSingleNode("rss[@version='2.0']") != null)
      {
        XmlNodeList xmlNodes = xmlDoc.SelectNodes("rss/channel/item");
        foreach (XmlNode xmlNode in xmlNodes)
        {
          _Return.Add(new GUIListItem(Regex.Replace(xmlNode["title"].InnerText, @"\d{3}-\s|\d{2}%\s", String.Empty)));
        }
      }
      else
      {
        GUIPropertyManager.SetProperty("#Status", "Error parsing XML");
      }

      return _Return;
    }

  }
}