using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Databox.Libs.Common
{
    public partial class CaptchaForm : Form
    {

        public string ImageLocation
        {
            set
            {
                pictureBox1.ImageLocation = value;
            }
			get
			{
				return pictureBox1.ImageLocation;
			}
        }

        public string Result
        {
            get
            {
                return textBox1.Text;
            }
        }

        public CaptchaForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }


		public string DBCLogin { get; set; }
		public string DBCPass { get; set; }
		protected void SolveCaptcha()
		{
			try
			{
				var client = (DeathByCaptcha.Client)new DeathByCaptcha.SocketClient(DBCLogin, DBCPass);
				var captcha = client.Upload(ImageLocation);
				if (null != captcha)
				{
					while (captcha.Uploaded && !captcha.Solved)
					{
						System.Threading.Thread.Sleep(2 * 1000);
						captcha = client.GetCaptcha(captcha.Id);
					}

					if (captcha.Solved)
					{
						this.Invoke(new Action(() =>
						{
							textBox1.Text = captcha.Text;
							button1.PerformClick();
						}));
					}
				}

			}
			catch (Exception err)
			{
			}
		}

		private void CaptchaForm_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(DBCLogin) && !string.IsNullOrEmpty(DBCPass))
			{
				var a = new Action(SolveCaptcha);
				a.BeginInvoke(null, null);
				return;
			}
			progressBar1.Enabled = false;
			progressBar1.Visible = false;
			label1.Text = "Please set DBC login and pass";
		}
    }
}