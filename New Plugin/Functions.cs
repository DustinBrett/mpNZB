using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;

using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Profile;
using MediaPortal.Util;

namespace mpNZB
{
  class Functions
  {

    #region Dialogs

    public void OK(string _Line, string _Heading)
    {
      // Init Dialog
      GUIDialogOK Dialog = (GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);
      Dialog.Reset();

      // Set Dialog Information
      Dialog.SetHeading(_Heading);
      Dialog.SetLine(1, _Line);

      // Display Dialog
      Dialog.DoModal(GUIWindowManager.ActiveWindow);
    }

    public void Text(string _Text, string _Heading)
    {
      // Init Dialog
      GUIDialogText Dialog = (GUIDialogText)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_TEXT);
      Dialog.Reset();

      // Set Dialog Information
      Dialog.SetHeading(_Heading);
      Dialog.SetText(_Text);

      // Display Dialog
      Dialog.DoModal(GUIWindowManager.ActiveWindow);
    }

    public bool YesNo(string _Text, string _Heading)
    {
      // Init Dialog
      GUIDialogYesNo Dialog = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);
      Dialog.Reset();

      // Set Dialog Information
      Dialog.SetHeading(_Heading);
      Dialog.SetLine(1, _Text);

      // Display Dialog
      Dialog.DoModal(GUIWindowManager.ActiveWindow);

      // Return Result
      return Dialog.IsConfirmed;
    }

    public GUIListItem Menu(List<GUIListItem> _Items, string _Heading)
    {
      // Init Dialog
      GUIDialogMenu Dialog = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
      Dialog.Reset();

      // Set Dialog Information
      Dialog.SetHeading(_Heading);
      _Items.ForEach(Dialog.Add);

      // Display Dialog
      Dialog.DoModal(GUIWindowManager.ActiveWindow);

      // Return Result
      if (Dialog.SelectedLabel >= 0)
      {
        _Items[Dialog.SelectedLabel].Label = Dialog.SelectedLabelText;
        return _Items[Dialog.SelectedLabel];
      }
      return null;
    }

    public string Keyboard()
    {
      // Init Dialog
      VirtualKeyboard Dialog = (VirtualKeyboard)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD);
      Dialog.Reset();

      // Display Dialog
      Dialog.DoModal(GUIWindowManager.ActiveWindow);

      // Return Result
      return ((Dialog.IsConfirmed) ? Dialog.Text : String.Empty);
    }

    #endregion

    #region Controls

    public void ListItem(GUIListControl _List, string _Label, string _Label2, string _Path, int _ItemId, string _DVDLabel, DateTime _CreationTime, long _Length)
    {
      // Create File Information
      FileInformation FileInfo = new FileInformation();
      FileInfo.CreationTime = _CreationTime;
      FileInfo.Length = _Length;

      // Create Item
      GUIListItem Item = new GUIListItem(_Label, _Label2, _Path, false, FileInfo);
      Item.ItemId = _ItemId;
      Item.DVDLabel = _DVDLabel;

      // Add Item to List
      _List.Add(Item);
    }

    #endregion

    #region Settings

    public string GetValueAsString(string _Section, string _Entry, string _Default)
    {
      string _Return = _Default;

      try
      {
        Settings mpSettings = new Settings(Config.GetFolder(Config.Dir.Config) + @"\mpNZB.xml");
        _Return = mpSettings.GetValueAsString(_Section, _Entry, _Default);
        mpSettings.Dispose();
      }
      catch (Exception ex) { Error(ex); }

      return _Return;
    }

    public int GetValueAsInt(string _Section, string _Entry, int _Default)
    {
      int _Return = _Default;

      try
      {
        Settings mpSettings = new Settings(Config.GetFolder(Config.Dir.Config) + @"\mpNZB.xml");
        _Return = mpSettings.GetValueAsInt(_Section, _Entry, _Default);
        mpSettings.Dispose();
      }
      catch (Exception ex) { Error(ex); }

      return _Return;
    }

    public bool GetValueAsBool(string _Section, string _Entry, bool _Default)
    {
      bool _Return = _Default;

      try
      {
        Settings mpSettings = new Settings(Config.GetFolder(Config.Dir.Config) + @"\mpNZB.xml");
        _Return = mpSettings.GetValueAsBool(_Section, _Entry, _Default);
        mpSettings.Dispose();
      }
      catch (Exception ex) { Error(ex); }

      return _Return;
    }

    #endregion

    #region WebRequest

    public string GetURI(string _URI)
    {
      string _Return = String.Empty;

      try
      {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_URI);
        request.Headers.Add(HttpRequestHeader.AcceptEncoding, "deflate, gzip");

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream responseStream = response.GetResponseStream();

        switch (response.ContentEncoding.ToLower())
        {
          case "deflate":
            responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
            break;
          case "gzip":
            responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            break;
        }

        StreamReader responseReader = new StreamReader(responseStream);
        _Return = responseReader.ReadToEnd();

        responseReader.Close();
        responseStream.Close();
        response.Close();
      }
      catch (Exception ex) { Error(ex); }

      return _Return;
    }

    #endregion

    #region Error

    public void Error(Exception ex)
    {
      // Log Error
      Log.Error("[mpNZB]" + " Message: " + ex.Message);
      Log.Error("[mpNZB]" + " Source: " + ex.Source);
      Log.Error("[mpNZB]" + " TargetSite: " + ex.TargetSite);

      // Update Status
      GUIPropertyManager.SetProperty("#Status", "Error.");
    }

    #endregion

  }
}