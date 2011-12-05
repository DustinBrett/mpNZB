using System;
using System.Collections.Generic;

using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

namespace mpNZB
{
  class mpFunctions
  {

    #region Dialog

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

    public void Notify(string _Line, string _Heading, int hideAfterSeconds)
    {
        // Init Dialog
        GUIDialogNotify Dialog = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
        Dialog.Reset();

        // Set Dialog Information
        Dialog.SetHeading(_Heading);
        Dialog.SetText(_Line);
        Dialog.TimeOut = hideAfterSeconds;

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

    public bool YesNo(string _Text, string _Heading, bool _Default)
    {
      // Init Dialog
      GUIDialogYesNo Dialog = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);
      Dialog.Reset();

      // Set Dialog Information
      Dialog.SetHeading(_Heading);
      Dialog.SetLine(1, _Text);
      Dialog.SetDefaultToYes(_Default);

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
      if (Dialog.SelectedLabel != -1)
      {
        _Items[Dialog.SelectedLabel].Label = Dialog.SelectedLabelText;
        return _Items[Dialog.SelectedLabel];
      }
      return null;
    }

    public string Keyboard()
    {
        return Keyboard(null);
    }

    public string Keyboard(string parameter)
    {
      // Init Dialog
      VirtualKeyboard Dialog = (VirtualKeyboard)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD);
      Dialog.Reset();

      if (parameter != null)
      {
          Dialog.Text = parameter;
      }

      // Display Dialog
      Dialog.DoModal(GUIWindowManager.ActiveWindow);

      // Return Result
      return ((Dialog.IsConfirmed) ? Dialog.Text : String.Empty);
    }

    #endregion

    #region Control

    public void ListItem(GUIListControl _List, string _Label, string _Label2, string _DVDLabel, DateTime _CreationTime, long _Length, string _Name, string _Path, int _ItemId)
    {
      // Init Item
      GUIListItem Item = new GUIListItem();

      // Set Item Information
      Item.Label = _Label;
      Item.Label2 = _Label2;
      Item.DVDLabel = _DVDLabel;
      Item.Path = _Path;
      Item.ItemId = _ItemId;

      // Set File Information
      Item.FileInfo = new MediaPortal.Util.FileInformation();
      Item.FileInfo.CreationTime = _CreationTime;
      Item.FileInfo.Length = _Length;
      Item.FileInfo.Name = _Name;

      // Add Item to List
      _List.Add(Item);
    }

    public void UpdateListItem(int index, GUIListControl _List, string _Label, string _Label2, string _DVDLabel, DateTime _CreationTime, long _Length, string _Name, string _Path, int _ItemId)
    {
        if (_List.Count <= index)
        {
            return;
        }

        // Set Item Information
        _List.ListItems[index].Label = _Label;
        _List.ListItems[index].Label2 = _Label2;
        _List.ListItems[index].DVDLabel = _DVDLabel;
        _List.ListItems[index].Path = _Path;
        _List.ListItems[index].ItemId = _ItemId;

        // Set File Information
        _List.ListItems[index].FileInfo = new MediaPortal.Util.FileInformation();
        _List.ListItems[index].FileInfo.CreationTime = _CreationTime;
        _List.ListItems[index].FileInfo.Length = _Length;
        _List.ListItems[index].FileInfo.Name = _Name;        

    }

    #endregion

    #region Error

    public void Error(Exception e)
    {
      // Log Error
      Log.Error("[mpNZB]" + " Message: " + e.Message);
      Log.Error("[mpNZB]" + " Source: " + e.Source);
      Log.Error("[mpNZB]" + " TargetSite: " + e.TargetSite);

      // Update Status
      GUIPropertyManager.SetProperty("#Status", "Error occured");
    }

    #endregion

  }
}