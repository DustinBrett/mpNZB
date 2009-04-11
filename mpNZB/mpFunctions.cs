using System;
using System.Collections.Generic;

using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

namespace mpNZB
{
  class mpFunctions
  {

    #region Definition

    public class MenuItem
    {
      public string Label;
      public string Path;

      public MenuItem(string _Label, string _Path)
      {
        this.Label = _Label;
        this.Path = _Path;
      }
    }

    #endregion

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

    public bool YesNo(string _Line, string _Heading)
    {
      // Init Dialog
      GUIDialogYesNo Dialog = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);
      Dialog.Reset();

      // Set Dialog Information
      Dialog.SetHeading(_Heading);
      Dialog.SetLine(1, _Line);

      // Display Dialog
      Dialog.DoModal(GUIWindowManager.ActiveWindow);

      // Return Result
      return Dialog.IsConfirmed;
    }

    public MenuItem Menu(List<MenuItem> _Items, string _Heading)
    {
      // Init Dialog
      GUIDialogMenu Dialog = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
      Dialog.Reset();

      // Set Dialog Information
      Dialog.SetHeading(_Heading);
      GUIListItem ListItem;
      foreach (MenuItem Item in _Items)
      {
        ListItem = new GUIListItem(Item.Label);
        ListItem.Path = Item.Path;
        Dialog.Add(ListItem);
      }

      // Display Dialog
      Dialog.DoModal(GUIWindowManager.ActiveWindow);

      // Return Result
      return ((Dialog.SelectedLabel != -1) ? _Items[Dialog.SelectedLabel] : null);
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

    public void ListItem(GUIListControl _List, string _Label, string _Label2, string _Path, int _ItemId)
    {
      // Init Item
      GUIListItem Item = new GUIListItem();

      // Set Item Information
      Item.Label = _Label;
      Item.Label2 = _Label2;
      Item.Path = _Path;
      Item.ItemId = _ItemId;

      // Add Item to List
      _List.Add(Item);
    }

    #endregion

    #region Error

    public void Error(Exception e)
    {
      // Log Error
      Log.Info("Data: " + e.Data);
      Log.Info("HelpLink: " + e.HelpLink);
      Log.Info("InnerException: " + e.InnerException);
      Log.Info("Message: " + e.Message);
      Log.Info("Source: " + e.Source);
      Log.Info("StackTrace: " + e.StackTrace);
      Log.Info("TargetSite: " + e.TargetSite);

      // Update Status
      GUIPropertyManager.SetProperty("#Status", "Error occured.");
    }

    #endregion

  }
}