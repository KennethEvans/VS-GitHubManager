namespace GitHubManager
{
	partial class LoginForm
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
            this.authLabel = new System.Windows.Forms.Label();
            this.authComboBox = new System.Windows.Forms.ComboBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.flowLayoutPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.okButton = new System.Windows.Forms.Button();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // authLabel
            // 
            this.authLabel.AutoSize = true;
            this.authLabel.Location = new System.Drawing.Point(34, 32);
            this.authLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.authLabel.Name = "authLabel";
            this.authLabel.Size = new System.Drawing.Size(159, 41);
            this.authLabel.TabIndex = 0;
            this.authLabel.Text = "Auth Type:";
            // 
            // authComboBox
            // 
            this.authComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.authComboBox.FormattingEnabled = true;
            this.authComboBox.Items.AddRange(new object[] {
            "Unauthenticated Access",
            "Basic Authentication",
            "OAuth Token Authentication"});
            this.authComboBox.Location = new System.Drawing.Point(215, 23);
            this.authComboBox.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.authComboBox.Name = "authComboBox";
            this.authComboBox.Size = new System.Drawing.Size(672, 49);
            this.authComboBox.TabIndex = 1;
            this.authComboBox.SelectedIndexChanged += new System.EventHandler(this.authComboBox_SelectedIndexChanged);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(34, 93);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(150, 41);
            this.passwordLabel.TabIndex = 4;
            this.passwordLabel.Text = "Password:";
            this.passwordLabel.Visible = false;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(215, 93);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(672, 47);
            this.passwordTextBox.TabIndex = 5;
            this.passwordTextBox.UseSystemPasswordChar = true;
            this.passwordTextBox.Visible = false;
            this.passwordTextBox.TextChanged += new System.EventHandler(this.passwordTextBox_TextChanged);
            // 
            // flowLayoutPanelButtons
            // 
            this.flowLayoutPanelButtons.AutoSize = true;
            this.flowLayoutPanelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelButtons.Controls.Add(this.okButton);
            this.flowLayoutPanelButtons.Location = new System.Drawing.Point(435, 215);
            this.flowLayoutPanelButtons.Name = "flowLayoutPanelButtons";
            this.flowLayoutPanelButtons.Size = new System.Drawing.Size(74, 57);
            this.flowLayoutPanelButtons.TabIndex = 7;
            // 
            // okButton
            // 
            this.okButton.AutoSize = true;
            this.okButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.okButton.Location = new System.Drawing.Point(3, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(68, 51);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(215, 149);
            this.userTextBox.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(672, 47);
            this.userTextBox.TabIndex = 3;
            this.userTextBox.Visible = false;
            this.userTextBox.TextChanged += new System.EventHandler(this.userTextBox_TextChanged);
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(34, 149);
            this.userLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(159, 41);
            this.userLabel.TabIndex = 2;
            this.userLabel.Text = "Username:";
            this.userLabel.Visible = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 286);
            this.Controls.Add(this.flowLayoutPanelButtons);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.userTextBox);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.authComboBox);
            this.Controls.Add(this.authLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.flowLayoutPanelButtons.ResumeLayout(false);
            this.flowLayoutPanelButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label authLabel;
		private System.Windows.Forms.ComboBox authComboBox;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButtons;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.Label userLabel;
    }
}