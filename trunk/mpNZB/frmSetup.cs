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
      // --------------------------------------------------
      txtUpdateFreq.Text = mpSettings.GetValueAsInt("#Plugin", "UpdateFrequency", 1).ToString();
      txtDisplayName.Text = mpSettings.GetValue("#Plugin", "DisplayName");
      // --------------------------------------------------

      // Client Settings
      // --------------------------------------------------
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
      // --------------------------------------------------

      // Site Settings
      // --------------------------------------------------
      chkNewzbin_auth.Checked = mpSettings.GetValueAsBool("#Sites", "Newzbin_auth", false);
      txtNewzbin_username.Enabled = chkNewzbin_auth.Checked;
      txtNewzbin_password.Enabled = chkNewzbin_auth.Checked;
      txtNewzbin_username.Text = mpSettings.GetValue("#Sites", "Newzbin_username");
      txtNewzbin_password.Text = mpSettings.GetValue("#Sites", "Newzbin_password");

      chkNZBMatrix_auth.Checked = mpSettings.GetValueAsBool("#Sites", "NZBMatrix_auth", false);
      txtNZBMatrix_username.Enabled = chkNZBMatrix_auth.Checked;
      txtNZBMatrix_password.Enabled = chkNZBMatrix_auth.Checked;
      txtNZBMatrix_username.Text = mpSettings.GetValue("#Sites", "NZBMatrix_username");
      txtNZBMatrix_password.Text = mpSettings.GetValue("#Sites", "NZBMatrix_password");

      chkNZBsRus_auth.Checked = mpSettings.GetValueAsBool("#Sites", "NZBsRus_auth", false);
      txtNZBsRus_username.Enabled = chkNZBsRus_auth.Checked;
      txtNZBsRus_password.Enabled = chkNZBsRus_auth.Checked;
      txtNZBsRus_username.Text = mpSettings.GetValue("#Sites", "NZBsRus_username");
      txtNZBsRus_password.Text = mpSettings.GetValue("#Sites", "NZBsRus_password");
      // --------------------------------------------------

      mpSettings.Dispose();
    }

    private void fncSaveConfig()
    {
      Settings mpSettings = new Settings(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\mpNZB.xml");

      // Plugin Settings
      // --------------------------------------------------
      int intUpdateFreq = 1;
      int.TryParse(txtUpdateFreq.Text, out intUpdateFreq);
      mpSettings.SetValue("#Plugin", "UpdateFrequency", intUpdateFreq);
      mpSettings.SetValue("#Plugin", "DisplayName", txtDisplayName.Text);
      // --------------------------------------------------

      // Client Settings
      // --------------------------------------------------
      mpSettings.SetValue("#Client", "Grabber", cmbGrabber.Text);

      mpSettings.SetValue("#Client", "Host", txtHost.Text);
      mpSettings.SetValue("#Client", "Port", txtPort.Text);

      mpSettings.SetValueAsBool("#Client", "CatSelect", chkCatSelect.Checked);

      mpSettings.SetValueAsBool("#Client", "Auth", chkAuth.Checked);
      mpSettings.SetValue("#Client", "Username", txtUsername.Text);
      mpSettings.SetValue("#Client", "Password", txtPassword.Text);
      // --------------------------------------------------

      // Site Settings
      // --------------------------------------------------
      mpSettings.SetValueAsBool("#Sites", "Newzbin_auth", chkNewzbin_auth.Checked);
      mpSettings.SetValue("#Sites", "Newzbin_username", txtNewzbin_username.Text);
      mpSettings.SetValue("#Sites", "Newzbin_password", txtNewzbin_password.Text);

      mpSettings.SetValueAsBool("#Sites", "NZBMatrix_auth", chkNZBMatrix_auth.Checked);
      mpSettings.SetValue("#Sites", "NZBMatrix_username", txtNZBMatrix_username.Text);
      mpSettings.SetValue("#Sites", "NZBMatrix_password", txtNZBMatrix_password.Text);

      mpSettings.SetValueAsBool("#Sites", "NZBsRus_auth", chkNZBsRus_auth.Checked);
      mpSettings.SetValue("#Sites", "NZBsRus_username", txtNZBsRus_username.Text);
      mpSettings.SetValue("#Sites", "NZBsRus_password", txtNZBsRus_password.Text);
      // --------------------------------------------------

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

    private void chkNewzbin_CheckedChanged(object sender, EventArgs e)
    {
      txtNewzbin_username.Enabled = chkNewzbin_auth.Checked;
      txtNewzbin_password.Enabled = chkNewzbin_auth.Checked;
    }

    private void chkNZBMatrix_auth_CheckedChanged(object sender, EventArgs e)
    {
      txtNZBMatrix_username.Enabled = chkNZBMatrix_auth.Checked;
      txtNZBMatrix_password.Enabled = chkNZBMatrix_auth.Checked;
    }

    private void chkNZBsRus_auth_CheckedChanged(object sender, EventArgs e)
    {
      txtNZBsRus_username.Enabled = chkNZBsRus_auth.Checked;
      txtNZBsRus_password.Enabled = chkNZBsRus_auth.Checked;
    }

    private void btnTestConn_Click(object sender, EventArgs e)
    {     
      // Setup Client
      // --------------------------------------------------
      Clients.iClient Client;
      switch (cmbGrabber.Text)
      {
        case "SABnzbd":
          Client = new Clients.SABnzbd(txtHost.Text, txtPort.Text, chkCatSelect.Checked, chkAuth.Checked, txtUsername.Text, txtPassword.Text);
          string strVersion = Client.Version();
          if (strVersion.Length != 0)
          {
            MessageBox.Show(null, "Connection: OK" + Environment.NewLine + "".PadLeft(30) + Environment.NewLine + "Version: " + strVersion, "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
          else
          {
            MessageBox.Show(null, "Connection: Failed" + Environment.NewLine + "".PadLeft(35) + Environment.NewLine + "Version: Unknown", "Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          break;
      }
      // --------------------------------------------------
    }
  }
}