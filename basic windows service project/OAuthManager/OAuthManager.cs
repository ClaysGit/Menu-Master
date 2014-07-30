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
            // Try to load a refresh token from an earlier instance. If that fails, run user authentication and save
            // the info for next time.
            if (!Load())
            {
                GetUserAuthentication();
                Save();
            }
            else // If we successfully loaded a refresh token, use it to get an auth token
                GoogleCodeToToken(null);
        }

        // Load up our webform, which will guide the user through the authentication process with Google. Hopefully
        // get an Auth token and a Refresh token.
        private void GetUserAuthentication()
        {
            using (var tokenForm = new OAuthTokenManagerForm())
            {
                Application.EnableVisualStyles();
                Application.Run(tokenForm);
                GoogleCodeToToken(tokenForm.GoogleCode);
            }

            Debug.WriteLine("Access Token: " + OAuthAccessToken);
            Debug.WriteLine("Refresh Token: " + OAuthRefreshToken);

            Save();
        }

        // Placeholder save and load functions for our refresh token.
        public void Save()
        {
            string[] text = {OAuthRefreshToken};
            File.WriteAllLines("appdata.txt", text);
        }

        // See if the file exists, then pull the first line, which should be our OAuth Refresh Token. If it isn't
        // for whatever reason, return false (failed to load), otherwise return true.
        public bool Load()
        {
            if (File.Exists("appdata.txt"))
            {
                string[] text = File.ReadAllLines("appdata.txt");

                if (text[0] == null)
                {
                    Debug.WriteLine("Failed to load appdata.txt");
                    return false;
                }
                else
                {
                    Debug.WriteLine("Loaded successfully");
                    OAuthRefreshToken = text[0];
                    return true;
                }
            }
            else
                return false;
        }

        // Load up our webform, which will guide the user through authenticating with Google. Generates a code if 
        // successful, use that code to exchange for our auth tokens.
        private void GetUserAuthorization()
        {
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
                data["client_id"] = Globals.MenuMasterClientID;
                data["client_secret"] = Globals.MenuMasterClientSecret;
                
                // If no authorization code is provided, use the refresh token "mode" instead
                if (code == null)
                {
                    data["grant_type"] = Globals.RefreshGrantType;
                    data["refresh_token"] = OAuthRefreshToken;
                }

                else
                {
                    data["grant_type"] = Globals.AuthGrantType;
                    data["code"] = code;
                    data["redirect_uri"] = Globals.ManualRedirectUri;
                }

                // POST the request to google, getting a response, and convert that response to a convenient
                // dictionary format.
                var responseBytes = wb.UploadValues(Globals.CodeExchangeEndpoint, "POST", data);
                var responseJson = Encoding.ASCII.GetString(responseBytes);

                responseKeyValueSets = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseJson);
                
                // Obviously, we don't get a refresh token if we were using one to GET this info!
                if (code != null)
                    OAuthRefreshToken = responseKeyValueSets["refresh_token"];
                OAuthAccessToken = responseKeyValueSets["access_token"];
            }
        }

        public string OAuthAccessToken { get; set; }
        private string OAuthRefreshToken;

        private Dictionary<string, string> responseKeyValueSets;

    }
}
