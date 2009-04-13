using System;
using System.Windows.Forms;

using MediaPortal.Profile;

namespace mpNZB
{
  public partial class frmSetup : Form
  {
    public frmSetup()
    {
      InitializeComponent();
    }

    private void frmSetup_Load(object sender, EventArgs e)
    {
      Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");

      // Plugin Settings
      // ##################################################
      txtUpdateFreq.Text = mpSettings.GetValueAsInt("#Plugin", "UpdateFrequency", 1).ToString();
      txtDisplayName.Text = mpSettings.GetValue("#Plugin", "DisplayName");
      // ##################################################

      // Client Settings
      // ##################################################
      string strGrabber = mpSettings.GetValue("#Client", "Grabber");
      cmbGrabber.Text = ((strGrabber.Length > 0) ? strGrabber : "SABnzbd");

      string strHost = mpSettings.GetValue("#Client", "Host");
      string strPort = mpSettings.GetValue("#Client", "Port");
      txtHost.Text = ((strHost.Length > 0) ? strHost : "127.0.0.1");
      txtPort.Text = ((strPort.Length > 0) ? strPort : "8080");

      chkCatSelect.Enabled = (cmbGrabber.Text == "SABnzbd");
      chkCatSelect.Checked = mpSettings.GetValueAsBool("#Client", "CatSelect", false);

      chkAuth.Checked = mpSettings.GetValueAsBool("#Client", "Auth", false);
      txtUsername.Enabled = chkAuth.Checked;
      txtPassword.Enabled = chkAuth.Checked;
      txtUsername.Text = mpSettings.GetValue("#Client", "Username");
      txtPassword.Text = mpSettings.GetValue("#Client", "Password");
      // ##################################################

      // Site Settings
      // ##################################################
      txtMaxResults.Text = mpSettings.GetValueAsInt("#Sites", "MaxResults", 50).ToString();
      string strSortBy = mpSettings.GetValue("#Sites", "SortBy");
      cmbSort.Text = ((strSortBy.Length > 0) ? strSortBy : "Default");
      // ##################################################

      mpSettings.Dispose();
    }

    private void fncSaveConfig()
    {
      Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");

      // Plugin Settings
      // ##################################################
      int intUpdateFreq = 1;
      int.TryParse(txtUpdateFreq.Text, out intUpdateFreq);
      mpSettings.SetValue("#Plugin", "UpdateFrequency", intUpdateFreq);
      mpSettings.SetValue("#Plugin", "DisplayName", txtDisplayName.Text);
      // ##################################################

      // Client Settings
      // ##################################################
      mpSettings.SetValue("#Client", "Grabber", cmbGrabber.Text);

      mpSettings.SetValue("#Client", "Host", txtHost.Text);
      mpSettings.SetValue("#Client", "Port", txtPort.Text);

      mpSettings.SetValueAsBool("#Client", "CatSelect", chkCatSelect.Checked);

      mpSettings.SetValueAsBool("#Client", "Auth", chkAuth.Checked);
      mpSettings.SetValue("#Client", "Username", txtUsername.Text);
      mpSettings.SetValue("#Client", "Password", txtPassword.Text);
      // ##################################################

      // Site Settings
      // ##################################################
      int intMaxResults = 50;
      int.TryParse(txtMaxResults.Text, out intMaxResults);
      mpSettings.SetValue("#Sites", "MaxResults", intMaxResults);
      mpSettings.SetValue("#Sites", "SortBy", cmbSort.Text);
      // ##################################################

      mpSettings.Dispose();

    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      fncSaveConfig();
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void chkAuth_CheckedChanged(object sender, EventArgs e)
    {
      txtUsername.Enabled = chkAuth.Checked;
      txtPassword.Enabled = chkAuth.Checked;
    }

    private void btnTestConn_Click(object sender, EventArgs e)
    {
      // Setup Client
      // ##################################################
      Clients.iClient Client;
      switch (cmbGrabber.Text)
      {
        case "SABnzbd":
          Client = new Clients.SABnzbd(txtHost.Text, txtPort.Text, false, chkAuth.Checked, txtUsername.Text, txtPassword.Text, 1);
          if (Client.Version().Length != 0)
          {
            MessageBox.Show(null, "Connection: OK", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
          else
          {
            MessageBox.Show(null, "Connection: Failed", "Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          break;
      }
      // ##################################################
    }
  }
}