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

        // First leg of authentication. Construct a request, send to google, get a URI back, and point the user at it.
        // This will be the consent screen, informing the user of exactly what we just asked permission for. If they
        // accept, they get sent to a screen with an access code, which they must manually copy-paste into a box for
        // the next leg.
        private void RequestConsent_Click(object sender, EventArgs e)
        {
            var uriBuilder = new StringBuilder();
            uriBuilder.Append(Globals.OAuthRequestEndpoint);
            uriBuilder.AppendFormat("?client_id={0}", Globals.MenuMasterClientID);
            uriBuilder.AppendFormat("&scope={0}", Globals.EditScope);
            uriBuilder.AppendFormat("&redirect_uri={0}", Globals.ManualRedirectUri);
            uriBuilder.AppendFormat("&response_type={0}", Globals.ResponseType);
            uriBuilder.AppendFormat("&access_type={0}", Globals.AccessType);
            //uriBuilder.AppendFormat("&state={0}", EditScope);
            //uriBuilder.AppendFormat("&login_hint={0}", EditScope);
            //uriBuilder.AppendFormat("&include_granted_scopes={0}", EditScope);
            var uri = uriBuilder.ToString();

            WebBrowser.Url = new Uri(uri);
        }

        // The second leg. The user inputs the code, given to them by Google after they give consent to our
        // application. The code is then exchanged for our Authentication Token, which we use in all activity requests
        // made to Google.
        private void Submit_Click(object sender, EventArgs e)
        {
            GoogleCode = CodeTextbox.Text;
            this.Close();
        }

        public string GoogleCode;
    }

    static class Globals
    {
        //Authentication Uris
        public const string OAuthRequestEndpoint = "https://accounts.google.com/o/oauth2/auth";
        public const string CodeExchangeEndpoint = "https://accounts.google.com/o/oauth2/token";

        //Application specific values
        public const string MenuMasterClientID = "278517180641-6q3eavp22pdcmr1pggddf7d258igu65b.apps.googleusercontent.com";
        public const string MenuMasterClientSecret = "3QQMshiPgpeH-iR9jUUXOUBs";

        //Application specific values: Redirect Uris
        public const string HttpRedirectUri = "http://localhost";
        public const string ManualRedirectUri = "urn:ietf:wg:oauth:2.0:oob";

        //Calendar scopes
        public const string EditScope = "https://www.googleapis.com/auth/calendar";
        public const string ViewScope = "https://www.googleapis.com/auth/calendar.readonly";

        //Hard coded values
        public const string ResponseType = "code";
        public const string GrantType = "authorization_code";
        public const string AccessType = "offline";
    }
}
