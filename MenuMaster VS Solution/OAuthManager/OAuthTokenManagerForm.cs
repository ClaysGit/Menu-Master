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
            OAuthToken = "";
        }

        // First leg of authentication. Construct a request, send to google, get a URI back, and point the user at it.
        // This will be the consent screen, informing the user of exactly what we just asked permission for. If they
        // accept, they get sent to a screen with an access code, which they must manually copy-paste into a box for
        // the next leg.
        private void RequestConsent_Click(object sender, EventArgs e)
        {
            var uriBuilder = new StringBuilder();
            uriBuilder.Append(OAuthRequestEndpoint);
            uriBuilder.AppendFormat("?client_id={0}", MenuMasterClientID);
            uriBuilder.AppendFormat("&scope={0}", EditScope);
            uriBuilder.AppendFormat("&redirect_uri={0}", ManualRedirectUri);
            uriBuilder.AppendFormat("&response_type={0}", ResponseType);
            uriBuilder.AppendFormat("&access_type={0}", AccessType);
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
            ResponseDict = GoogleCodeToToken(CodeTextbox.Text);
            this.Close();
        }

        private Dictionary<string, string> GoogleCodeToToken(string code)
        {
            using (var wb = new WebClient())
            {
                // Build an object containing our request
                var data = new NameValueCollection();
                data["code"] = code;
                data["client_id"] = MenuMasterClientID;
                data["client_secret"] = MenuMasterClientSecret;
                data["redirect_uri"] = ManualRedirectUri;
                data["grant_type"] = GrantType;

                // POST the request to google, getting a response, and convert that response to a convenient
                // dictionary format.
                var responseBytes = wb.UploadValues(CodeExchangeEndpoint, "POST", data);
                var responseJson = Encoding.ASCII.GetString(responseBytes);

                var responseKeyValueSets = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseJson);

                return responseKeyValueSets;
            }
        }

        public string OAuthToken { get; set; }
        public Dictionary<string, string> ResponseDict { get; set; }

        //Authentication Uris
        private const string OAuthRequestEndpoint = "https://accounts.google.com/o/oauth2/auth";
        private const string CodeExchangeEndpoint = "https://accounts.google.com/o/oauth2/token";

        //Application specific values
        private const string MenuMasterClientID = "278517180641-6q3eavp22pdcmr1pggddf7d258igu65b.apps.googleusercontent.com";
        private const string MenuMasterClientSecret = "3QQMshiPgpeH-iR9jUUXOUBs";

        //Application specific values: Redirect Uris
        private const string HttpRedirectUri = "http://localhost";
        private const string ManualRedirectUri = "urn:ietf:wg:oauth:2.0:oob";

        //Calendar scopes
        private const string EditScope = "https://www.googleapis.com/auth/calendar";
        private const string ViewScope = "https://www.googleapis.com/auth/calendar.readonly";

        //Hard coded values
        private const string ResponseType = "code";
        private const string GrantType = "authorization_code";
        private const string AccessType = "offline";
    }
}
