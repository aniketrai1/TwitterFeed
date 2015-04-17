using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

   
    public partial class _Default : System.Web.UI.Page
    {
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl myDiv;
        public string query = "Aniket";
        
        public string url = "https://api.twitter.com/1.1/statuses/user_timeline.json";
        protected void Page_Load(object sender, EventArgs e)
        {
            tweets(url, query);
        }
        public void tweets(string resource_url, string q)
        {
          
            var oauth_token = "2923011289-0Hy0fa2oVFfLIj8v1TeiFl16u3YA5FhWSZIdpc8";
            var oauth_token_secret = "D9d4oViHo7hBB68jpxmKi7ZbMd3NquM82JgTGEWBJh5Bf";
            var oauth_consumer_key = "y6CwAjNL8XVzX3EJ6Xjb30nUG";
            var oauth_consumer_secret = "fHctXz1LgVarBdOV0R6yTCLff5IbGoXLPtiRAP94d9EPSWqo6s";
           
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            var oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&q={6}";
            var baseString = string.Format(baseFormat,
                                        oauth_consumer_key,
                                        oauth_nonce,
                                        oauth_signature_method,
                                        oauth_timestamp,
                                        oauth_token,
                                        oauth_version,
                                        Uri.EscapeDataString(q)
                                        );
            baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));
            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));
            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                               "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                               "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                               "oauth_version=\"{6}\"";
            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_version)
                            );

           
            ServicePointManager.Expect100Continue = false;
           
            var postBody = "q=" + Uri.EscapeDataString(q);
            resource_url += "?" + postBody;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var objText = reader.ReadToEnd();
            myDiv.InnerHtml = objText;
            string html = "";
            
                JArray jsonDat = JArray.Parse(objText);
                for (int x = 0; x < jsonDat.Count(); x++)
                {
                   
                    html += jsonDat[x]["text"].ToString() + "<br/>";
                   
                 
                }
                myDiv.InnerHtml = html;
            
        }
   }
