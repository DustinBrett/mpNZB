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
      this.tabPages = new System.Windows.Forms.TabControl();
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.txtMaxResults = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.grpVisual = new System.Windows.Forms.GroupBox();
      this.txtUpdateFreq = new System.Windows.Forms.TextBox();
      this.txtDisplayName = new System.Windows.Forms.TextBox();
      this.lblDisplayName = new System.Windows.Forms.Label();
      this.lblUpdateFrequency = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.tabSearch = new System.Windows.Forms.TabPage();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.txtSearchName = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtSearchString = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.lvSearches = new System.Windows.Forms.ListView();
      this.clmName = new System.Windows.Forms.ColumnHeader();
      this.clmString = new System.Windows.Forms.ColumnHeader();
      this.btnAdd = new System.Windows.Forms.Button();
      this.btnDelete = new System.Windows.Forms.Button();
      this.tabPages.SuspendLayout();
      this.tabClientSetup.SuspendLayout();
      this.grpSettings.SuspendLayout();
      this.grpProgram.SuspendLayout();
      this.tabPluginSetup.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.grpVisual.SuspendLayout();
      this.tabSearch.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabPages
      // 
      this.tabPages.Controls.Add(this.tabClientSetup);
      this.tabPages.Controls.Add(this.tabPluginSetup);
      this.tabPages.Controls.Add(this.tabSearch);
      this.tabPages.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tabPages.Location = new System.Drawing.Point(12, 12);
      this.tabPages.Name = "tabPages";
      this.tabPages.SelectedIndex = 0;
      this.tabPages.Size = new System.Drawing.Size(320, 299);
      this.tabPages.TabIndex = 1;
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
      this.tabPluginSetup.Controls.Add(this.groupBox1);
      this.tabPluginSetup.Controls.Add(this.grpVisual);
      this.tabPluginSetup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tabPluginSetup.Location = new System.Drawing.Point(4, 22);
      this.tabPluginSetup.Name = "tabPluginSetup";
      this.tabPluginSetup.Size = new System.Drawing.Size(312, 273);
      this.tabPluginSetup.TabIndex = 1;
      this.tabPluginSetup.Text = "Plugin Setup";
      this.tabPluginSetup.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.txtMaxResults);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox1.Location = new System.Drawing.Point(16, 95);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(280, 51);
      this.groupBox1.TabIndex = 19;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Feeds:";
      // 
      // txtMaxResults
      // 
      this.txtMaxResults.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtMaxResults.Location = new System.Drawing.Point(139, 18);
      this.txtMaxResults.Name = "txtMaxResults";
      this.txtMaxResults.Size = new System.Drawing.Size(55, 21);
      this.txtMaxResults.TabIndex = 18;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(12, 21);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(121, 13);
      this.label2.TabIndex = 16;
      this.label2.Text = "Max Results (per feed):";
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
      // tabSearch
      // 
      this.tabSearch.Controls.Add(this.lvSearches);
      this.tabSearch.Controls.Add(this.groupBox2);
      this.tabSearch.Location = new System.Drawing.Point(4, 22);
      this.tabSearch.Name = "tabSearch";
      this.tabSearch.Padding = new System.Windows.Forms.Padding(3);
      this.tabSearch.Size = new System.Drawing.Size(312, 273);
      this.tabSearch.TabIndex = 2;
      this.tabSearch.Text = "Custom Searches";
      this.tabSearch.UseVisualStyleBackColor = true;
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Enabled = false;
      this.checkBox1.Location = new System.Drawing.Point(68, 44);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(116, 17);
      this.checkBox1.TabIndex = 11;
      this.checkBox1.Text = "Save to categories";
      this.checkBox1.UseVisualStyleBackColor = true;
      // 
      // comboBox1
      // 
      this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Items.AddRange(new object[] {
            "SABnzbd"});
      this.comboBox1.Location = new System.Drawing.Point(68, 17);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(196, 21);
      this.comboBox1.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(12, 21);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(50, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Grabber:";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.btnDelete);
      this.groupBox2.Controls.Add(this.btnAdd);
      this.groupBox2.Controls.Add(this.label5);
      this.groupBox2.Controls.Add(this.txtSearchString);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.txtSearchName);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox2.Location = new System.Drawing.Point(16, 14);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(280, 96);
      this.groupBox2.TabIndex = 20;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Fields:";
      // 
      // txtSearchName
      // 
      this.txtSearchName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtSearchName.Location = new System.Drawing.Point(56, 18);
      this.txtSearchName.Name = "txtSearchName";
      this.txtSearchName.Size = new System.Drawing.Size(150, 21);
      this.txtSearchName.TabIndex = 18;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(12, 21);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(38, 13);
      this.label3.TabIndex = 16;
      this.label3.Text = "Name:";
      // 
      // txtSearchString
      // 
      this.txtSearchString.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtSearchString.Location = new System.Drawing.Point(56, 45);
      this.txtSearchString.Name = "txtSearchString";
      this.txtSearchString.Size = new System.Drawing.Size(150, 21);
      this.txtSearchString.TabIndex = 20;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(12, 48);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(39, 13);
      this.label4.TabIndex = 19;
      this.label4.Text = "String:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(45, 71);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(225, 13);
      this.label5.TabIndex = 21;
      this.label5.Text = "Seperate search terms by using the | symbol.";
      // 
      // lvSearches
      // 
      this.lvSearches.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmString});
      this.lvSearches.FullRowSelect = true;
      this.lvSearches.GridLines = true;
      this.lvSearches.Location = new System.Drawing.Point(16, 116);
      this.lvSearches.MultiSelect = false;
      this.lvSearches.Name = "lvSearches";
      this.lvSearches.Size = new System.Drawing.Size(280, 142);
      this.lvSearches.TabIndex = 21;
      this.lvSearches.UseCompatibleStateImageBehavior = false;
      this.lvSearches.View = System.Windows.Forms.View.Details;
      // 
      // clmName
      // 
      this.clmName.Text = "Name";
      this.clmName.Width = 90;
      // 
      // clmString
      // 
      this.clmString.Text = "String";
      this.clmString.Width = 170;
      // 
      // btnAdd
      // 
      this.btnAdd.Location = new System.Drawing.Point(212, 18);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(55, 21);
      this.btnAdd.TabIndex = 22;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // btnDelete
      // 
      this.btnDelete.Location = new System.Drawing.Point(212, 45);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(55, 21);
      this.btnDelete.TabIndex = 23;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // frmSetup
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(344, 354);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.tabPages);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "frmSetup";
      this.Text = "mpNZB Setup";
      this.Load += new System.EventHandler(this.frmSetup_Load);
      this.tabPages.ResumeLayout(false);
      this.tabClientSetup.ResumeLayout(false);
      this.grpSettings.ResumeLayout(false);
      this.grpSettings.PerformLayout();
      this.grpProgram.ResumeLayout(false);
      this.grpProgram.PerformLayout();
      this.tabPluginSetup.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.grpVisual.ResumeLayout(false);
      this.grpVisual.PerformLayout();
      this.tabSearch.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabPages;
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
    private System.Windows.Forms.TextBox txtUpdateFreq;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.CheckBox chkCatSelect;
    private System.Windows.Forms.Button btnTestConn;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox txtMaxResults;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TabPage tabSearch;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txtSearchString;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox txtSearchName;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListView lvSearches;
    private System.Windows.Forms.ColumnHeader clmName;
    private System.Windows.Forms.ColumnHeader clmString;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnAdd;
  }
}