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

    public bool YesNo(List<string> _Lines, string _Heading)
    {
      // Init Dialog
      GUIDialogYesNo Dialog = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);
      Dialog.Reset();

      // Set Dialog Information
      Dialog.SetHeading(_Heading);
      for (int i = 0; i < _Lines.Count; i++)
      {
        Dialog.SetLine(i + 1, _Lines[i]);
      }

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

      // Check if Item Selected
      if (Dialog.SelectedLabel != -1)
      {
        // Remove Item #
        _Items[Dialog.SelectedLabel].Label = _Items[Dialog.SelectedLabel].Label.Substring(_Items[Dialog.SelectedLabel].Label.IndexOf(" ") + 1, _Items[Dialog.SelectedLabel].Label.Length - (_Items[Dialog.SelectedLabel].Label.IndexOf(" ") + 1));

        // Return Result
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

    #region Control

    public void ListItem(GUIListControl _List, string _Label, string _Label2, DateTime _CreationTime, long _Length, string _Path, string _DVDLabel, int _ItemId)
    {
      // Init Item
      GUIListItem Item = new GUIListItem();

      // Set Item Information
      Item.Label = _Label;
      Item.Label2 = _Label2;
      Item.FileInfo = new MediaPortal.Util.FileInformation();
      Item.FileInfo.CreationTime = _CreationTime;
      Item.FileInfo.Length = _Length;
      Item.Path = _Path;
      Item.DVDLabel = _DVDLabel;
      Item.ItemId = _ItemId;

      // Add Item to List
      _List.Add(Item);
    }

    #endregion

    #region Error

    private string strAppName = "mpNZB";

    public void Error(Exception e)
    {
      // Log Error
      Log.Error("[" + strAppName + "] " + "Message: " + e.Message);      
      Log.Error("[" + strAppName + "] " + "TargetSite: " + e.TargetSite);

      // Update Status
      GUIPropertyManager.SetProperty("#Status", "Error occured");
    }

    #endregion

  }
}