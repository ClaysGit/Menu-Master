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

using System.Diagnostics;
using System.IO;

namespace OAuthManager
{
    // OAuthTokenManager, our class to handle all authentication-related business for our program. When initialized,
    // our TokenManagerForm will be loaded up, the user will run through some hoops, and we'll have an authentication
    // token (hopefully). This version will also pull out a refresh token, though we don't do anything with it yet.
    public class OAuthTokenManager
    {
        public OAuthTokenManager()
        {
            // Check if we already have a refresh token, and then check if it works.


            // instantiate our form, run it, and then wait for it to finish. Generates a dictionary with full
            // response data from Google, most importantly including the Authentication Token and Refresh Token
            using (var tokenForm = new OAuthTokenManagerForm())
            {
                Application.EnableVisualStyles();
                Application.Run(tokenForm);
                GoogleCodeToToken(tokenForm.GoogleCode);
            }

            Debug.WriteLine("Access Token: " + OAuthAccessToken);
            Debug.WriteLine("Refresh Token: " + OAuthRefreshToken);
        }

        // Placeholder save and load functions for our refresh token.
        public void Save()
        {
            string[] text = {OAuthRefreshToken};
            File.WriteAllLines("appdata.txt", text);
        }

        public void Load()
        {
            string[] text = File.ReadAllLines("appdata.txt");
            OAuthRefreshToken = text[0];
        }

        private void GetUserAuthorization()
        {
            // instantiate our form, run it, and then wait for it to finish. Generates a code which we can exchange
            // for the requested access rights, in the form of an Access and a Refresh token.
            using (var tokenForm = new OAuthTokenManagerForm())
            {
                Application.EnableVisualStyles();
                Application.Run(tokenForm);
                GoogleCodeToToken(tokenForm.GoogleCode);
            }
        }

        // Exchange a code, representing client consent for our program, for an authorization token which will be sent
        // along with all requests we make to Google.
        private void GoogleCodeToToken(string code)
        {
            using (var wb = new WebClient())
            {
                // Build an object containing our request
                var data = new NameValueCollection();
                data["code"] = code;
                data["client_id"] = Globals.MenuMasterClientID;
                data["client_secret"] = Globals.MenuMasterClientSecret;
                data["redirect_uri"] = Globals.ManualRedirectUri;
                data["grant_type"] = Globals.GrantType;

                // POST the request to google, getting a response, and convert that response to a convenient
                // dictionary format.
                var responseBytes = wb.UploadValues(Globals.CodeExchangeEndpoint, "POST", data);
                var responseJson = Encoding.ASCII.GetString(responseBytes);

                responseKeyValueSets = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseJson);

                OAuthAccessToken = responseKeyValueSets["access_token"];
                OAuthRefreshToken = responseKeyValueSets["refresh_token"];
            }
        }

        public string OAuthAccessToken { get; set; }
        private string OAuthRefreshToken;

        private Dictionary<string, string> responseKeyValueSets;

    }
}
