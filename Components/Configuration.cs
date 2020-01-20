using System.Collections.Generic;
using PayPal.Api;

//using DotNetNuke.Entities.Modules;
//using DotNetNuke.Services.Localization;
//using DotNetNuke.Common;


namespace GIBS.Modules.GiftCertificate.Components
{
    public class Configuration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        private bool _sandboxMode;
        public bool SandboxMode
        {
            get { return _sandboxMode; }
            set { _sandboxMode = value; }
        }





        // Static constructor for setting the readonly static members.
        static Configuration()
        {



            //testseller@gibs.com

            // GiftCert2 Sandbox
            // testseller@gibs.com

            //           ClientId = "[YOUR ClientId]";
            //           ClientSecret = "[ClientSecret ]";


            // TEST BUYER
            // buyer@gibs.com
            // Bouyea9213

            //// GiftCert2 Live LIVE LIVE SETTINGS LIVE
            //// paypal@gibs.com
            //           ClientId = "[YOUR ClientId]";
            //           ClientSecret = "[ClientSecret ]";
            var config = GetConfig();
        }
        // Create the configuration map that contains mode and other optional configuration details.


        public static Dictionary<string, string> GetConfig()
        {
            // SET MODE
            // sandbox
            // live
            
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("mode", "live");
        //    dictionary.Add("mode", "sandbox");
            dictionary.Add("connectionTimeout", "360000");
            dictionary.Add("requestRetries", "1");
            dictionary.Add("ClientId", ClientId);
            dictionary.Add("ClientSecret", ClientSecret);
            return dictionary;//  ConfigManager.Instance.GetProperties();
            

        }

        // Create accessToken
        private static string GetAccessToken()
        {
            
            // ###AccessToken
            // Retrieve the access token from
            // OAuthTokenCredential by passing in
            // ClientID and ClientSecret
            // It is not mandatory to generate Access Token on a per call basis.
            // Typically the access token can be generated once and
            // reused within the expiry window                
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        // Returns APIContext object
        public static APIContext GetAPIContext(string accessToken = "")
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            var apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ? GetAccessToken() : accessToken);
            apiContext.Config = GetConfig();

            // Use this variant if you want to pass in a request id  
            // that is meaningful in your application, ideally 
            // a order id.
            // String requestId = Long.toString(System.nanoTime();
            // APIContext apiContext = new APIContext(GetAccessToken(), requestId ));

            return apiContext;
        }
    }
}