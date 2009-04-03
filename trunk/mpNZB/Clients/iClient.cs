using System.Timers;

using MediaPortal.GUI.Library;

namespace mpNZB.Clients
{
  interface iClient
  {
    void Status(statusTimer Status, GUIToggleButtonControl btnButton);
    void Queue(GUIListControl lstItemList, GUIWindow GUI);
    void Delete(GUIListControl lstItemList, GUIWindow GUI);
    void Download(GUIListControl lstItemList, string strSiteName, Clients.statusTimer Status);
    void Pause(bool bolPause, Clients.statusTimer Status);
    string Version();
  }

  struct statusTimer
  {
    public Timer tmrTimer;
    public bool KeepAlive;
  }
}