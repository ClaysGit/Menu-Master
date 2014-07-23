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
    public class OAuthManager
    {
        public OAuthManager()
        {
            OAuthAccessToken = "";
            OAuthRefreshToken = "";

            ManagerWebForm = new OAuthTokenManagerForm;
        }

        public Uri RequestConsentUri()
        {
            var uriBuilder = new StringBuilder();
            uriBuilder.Append(OAuthRequestEndpoint);
            uriBuilder.AppendFormat("?client_id={0}", MenuMasterClientID);
            uriBuilder.AppendFormat("&scope={0}", EditScope);
            uriBuilder.AppendFormat("&redirect_uri={0}", ManualRedirectUri);
            uriBuilder.AppendFormat("&response_type={0}", ResponseType);
            //uriBuilder.AppendFormat("&state={0}", EditScope);
            //uriBuilder.AppendFormat("&login_hint={0}", EditScope);
            //uriBuilder.AppendFormat("&include_granted_scopes={0}", EditScope);
            var uri = new Uri(uriBuilder.ToString());

            return uri;
        }

        public void SubmitTokenRequest(string code)
        {
            OAuthAccessToken = GoogleCodeToToken(code);
        }

        public string GoogleCodeToToken(string code)
        {
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["code"] = code;
                data["client_id"] = MenuMasterClientID;
                data["client_secret"] = MenuMasterClientSecret;
                data["redirect_uri"] = ManualRedirectUri;
                data["grant_type"] = GrantType;

                var responseBytes = wb.UploadValues(CodeExchangeEndpoint, "POST", data);
                var responseJson = Encoding.ASCII.GetString(responseBytes);

                var responseKeyValueSets = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseJson);

                return responseKeyValueSets["access_token"];
            }
        }

        private OAuthTokenManagerForm ManagerWebForm;

        public string OAuthAccessToken { get; set; }
        private string OAuthRefreshToken;

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
