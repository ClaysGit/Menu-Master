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
    // OAuthTokenManager, our class to handle all authentication-related business for our program. When initialized,
    // our TokenManagerForm will be loaded up, the user will run through some hoops, and we'll have an authentication
    // token (hopefully). This version will also pull out a refresh token, though we don't do anything with it yet.
    public class OAuthTokenManager
    {
        public OAuthTokenManager()
        {
            // instantiate our form, run it, and then wait for it to finish. Generates a dictionary with full
            // response data from Google, most importantly including the Authentication Token and Refresh Token
            using (var tokenForm = new OAuthTokenManagerForm())
            {
                Application.EnableVisualStyles();
                Application.Run(tokenForm);
                responseDict = tokenForm.ResponseDict;
            }

            OAuthAccessToken = responseDict["access_token"];
            OAuthRefreshToken = responseDict["refresh_token"];
        }

        public string OAuthAccessToken { get; set; }
        private string OAuthRefreshToken;

        private Dictionary<string, string> responseDict;

    }
}
