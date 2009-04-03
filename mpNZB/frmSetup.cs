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
      string strTempUsername;
      string strTempPassword;

      for (int i = 0; i < (cmbSites.Items.Count - 1); i++)
      {        
        strTempUsername = mpSettings.GetValue("#Sites", cmbSites.Items[i].ToString() + "_username");
        strTempPassword = mpSettings.GetValue("#Sites", cmbSites.Items[i].ToString() + "_password");
        if ((strTempUsername.Length > 0) && (strTempPassword.Length > 0))
        {
          lvSites.Items.Add(cmbSites.Items[i].ToString());
          lvSites.Items[lvSites.Items.Count - 1].SubItems.Add(strTempUsername);
          lvSites.Items[lvSites.Items.Count - 1].SubItems.Add(strTempPassword);
        }
      }
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
      string strFeedList = String.Empty;
      string strSearchList = String.Empty;
      for (int i = 0; i < (lvSites.Items.Count - 1); i++)
      {
        mpSettings.SetValue("#Sites", lvSites.Items[i].Text + "_username", lvSites.Items[i].SubItems[1].Text);
        mpSettings.SetValue("#Sites", lvSites.Items[i].Text + "_password", lvSites.Items[i].SubItems[2].Text);

        // Add as Feed
        if (lvSites.Items[i].SubItems[3].Text == "*")
        {
          strFeedList += lvSites.Items[i].Text + (char)0;
        }

        // Add as Search
        if (lvSites.Items[i].SubItems[4].Text == "*")
        {
          strFeedList += lvSites.Items[i].Text + (char)0;
        }
      }
      mpSettings.SetValue("#Lists", "FeedList", strFeedList.Substring(0, (strFeedList.Length - 1)));
      mpSettings.SetValue("#Lists", "SearchList", strSearchList.Substring(0, (strSearchList.Length - 1)));
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

    private void btnAdd_Click(object sender, EventArgs e)
    {
      // Set Username/Password
      switch (cmbSites.SelectedText)
      {
        case "Newzbin":
        case "NZBMatrix":
        case "NZBsRus":
          if ((txtSiteUsername.Text.Length > 0) && (txtSitePassword.Text.Length > 0))
          {
            lvSites.Items.Add(cmbSites.Text);
            lvSites.Items[lvSites.Items.Count - 1].SubItems.Add(txtSiteUsername.Text);
            lvSites.Items[lvSites.Items.Count - 1].SubItems.Add(txtSitePassword.Text);
          }
          else
          {
            MessageBox.Show(null, "Username/Password Invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
          }
          break;
        case "TvNZB":
        case "NZBIndex":
          lvSites.Items.Add(cmbSites.Text);
          lvSites.Items[lvSites.Items.Count - 1].SubItems.Add(String.Empty);
          lvSites.Items[lvSites.Items.Count - 1].SubItems.Add(String.Empty);
          break;
        default:
          return;
      }

      // Set Username/Password
      switch (cmbSites.SelectedText)
      {
        case "Newzbin":
          lvSites.Items[lvSites.Items.Count - 1].SubItems.Add("*");
          lvSites.Items[lvSites.Items.Count - 1].SubItems.Add("*");
          break;
        case "NZBIndex":
          lvSites.Items[lvSites.Items.Count - 1].SubItems.Add(String.Empty);
          lvSites.Items[lvSites.Items.Count - 1].SubItems.Add("*");
          break;
        case "NZBMatrix":
        case "NZBsRus":
        case "TvNZB":
          lvSites.Items[lvSites.Items.Count - 1].SubItems.Add("*");
          lvSites.Items[lvSites.Items.Count - 1].SubItems.Add(String.Empty);
          break;
      }

      cmbSites.SelectedIndex = -1;
      btnAdd.Enabled = false;
      txtSiteUsername.Text = "";
      txtSitePassword.Text = "";
    }

    private void cmbSites_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cmbSites.SelectedIndex != -1)
      {
        btnAdd.Enabled = true;
      }
    }

    private void lvSites_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (lvSites.SelectedItems.Count > 0)
      {
        btnDelete.Enabled = true;
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      lvSites.SelectedItems[0].Remove();
    }
  }
}