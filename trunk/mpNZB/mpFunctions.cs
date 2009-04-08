using System;

using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

namespace mpNZB
{
  class mpFunctions
  {

    #region Windows

    public void OK(string strLine, string strHeading)
    {
      GUIDialogOK dlgOK = (GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);

      dlgOK.Reset();
      dlgOK.SetHeading(strHeading);
      dlgOK.SetLine(1, strLine);
      dlgOK.DoModal(GUIWindowManager.ActiveWindow);
    }

    public bool YesNo(string strLine, string strHeading)
    {
      GUIDialogYesNo dlgYesNo = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);

      dlgYesNo.Reset();
      dlgYesNo.SetHeading(strHeading);
      dlgYesNo.SetLine(1, strLine);
      dlgYesNo.DoModal(GUIWindowManager.ActiveWindow);

      return dlgYesNo.IsConfirmed;
    }

    public string Menu(string[] strLabels, string strHeading)
    {
      GUIDialogMenu dlgMenu = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);

      dlgMenu.Reset();
      dlgMenu.SetHeading(strHeading);
      foreach (string strLabel in strLabels)
      {
        dlgMenu.Add(strLabel);
      }
      dlgMenu.DoModal(GUIWindowManager.ActiveWindow);

      if (dlgMenu.SelectedLabel != -1)
      {
        return dlgMenu.SelectedLabelText;
      }
      return String.Empty;
    }

    public string Keyboard()
    {
      VirtualKeyboard dlgKeyboard = (VirtualKeyboard)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD);
      
      dlgKeyboard.Reset();
      dlgKeyboard.DoModal(GUIWindowManager.ActiveWindow);

      if (dlgKeyboard.IsConfirmed)
      {
        return dlgKeyboard.Text;
      }
      return String.Empty;
    }

    #endregion

    #region Controls

    public void AddItem(GUIListControl lstItemList, string strLabel, string strLabel2, string strPath, int intItemId)
    {
      GUIListItem Item = new GUIListItem();

      Item.Label = strLabel;
      Item.Label2 = strLabel2;
      Item.Path = strPath;
      Item.ItemId = intItemId;

      lstItemList.Add(Item);
    }

    #endregion

  }
}