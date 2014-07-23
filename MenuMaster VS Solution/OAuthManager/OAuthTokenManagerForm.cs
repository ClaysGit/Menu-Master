using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace OAuthManager
{
    public partial class OAuthTokenManagerForm : Form
    {
        public OAuthTokenManagerForm()
        {
            InitializeComponent();
        }

        private void RequestConsent_Click(object sender, EventArgs e)
        {
            WebBrowser.Url = OAuthManager.RequestConsentUri();
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            OAuthManager.SubmitTokenRequest(CodeTextbox.Text);
            token = OAuthManager.OAuthAccessToken;
            this.Close();
        }

        public string token { get; set; }
    }

}