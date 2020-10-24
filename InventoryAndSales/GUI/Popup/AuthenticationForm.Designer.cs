namespace InventoryAndSales.GUI.Popup
{
  partial class AuthenticationForm
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
      this.buttonAuthenticate = new System.Windows.Forms.Button();
      this.buttonBack = new System.Windows.Forms.Button();
      this.labelPassword = new System.Windows.Forms.Label();
      this.labelUsername = new System.Windows.Forms.Label();
      this.textBoxPassword = new System.Windows.Forms.TextBox();
      this.textBoxUsername = new System.Windows.Forms.TextBox();
      this.labelInvalidAuthentication = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // buttonAuthenticate
      // 
      this.buttonAuthenticate.Location = new System.Drawing.Point(239, 135);
      this.buttonAuthenticate.Name = "buttonAuthenticate";
      this.buttonAuthenticate.Size = new System.Drawing.Size(75, 23);
      this.buttonAuthenticate.TabIndex = 0;
      this.buttonAuthenticate.Text = "Ijinkan";
      this.buttonAuthenticate.UseVisualStyleBackColor = true;
      this.buttonAuthenticate.Click += new System.EventHandler(this.buttonAuthenticate_Click);
      // 
      // buttonBack
      // 
      this.buttonBack.Location = new System.Drawing.Point(158, 135);
      this.buttonBack.Name = "buttonBack";
      this.buttonBack.Size = new System.Drawing.Size(75, 23);
      this.buttonBack.TabIndex = 1;
      this.buttonBack.Text = "Kembali";
      this.buttonBack.UseVisualStyleBackColor = true;
      this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
      // 
      // labelPassword
      // 
      this.labelPassword.AutoSize = true;
      this.labelPassword.Location = new System.Drawing.Point(18, 75);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(53, 13);
      this.labelPassword.TabIndex = 8;
      this.labelPassword.Text = "Password";
      // 
      // labelUsername
      // 
      this.labelUsername.AutoSize = true;
      this.labelUsername.Location = new System.Drawing.Point(18, 43);
      this.labelUsername.Name = "labelUsername";
      this.labelUsername.Size = new System.Drawing.Size(55, 13);
      this.labelUsername.TabIndex = 7;
      this.labelUsername.Text = "Username";
      // 
      // textBoxPassword
      // 
      this.textBoxPassword.Location = new System.Drawing.Point(91, 72);
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxPassword.PasswordChar = '*';
      this.textBoxPassword.Size = new System.Drawing.Size(223, 20);
      this.textBoxPassword.TabIndex = 6;
      // 
      // textBoxUsername
      // 
      this.textBoxUsername.Location = new System.Drawing.Point(91, 39);
      this.textBoxUsername.Name = "textBoxUsername";
      this.textBoxUsername.Size = new System.Drawing.Size(223, 20);
      this.textBoxUsername.TabIndex = 5;
      // 
      // labelInvalidAuthentication
      // 
      this.labelInvalidAuthentication.AutoSize = true;
      this.labelInvalidAuthentication.ForeColor = System.Drawing.Color.Red;
      this.labelInvalidAuthentication.Location = new System.Drawing.Point(91, 99);
      this.labelInvalidAuthentication.Name = "labelInvalidAuthentication";
      this.labelInvalidAuthentication.Size = new System.Drawing.Size(0, 13);
      this.labelInvalidAuthentication.TabIndex = 9;
      // 
      // AuthenticationForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(326, 171);
      this.ControlBox = false;
      this.Controls.Add(this.labelInvalidAuthentication);
      this.Controls.Add(this.labelPassword);
      this.Controls.Add(this.labelUsername);
      this.Controls.Add(this.textBoxPassword);
      this.Controls.Add(this.textBoxUsername);
      this.Controls.Add(this.buttonBack);
      this.Controls.Add(this.buttonAuthenticate);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "AuthenticationForm";
      this.Text = "Persetujuan Admin / Supervisor";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonAuthenticate;
    private System.Windows.Forms.Button buttonBack;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.Label labelUsername;
    private System.Windows.Forms.TextBox textBoxPassword;
    private System.Windows.Forms.TextBox textBoxUsername;
    private System.Windows.Forms.Label labelInvalidAuthentication;
  }
}