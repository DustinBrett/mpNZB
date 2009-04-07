﻿using System;
using System.Globalization;
using System.Xml;

using MediaPortal.GUI.Library;

namespace mpNZB.Sites
{
  class Newzleech: iSite
  {
    
    #region Definitions

    public string SiteName
    {
      get { return "Newzleech"; }
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
        FeedURL = "http://www.newzleech.com/rss.php?n=250&s=" + FeedName.Replace(" ", "+");
      }
    }

    public void AddItem(XmlNode Node, GUIListControl lstList)
    {
      string strTemp = Node["description"].InnerText.Replace(" ", String.Empty);
      string strSizeText = ">Size:".ToLower();
      int intSizePOS = strTemp.ToLower().IndexOf(strSizeText.ToLower()) + strSizeText.Length;

      Dialogs.AddItem(lstList, Node["title"].InnerText, strTemp.Substring(intSizePOS, strTemp.IndexOf("<", intSizePOS) - intSizePOS).Replace("GB", " GB").Replace("MB", " MB").Replace("KB", " KB").Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0].ToString()).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0].ToString()), Node["link"].InnerText.Replace("?p=", "?m=gen&dl=1&post=").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\""), 1);
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