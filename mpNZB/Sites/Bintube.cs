﻿using System;
using System.Globalization;
using System.Xml;

using MediaPortal.GUI.Library;

namespace mpNZB.Sites
{
  class Bintube:iSite
  {
    
    #region Definitions

    public string SiteName
    {
      get { return "Bintube"; }
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
        FeedURL = "http://www.bintube.com/rss.aspx?q=" + FeedName.Replace(" ", "+") + "&r=250";
      }
    }

    public void AddItem(XmlNode Node, GUIListControl lstList)
    {
      string strTemp = Node["description"].InnerText.Replace(" ", String.Empty);
      string strSizeText = "<TR><TD>Size:</TD><TD>".ToLower();
      int intSizePOS = strTemp.ToLower().IndexOf(strSizeText.ToLower()) + strSizeText.Length;

      Dialogs.AddItem(lstList, Node["title"].InnerText, strTemp.Substring(intSizePOS, strTemp.IndexOf("</TD></TR>", intSizePOS) - intSizePOS).Replace("GB", " GB").Replace("MB", " MB").Replace("KB", " KB").Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0].ToString()).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0].ToString()), Node["link"].InnerText.Replace("/nzb/", "/download/"), 1);
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