namespace InventoryAndSales.GUI.Page
{
  partial class LoginPage
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.groupBoxLogin = new System.Windows.Forms.GroupBox();
      this.labelCannotLogin = new System.Windows.Forms.Label();
      this.labelPassword = new System.Windows.Forms.Label();
      this.labelUsername = new System.Windows.Forms.Label();
      this.buttonLogin = new System.Windows.Forms.Button();
      this.textBoxPassword = new System.Windows.Forms.TextBox();
      this.textBoxUsername = new System.Windows.Forms.TextBox();
      this.groupBoxLogin.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxLogin
      // 
      this.groupBoxLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxLogin.Controls.Add(this.labelCannotLogin);
      this.groupBoxLogin.Controls.Add(this.labelPassword);
      this.groupBoxLogin.Controls.Add(this.labelUsername);
      this.groupBoxLogin.Controls.Add(this.buttonLogin);
      this.groupBoxLogin.Controls.Add(this.textBoxPassword);
      this.groupBoxLogin.Controls.Add(this.textBoxUsername);
      this.groupBoxLogin.Location = new System.Drawing.Point(249, 176);
      this.groupBoxLogin.Name = "groupBoxLogin";
      this.groupBoxLogin.Size = new System.Drawing.Size(356, 187);
      this.groupBoxLogin.TabIndex = 1;
      this.groupBoxLogin.TabStop = false;
      this.groupBoxLogin.Text = "Please Login";
      // 
      // labelCannotLogin
      // 
      this.labelCannotLogin.AutoSize = true;
      this.labelCannotLogin.ForeColor = System.Drawing.Color.Red;
      this.labelCannotLogin.Location = new System.Drawing.Point(93, 114);
      this.labelCannotLogin.Name = "labelCannotLogin";
      this.labelCannotLogin.Size = new System.Drawing.Size(0, 13);
      this.labelCannotLogin.TabIndex = 5;
      // 
      // labelPassword
      // 
      this.labelPassword.AutoSize = true;
      this.labelPassword.Location = new System.Drawing.Point(23, 82);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(53, 13);
      this.labelPassword.TabIndex = 4;
      this.labelPassword.Text = "Password";
      // 
      // labelUsername
      // 
      this.labelUsername.AutoSize = true;
      this.labelUsername.Location = new System.Drawing.Point(23, 50);
      this.labelUsername.Name = "labelUsername";
      this.labelUsername.Size = new System.Drawing.Size(55, 13);
      this.labelUsername.TabIndex = 3;
      this.labelUsername.Text = "Username";
      // 
      // buttonLogin
      // 
      this.buttonLogin.Location = new System.Drawing.Point(207, 139);
      this.buttonLogin.Name = "buttonLogin";
      this.buttonLogin.Size = new System.Drawing.Size(112, 28);
      this.buttonLogin.TabIndex = 2;
      this.buttonLogin.Text = "Login";
      this.buttonLogin.UseVisualStyleBackColor = true;
      this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
      // 
      // textBoxPassword
      // 
      this.textBoxPassword.Location = new System.Drawing.Point(96, 79);
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxPassword.PasswordChar = '*';
      this.textBoxPassword.Size = new System.Drawing.Size(223, 20);
      this.textBoxPassword.TabIndex = 1;
      this.textBoxPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPassword_KeyPress);
      this.textBoxPassword.Enter += new System.EventHandler(this.textBoxPassword_Enter);
      // 
      // textBoxUsername
      // 
      this.textBoxUsername.Location = new System.Drawing.Point(96, 43);
      this.textBoxUsername.Name = "textBoxUsername";
      this.textBoxUsername.Size = new System.Drawing.Size(223, 20);
      this.textBoxUsername.TabIndex = 0;
      this.textBoxUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxUsername_KeyPress);
      this.textBoxUsername.Enter += new System.EventHandler(this.textBoxUsername_Enter);
      // 
      // LoginPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBoxLogin);
      this.Name = "LoginPage";
      this.Size = new System.Drawing.Size(885, 563);
      this.groupBoxLogin.ResumeLayout(false);
      this.groupBoxLogin.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxLogin;
    private System.Windows.Forms.Label labelCannotLogin;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.Label labelUsername;
    private System.Windows.Forms.Button buttonLogin;
    private System.Windows.Forms.TextBox textBoxPassword;
    private System.Windows.Forms.TextBox textBoxUsername;
  }
}
