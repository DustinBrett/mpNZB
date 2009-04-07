namespace mpNZB
{
  partial class frmSetup
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.tabSetup = new System.Windows.Forms.TabControl();
      this.tabClientSetup = new System.Windows.Forms.TabPage();
      this.grpSettings = new System.Windows.Forms.GroupBox();
      this.btnTestConn = new System.Windows.Forms.Button();
      this.chkAuth = new System.Windows.Forms.CheckBox();
      this.txtUsername = new System.Windows.Forms.TextBox();
      this.lblUsername = new System.Windows.Forms.Label();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.lblPort = new System.Windows.Forms.Label();
      this.lblHost = new System.Windows.Forms.Label();
      this.txtPort = new System.Windows.Forms.TextBox();
      this.lblPassword = new System.Windows.Forms.Label();
      this.txtHost = new System.Windows.Forms.TextBox();
      this.grpProgram = new System.Windows.Forms.GroupBox();
      this.chkCatSelect = new System.Windows.Forms.CheckBox();
      this.cmbGrabber = new System.Windows.Forms.ComboBox();
      this.lblGrabber = new System.Windows.Forms.Label();
      this.tabPluginSetup = new System.Windows.Forms.TabPage();
      this.grpVisual = new System.Windows.Forms.GroupBox();
      this.txtUpdateFreq = new System.Windows.Forms.TextBox();
      this.txtDisplayName = new System.Windows.Forms.TextBox();
      this.lblDisplayName = new System.Windows.Forms.Label();
      this.lblUpdateFrequency = new System.Windows.Forms.Label();
      this.tabSetupSetup = new System.Windows.Forms.TabPage();
      this.grpSites = new System.Windows.Forms.GroupBox();
      this.btnDelete = new System.Windows.Forms.Button();
      this.lvSites = new System.Windows.Forms.ListView();
      this.clmSite = new System.Windows.Forms.ColumnHeader();
      this.clmUsername = new System.Windows.Forms.ColumnHeader();
      this.clmPassword = new System.Windows.Forms.ColumnHeader();
      this.clmFeed = new System.Windows.Forms.ColumnHeader();
      this.clmSearch = new System.Windows.Forms.ColumnHeader();
      this.cmbSites = new System.Windows.Forms.ComboBox();
      this.lblSite = new System.Windows.Forms.Label();
      this.btnAdd = new System.Windows.Forms.Button();
      this.txtSitePassword = new System.Windows.Forms.TextBox();
      this.lblSitePassword = new System.Windows.Forms.Label();
      this.txtSiteUsername = new System.Windows.Forms.TextBox();
      this.lblSiteUsername = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.tabSetup.SuspendLayout();
      this.tabClientSetup.SuspendLayout();
      this.grpSettings.SuspendLayout();
      this.grpProgram.SuspendLayout();
      this.tabPluginSetup.SuspendLayout();
      this.grpVisual.SuspendLayout();
      this.tabSetupSetup.SuspendLayout();
      this.grpSites.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabSetup
      // 
      this.tabSetup.Controls.Add(this.tabClientSetup);
      this.tabSetup.Controls.Add(this.tabPluginSetup);
      this.tabSetup.Controls.Add(this.tabSetupSetup);
      this.tabSetup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tabSetup.Location = new System.Drawing.Point(12, 12);
      this.tabSetup.Name = "tabSetup";
      this.tabSetup.SelectedIndex = 0;
      this.tabSetup.Size = new System.Drawing.Size(320, 299);
      this.tabSetup.TabIndex = 1;
      // 
      // tabClientSetup
      // 
      this.tabClientSetup.Controls.Add(this.grpSettings);
      this.tabClientSetup.Controls.Add(this.grpProgram);
      this.tabClientSetup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tabClientSetup.Location = new System.Drawing.Point(4, 22);
      this.tabClientSetup.Name = "tabClientSetup";
      this.tabClientSetup.Padding = new System.Windows.Forms.Padding(3);
      this.tabClientSetup.Size = new System.Drawing.Size(312, 273);
      this.tabClientSetup.TabIndex = 0;
      this.tabClientSetup.Text = "Client Setup";
      this.tabClientSetup.UseVisualStyleBackColor = true;
      // 
      // grpSettings
      // 
      this.grpSettings.Controls.Add(this.btnTestConn);
      this.grpSettings.Controls.Add(this.chkAuth);
      this.grpSettings.Controls.Add(this.txtUsername);
      this.grpSettings.Controls.Add(this.lblUsername);
      this.grpSettings.Controls.Add(this.txtPassword);
      this.grpSettings.Controls.Add(this.lblPort);
      this.grpSettings.Controls.Add(this.lblHost);
      this.grpSettings.Controls.Add(this.txtPort);
      this.grpSettings.Controls.Add(this.lblPassword);
      this.grpSettings.Controls.Add(this.txtHost);
      this.grpSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.grpSettings.Location = new System.Drawing.Point(16, 95);
      this.grpSettings.Name = "grpSettings";
      this.grpSettings.Size = new System.Drawing.Size(280, 160);
      this.grpSettings.TabIndex = 10;
      this.grpSettings.TabStop = false;
      this.grpSettings.Text = "Settings:";
      // 
      // btnTestConn
      // 
      this.btnTestConn.Location = new System.Drawing.Point(160, 47);
      this.btnTestConn.Name = "btnTestConn";
      this.btnTestConn.Size = new System.Drawing.Size(104, 21);
      this.btnTestConn.TabIndex = 8;
      this.btnTestConn.Text = "Test Connection";
      this.btnTestConn.UseVisualStyleBackColor = true;
      this.btnTestConn.Click += new System.EventHandler(this.btnTestConn_Click);
      // 
      // chkAuth
      // 
      this.chkAuth.AutoSize = true;
      this.chkAuth.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkAuth.Location = new System.Drawing.Point(77, 74);
      this.chkAuth.Name = "chkAuth";
      this.chkAuth.Size = new System.Drawing.Size(141, 17);
      this.chkAuth.TabIndex = 5;
      this.chkAuth.Text = "Requires Authentication";
      this.chkAuth.UseVisualStyleBackColor = true;
      this.chkAuth.CheckedChanged += new System.EventHandler(this.chkAuth_CheckedChanged);
      // 
      // txtUsername
      // 
      this.txtUsername.Enabled = false;
      this.txtUsername.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtUsername.Location = new System.Drawing.Point(77, 97);
      this.txtUsername.Name = "txtUsername";
      this.txtUsername.Size = new System.Drawing.Size(187, 21);
      this.txtUsername.TabIndex = 6;
      // 
      // lblUsername
      // 
      this.lblUsername.AutoSize = true;
      this.lblUsername.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUsername.Location = new System.Drawing.Point(12, 100);
      this.lblUsername.Name = "lblUsername";
      this.lblUsername.Size = new System.Drawing.Size(59, 13);
      this.lblUsername.TabIndex = 4;
      this.lblUsername.Text = "Username:";
      // 
      // txtPassword
      // 
      this.txtPassword.Enabled = false;
      this.txtPassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtPassword.Location = new System.Drawing.Point(77, 124);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new System.Drawing.Size(187, 21);
      this.txtPassword.TabIndex = 7;
      this.txtPassword.UseSystemPasswordChar = true;
      // 
      // lblPort
      // 
      this.lblPort.AutoSize = true;
      this.lblPort.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPort.Location = new System.Drawing.Point(40, 50);
      this.lblPort.Name = "lblPort";
      this.lblPort.Size = new System.Drawing.Size(31, 13);
      this.lblPort.TabIndex = 2;
      this.lblPort.Text = "Port:";
      // 
      // lblHost
      // 
      this.lblHost.AutoSize = true;
      this.lblHost.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblHost.Location = new System.Drawing.Point(21, 23);
      this.lblHost.Name = "lblHost";
      this.lblHost.Size = new System.Drawing.Size(50, 13);
      this.lblHost.TabIndex = 1;
      this.lblHost.Text = "Address:";
      // 
      // txtPort
      // 
      this.txtPort.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtPort.Location = new System.Drawing.Point(77, 47);
      this.txtPort.Name = "txtPort";
      this.txtPort.Size = new System.Drawing.Size(77, 21);
      this.txtPort.TabIndex = 4;
      // 
      // lblPassword
      // 
      this.lblPassword.AutoSize = true;
      this.lblPassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPassword.Location = new System.Drawing.Point(14, 127);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new System.Drawing.Size(57, 13);
      this.lblPassword.TabIndex = 6;
      this.lblPassword.Text = "Password:";
      // 
      // txtHost
      // 
      this.txtHost.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtHost.Location = new System.Drawing.Point(77, 20);
      this.txtHost.Name = "txtHost";
      this.txtHost.Size = new System.Drawing.Size(187, 21);
      this.txtHost.TabIndex = 3;
      // 
      // grpProgram
      // 
      this.grpProgram.Controls.Add(this.chkCatSelect);
      this.grpProgram.Controls.Add(this.cmbGrabber);
      this.grpProgram.Controls.Add(this.lblGrabber);
      this.grpProgram.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.grpProgram.Location = new System.Drawing.Point(16, 14);
      this.grpProgram.Name = "grpProgram";
      this.grpProgram.Size = new System.Drawing.Size(280, 75);
      this.grpProgram.TabIndex = 9;
      this.grpProgram.TabStop = false;
      this.grpProgram.Text = "Program:";
      // 
      // chkCatSelect
      // 
      this.chkCatSelect.AutoSize = true;
      this.chkCatSelect.Enabled = false;
      this.chkCatSelect.Location = new System.Drawing.Point(68, 44);
      this.chkCatSelect.Name = "chkCatSelect";
      this.chkCatSelect.Size = new System.Drawing.Size(116, 17);
      this.chkCatSelect.TabIndex = 11;
      this.chkCatSelect.Text = "Save to categories";
      this.chkCatSelect.UseVisualStyleBackColor = true;
      // 
      // cmbGrabber
      // 
      this.cmbGrabber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbGrabber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cmbGrabber.FormattingEnabled = true;
      this.cmbGrabber.Items.AddRange(new object[] {
            "SABnzbd"});
      this.cmbGrabber.Location = new System.Drawing.Point(68, 17);
      this.cmbGrabber.Name = "cmbGrabber";
      this.cmbGrabber.Size = new System.Drawing.Size(196, 21);
      this.cmbGrabber.TabIndex = 1;
      // 
      // lblGrabber
      // 
      this.lblGrabber.AutoSize = true;
      this.lblGrabber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblGrabber.Location = new System.Drawing.Point(12, 21);
      this.lblGrabber.Name = "lblGrabber";
      this.lblGrabber.Size = new System.Drawing.Size(50, 13);
      this.lblGrabber.TabIndex = 2;
      this.lblGrabber.Text = "Grabber:";
      // 
      // tabPluginSetup
      // 
      this.tabPluginSetup.Controls.Add(this.grpVisual);
      this.tabPluginSetup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tabPluginSetup.Location = new System.Drawing.Point(4, 22);
      this.tabPluginSetup.Name = "tabPluginSetup";
      this.tabPluginSetup.Size = new System.Drawing.Size(312, 273);
      this.tabPluginSetup.TabIndex = 1;
      this.tabPluginSetup.Text = "Plugin Setup";
      this.tabPluginSetup.UseVisualStyleBackColor = true;
      // 
      // grpVisual
      // 
      this.grpVisual.Controls.Add(this.txtUpdateFreq);
      this.grpVisual.Controls.Add(this.txtDisplayName);
      this.grpVisual.Controls.Add(this.lblDisplayName);
      this.grpVisual.Controls.Add(this.lblUpdateFrequency);
      this.grpVisual.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.grpVisual.Location = new System.Drawing.Point(16, 14);
      this.grpVisual.Name = "grpVisual";
      this.grpVisual.Size = new System.Drawing.Size(280, 75);
      this.grpVisual.TabIndex = 17;
      this.grpVisual.TabStop = false;
      this.grpVisual.Text = "Visual:";
      // 
      // txtUpdateFreq
      // 
      this.txtUpdateFreq.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtUpdateFreq.Location = new System.Drawing.Point(145, 17);
      this.txtUpdateFreq.Name = "txtUpdateFreq";
      this.txtUpdateFreq.Size = new System.Drawing.Size(55, 21);
      this.txtUpdateFreq.TabIndex = 18;
      // 
      // txtDisplayName
      // 
      this.txtDisplayName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtDisplayName.Location = new System.Drawing.Point(93, 44);
      this.txtDisplayName.Name = "txtDisplayName";
      this.txtDisplayName.Size = new System.Drawing.Size(171, 21);
      this.txtDisplayName.TabIndex = 18;
      // 
      // lblDisplayName
      // 
      this.lblDisplayName.AutoSize = true;
      this.lblDisplayName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblDisplayName.Location = new System.Drawing.Point(12, 48);
      this.lblDisplayName.Name = "lblDisplayName";
      this.lblDisplayName.Size = new System.Drawing.Size(75, 13);
      this.lblDisplayName.TabIndex = 17;
      this.lblDisplayName.Text = "Display Name:";
      // 
      // lblUpdateFrequency
      // 
      this.lblUpdateFrequency.AutoSize = true;
      this.lblUpdateFrequency.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUpdateFrequency.Location = new System.Drawing.Point(12, 21);
      this.lblUpdateFrequency.Name = "lblUpdateFrequency";
      this.lblUpdateFrequency.Size = new System.Drawing.Size(127, 13);
      this.lblUpdateFrequency.TabIndex = 16;
      this.lblUpdateFrequency.Text = "Update Frequency (sec):";
      // 
      // tabSetupSetup
      // 
      this.tabSetupSetup.Controls.Add(this.grpSites);
      this.tabSetupSetup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tabSetupSetup.Location = new System.Drawing.Point(4, 22);
      this.tabSetupSetup.Name = "tabSetupSetup";
      this.tabSetupSetup.Size = new System.Drawing.Size(312, 273);
      this.tabSetupSetup.TabIndex = 2;
      this.tabSetupSetup.Text = "Site Setup";
      this.tabSetupSetup.UseVisualStyleBackColor = true;
      // 
      // grpSites
      // 
      this.grpSites.Controls.Add(this.btnDelete);
      this.grpSites.Controls.Add(this.lvSites);
      this.grpSites.Controls.Add(this.cmbSites);
      this.grpSites.Controls.Add(this.lblSite);
      this.grpSites.Controls.Add(this.btnAdd);
      this.grpSites.Controls.Add(this.txtSitePassword);
      this.grpSites.Controls.Add(this.lblSitePassword);
      this.grpSites.Controls.Add(this.txtSiteUsername);
      this.grpSites.Controls.Add(this.lblSiteUsername);
      this.grpSites.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.grpSites.Location = new System.Drawing.Point(16, 14);
      this.grpSites.Name = "grpSites";
      this.grpSites.Size = new System.Drawing.Size(280, 237);
      this.grpSites.TabIndex = 0;
      this.grpSites.TabStop = false;
      this.grpSites.Text = "Add Sites:";
      // 
      // btnDelete
      // 
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new System.Drawing.Point(209, 18);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(55, 21);
      this.btnDelete.TabIndex = 32;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // lvSites
      // 
      this.lvSites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmSite,
            this.clmUsername,
            this.clmPassword,
            this.clmFeed,
            this.clmSearch});
      this.lvSites.FullRowSelect = true;
      this.lvSites.GridLines = true;
      this.lvSites.Location = new System.Drawing.Point(15, 99);
      this.lvSites.MultiSelect = false;
      this.lvSites.Name = "lvSites";
      this.lvSites.Size = new System.Drawing.Size(250, 122);
      this.lvSites.TabIndex = 31;
      this.lvSites.UseCompatibleStateImageBehavior = false;
      this.lvSites.View = System.Windows.Forms.View.Details;
      this.lvSites.SelectedIndexChanged += new System.EventHandler(this.lvSites_SelectedIndexChanged);
      // 
      // clmSite
      // 
      this.clmSite.Text = "Site";
      this.clmSite.Width = 58;
      // 
      // clmUsername
      // 
      this.clmUsername.Text = "Username";
      this.clmUsername.Width = 68;
      // 
      // clmPassword
      // 
      this.clmPassword.Text = "Password";
      this.clmPassword.Width = 68;
      // 
      // clmFeed
      // 
      this.clmFeed.Text = "F";
      this.clmFeed.Width = 18;
      // 
      // clmSearch
      // 
      this.clmSearch.Text = "S";
      this.clmSearch.Width = 18;
      // 
      // cmbSites
      // 
      this.cmbSites.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbSites.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cmbSites.FormattingEnabled = true;
      this.cmbSites.Items.AddRange(new object[] {
            "Newzbin",
            "Newzleech",
            "NZBClub",
            "NZBIndex",
            "NZBMatrix",
            "NZBsRus",
            "TvNZB"});
      this.cmbSites.Location = new System.Drawing.Point(47, 18);
      this.cmbSites.Name = "cmbSites";
      this.cmbSites.Size = new System.Drawing.Size(95, 21);
      this.cmbSites.TabIndex = 29;
      this.cmbSites.SelectedIndexChanged += new System.EventHandler(this.cmbSites_SelectedIndexChanged);
      // 
      // lblSite
      // 
      this.lblSite.AutoSize = true;
      this.lblSite.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblSite.Location = new System.Drawing.Point(12, 21);
      this.lblSite.Name = "lblSite";
      this.lblSite.Size = new System.Drawing.Size(29, 13);
      this.lblSite.TabIndex = 30;
      this.lblSite.Text = "Site:";
      // 
      // btnAdd
      // 
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new System.Drawing.Point(148, 18);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(55, 21);
      this.btnAdd.TabIndex = 28;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // txtSitePassword
      // 
      this.txtSitePassword.Enabled = false;
      this.txtSitePassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtSitePassword.Location = new System.Drawing.Point(75, 72);
      this.txtSitePassword.Name = "txtSitePassword";
      this.txtSitePassword.Size = new System.Drawing.Size(189, 21);
      this.txtSitePassword.TabIndex = 26;
      // 
      // lblSitePassword
      // 
      this.lblSitePassword.AutoSize = true;
      this.lblSitePassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblSitePassword.Location = new System.Drawing.Point(14, 75);
      this.lblSitePassword.Name = "lblSitePassword";
      this.lblSitePassword.Size = new System.Drawing.Size(57, 13);
      this.lblSitePassword.TabIndex = 25;
      this.lblSitePassword.Text = "Password:";
      // 
      // txtSiteUsername
      // 
      this.txtSiteUsername.Enabled = false;
      this.txtSiteUsername.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtSiteUsername.Location = new System.Drawing.Point(75, 45);
      this.txtSiteUsername.Name = "txtSiteUsername";
      this.txtSiteUsername.Size = new System.Drawing.Size(189, 21);
      this.txtSiteUsername.TabIndex = 24;
      // 
      // lblSiteUsername
      // 
      this.lblSiteUsername.AutoSize = true;
      this.lblSiteUsername.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblSiteUsername.Location = new System.Drawing.Point(12, 49);
      this.lblSiteUsername.Name = "lblSiteUsername";
      this.lblSiteUsername.Size = new System.Drawing.Size(59, 13);
      this.lblSiteUsername.TabIndex = 23;
      this.lblSiteUsername.Text = "Username:";
      // 
      // btnCancel
      // 
      this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCancel.Location = new System.Drawing.Point(257, 321);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnOK
      // 
      this.btnOK.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnOK.Location = new System.Drawing.Point(176, 321);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // frmSetup
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(344, 354);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.tabSetup);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "frmSetup";
      this.Text = "mpNZB Setup";
      this.Load += new System.EventHandler(this.frmSetup_Load);
      this.tabSetup.ResumeLayout(false);
      this.tabClientSetup.ResumeLayout(false);
      this.grpSettings.ResumeLayout(false);
      this.grpSettings.PerformLayout();
      this.grpProgram.ResumeLayout(false);
      this.grpProgram.PerformLayout();
      this.tabPluginSetup.ResumeLayout(false);
      this.grpVisual.ResumeLayout(false);
      this.grpVisual.PerformLayout();
      this.tabSetupSetup.ResumeLayout(false);
      this.grpSites.ResumeLayout(false);
      this.grpSites.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabSetup;
    private System.Windows.Forms.TabPage tabClientSetup;
    private System.Windows.Forms.Label lblHost;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.TextBox txtHost;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.Label lblPort;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.TextBox txtPort;
    private System.Windows.Forms.Label lblUsername;
    private System.Windows.Forms.GroupBox grpProgram;
    private System.Windows.Forms.ComboBox cmbGrabber;
    private System.Windows.Forms.Label lblGrabber;
    private System.Windows.Forms.GroupBox grpSettings;
    private System.Windows.Forms.CheckBox chkAuth;
    private System.Windows.Forms.TabPage tabPluginSetup;
    private System.Windows.Forms.GroupBox grpVisual;
    private System.Windows.Forms.Label lblUpdateFrequency;
    private System.Windows.Forms.TextBox txtDisplayName;
    private System.Windows.Forms.Label lblDisplayName;
    private System.Windows.Forms.TabPage tabSetupSetup;
    private System.Windows.Forms.TextBox txtUpdateFreq;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.CheckBox chkCatSelect;
    private System.Windows.Forms.Button btnTestConn;
    private System.Windows.Forms.GroupBox grpSites;
    private System.Windows.Forms.ComboBox cmbSites;
    private System.Windows.Forms.Label lblSite;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.TextBox txtSitePassword;
    private System.Windows.Forms.Label lblSitePassword;
    private System.Windows.Forms.TextBox txtSiteUsername;
    private System.Windows.Forms.Label lblSiteUsername;
    private System.Windows.Forms.ListView lvSites;
    private System.Windows.Forms.ColumnHeader clmSite;
    private System.Windows.Forms.ColumnHeader clmUsername;
    private System.Windows.Forms.ColumnHeader clmPassword;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.ColumnHeader clmFeed;
    private System.Windows.Forms.ColumnHeader clmSearch;
  }
}