using System.Timers;

using MediaPortal.GUI.Library;

namespace mpNZB.Clients
{
  interface iClient
  {
    void Status(Timer Status, GUIToggleButtonControl btnButton);
    void Queue(GUIListControl lstItemList, GUIWindow GUI);
    void Delete(GUIListItem lstItem, GUIListControl lstItemList, GUIWindow GUI);
    void Download(GUIListItem lstItem, Sites.iSite Site, Timer Status);
    void Pause(bool bolPause, Timer Status);
    string Version();
  }
}