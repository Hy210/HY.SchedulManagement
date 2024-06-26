﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CalendarAPI
{
    public class GoogleOAuthHelper
    {
        public const string TOKEN_REQUEST_URL = "https://accounts.google.com/o/oauth2/token";
        public const string SCOPE = "https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcalendar" + "+" + "https%3A%2F%2Fwww.google.com%2Fm8%2Ffeeds%2F" + "+" + "https%3A%2F%2Fmail.google.com%2F"; // IMAP & SMTP
        public const string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";
        public const string REDIRECT_TYPE = "code";

        public static string GetAccessToken(User user)
        {
            return GetAccessToken(user);
        }

        public static string GetAccessToken(GoogleUser user)
        {
            string access_token;
            string token_type;
            int expires_in;
            GetAccessToken(user, out access_token, out token_type, out expires_in);
            return access_token;
        }
        public static void GetAccessToken(GoogleUser user, out string access_token, out string token_type, out int expires_in)
        {
            access_token = null;
            token_type = null;
            expires_in = 0;
            var request = (HttpWebRequest)WebRequest.Create(TOKEN_REQUEST_URL);
            request.CookieContainer = new CookieContainer();
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var encodedParameters = string.Format("client_id={0}&client_secret={1}&refresh_token={2}&grant_type={3}",
            System.Web.HttpUtility.UrlEncode(user.ClientId), System.Web.HttpUtility.UrlEncode(user.ClientSecret), System.Web.HttpUtility.UrlEncode(user.RefreshToken), System.Web.HttpUtility.UrlEncode(GrantTypes.refresh_token.ToString()));
            byte[] requestData = Encoding.UTF8.GetBytes(encodedParameters);
            request.ContentLength = requestData.Length;
            if (requestData.Length > 0)
            {
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(requestData, 0, requestData.Length);
                }
            }

            var response = (HttpWebResponse)request.GetResponse();
            string responseText = null;
            using (TextReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
            {
                responseText = reader.ReadToEnd();
            }

            foreach (string sPair in responseText.Replace("{", "").Replace("}", "").Replace("\"", "").Split(new string[] { ",\n" }, StringSplitOptions.None))
            {
                string[] pair = sPair.Split(':');
                string name = pair[0].Trim().ToLower();
                string value = System.Web.HttpUtility.UrlDecode(pair[1].Trim());
                switch (name)
                {
                    case "access_token":
                        access_token = value;
                        break;
                    case "token_type":
                        token_type = value;
                        break;

                    case "expires_in":
                        expires_in = Convert.ToInt32(value);
                        break;
                }
            }
            Debug.WriteLine("");
            Debug.WriteLine("---------------------------------------------------------");
            Debug.WriteLine("-----------OAuth 2.0 authorization information-----------");
            Debug.WriteLine("---------------------------------------------------------");
            Debug.WriteLine(string.Format("Login: '{0}'", user.EMail));
            Debug.WriteLine(string.Format("Access token: '{0}'", access_token));
            Debug.WriteLine(string.Format("Token type: '{0}'", token_type));
            Debug.WriteLine(string.Format("Expires in: '{0}'", expires_in));
            Debug.WriteLine("---------------------------------------------------------");
            Debug.WriteLine("");
        }

        public static void GetAccessToken(User user, out string access_token, out string refresh_token)
        {
            GetAccessToken((GoogleUser)user, out access_token, out refresh_token);
        }

        public static void GetAccessToken(GoogleUser user, out string access_token, out string refresh_token)
        {
            string token_type;
            int expires_in;
            GoogleOAuthHelper.GetAccessToken(user, out access_token, out refresh_token, out token_type, out expires_in);
        }

        public static void GetAccessToken(User user, out string access_token, out string refresh_token, out string token_type, out int expires_in)
        {
            GetAccessToken((GoogleUser)user, out access_token, out refresh_token, out token_type, out expires_in);
        }

        public static void GetAccessToken(GoogleUser user, out string access_token, out string refresh_token, out string token_type, out int expires_in)
        {
            string authorizationCode = GoogleOAuthHelper.GetAuthorizationCode(user, GoogleOAuthHelper.SCOPE, GoogleOAuthHelper.REDIRECT_URI, GoogleOAuthHelper.REDIRECT_TYPE);
            GoogleOAuthHelper.GetAccessToken(authorizationCode, user, out access_token, out token_type, out expires_in, out refresh_token);
        }

        public static void GetAccessToken(string authorizationCode, User user, out string access_token, out string token_type, out int expires_in, out string refresh_token)
        {
            GetAccessToken(authorizationCode, (GoogleUser)user, out access_token, out token_type, out expires_in, out refresh_token);
        }

        public static void GetAccessToken(string authorizationCode, GoogleUser user, out string access_token, out string token_type, out int expires_in, out string refresh_token)
        {
            access_token = null;
            token_type = null;
            expires_in = 0;
            refresh_token = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TOKEN_REQUEST_URL);
            request.CookieContainer = new CookieContainer();
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string encodedParameters = string.Format("client_id={0}&code={1}&client_secret={2}&redirect_uri={3}&grant_type={4}", System.Web.HttpUtility.UrlEncode(user.ClientId), System.Web.HttpUtility.UrlEncode(authorizationCode), System.Web.HttpUtility.UrlEncode(user.ClientSecret), System.Web.HttpUtility.UrlEncode(REDIRECT_URI), System.Web.HttpUtility.UrlEncode(GrantTypes.authorization_code.ToString()));
            byte[] requestData = Encoding.UTF8.GetBytes(encodedParameters);
            request.ContentLength = requestData.Length;
            if (requestData.Length > 0)
            {
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(requestData, 0, requestData.Length);
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = null;
            using (TextReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
            {
                responseText = reader.ReadToEnd();
            }

            foreach (string sPair in responseText.Replace("{", "").Replace("}", "").Replace("\"", "").Split(new string[] { ",\n" }, StringSplitOptions.None))
            {
                string[] pair = sPair.Split(':');
                string name = pair[0].Trim().ToLower();
                string value = System.Web.HttpUtility.UrlDecode(pair[1].Trim());
                switch (name)
                {
                    case "access_token":
                        access_token = value;
                        break;
                    case "token_type":
                        token_type = value;
                        break;

                    case "expires_in":
                        expires_in = Convert.ToInt32(value);
                        break;

                    case "refresh_token":
                        refresh_token = value;
                        break;
                }
            }

            Debug.WriteLine(string.Format("Authorization code: '{0}'", authorizationCode));
            Debug.WriteLine(string.Format("Access token: '{0}'", access_token));
            Debug.WriteLine(string.Format("Refresh token: '{0}'", refresh_token));
            Debug.WriteLine(string.Format("Token type: '{0}'", token_type));
            Debug.WriteLine(string.Format("Expires in: '{0}'", expires_in));
            Debug.WriteLine("---------------------------------------------------------");
            Debug.WriteLine("");
        }

        public static string GetAuthorizationCode(User acc, string scope, string redirectUri, string responseType)
        {
            return GetAuthorizationCode((GoogleUser)acc, scope, redirectUri, responseType);
        }

        public static string GetAuthorizationCode(GoogleUser acc, string scope, string redirectUri, string responseType)
        {
            Debug.WriteLine("");
            Debug.WriteLine("---------------------------------------------------------");
            Debug.WriteLine("-----------OAuth 2.0 authorization information-----------");
            Debug.WriteLine("---------------------------------------------------------");
            Debug.WriteLine(string.Format("Login: '{0}'", acc.EMail));
            string authorizationCode = null;
            string error = null;
            string approveUrl = string.Format("https://accounts.google.com/o/oauth2/auth?redirect_uri={0}&response_type={1}&client_id={2}&scope={3}", redirectUri, responseType, acc.ClientId, scope);

            //var are0 = new AutoResetEvent(false);
            //var t = new Thread(delegate ()
            //{
            //    bool doEvents = true;
            //    WebBrowser browser = new WebBrowser();
            //    browser.AllowNavigation = true;
            //    browser.DocumentCompleted += delegate (object sender, WebBrowserDocumentCompletedEventArgs e) { doEvents = false; };
            //    Form f = new Form();
            //    f.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            //    f.ShowInTaskbar = false;
            //    f.StartPosition = FormStartPosition.Manual;
            //    f.Location = new System.Drawing.Point(-2000, -2000);
            //    f.Size = new System.Drawing.Size(1, 1);
            //    f.Controls.Add(browser);
            //    f.Load += delegate (object sender, EventArgs e)
            //    {
            //        try
            //        {
            //            browser.Navigate("https://accounts.google.com/Logout");
            //            doEvents = true;
            //            while (doEvents) Application.DoEvents();
            //            browser.Navigate("https://accounts.google.com/ServiceLogin?sacu=1");
            //            doEvents = true;
            //            while (doEvents) Application.DoEvents();
            //            HtmlElement loginForm = browser.Document.Forms["gaia_loginform"];
            //            if (loginForm != null)
            //            {
            //                HtmlElement userName = browser.Document.All["Email"];

            //                userName.SetAttribute("value", acc.EMail);
            //                loginForm.InvokeMember("submit");
            //                doEvents = true;
            //                while (doEvents)
            //                    Application.DoEvents();

            //                loginForm = browser.Document.Forms["gaia_loginform"];
            //                HtmlElement passwd = browser.Document.All["Passwd"];
            //                passwd.SetAttribute("value", acc.Password);

            //                loginForm.InvokeMember("submit");
            //                doEvents = true;
            //                while (doEvents)
            //                    Application.DoEvents();
            //            }
            //            else
            //            {
            //                error = "Login form is not found in \n" + browser.Document.Body.InnerHtml;
            //                return;
            //            }
            //            browser.Navigate(approveUrl);
            //            doEvents = true;
            //            while (doEvents) Application.DoEvents();
            //            HtmlElement approveForm = browser.Document.Forms["connect-approve"];
            //            if (approveForm != null)
            //            {
            //                HtmlElement submitAccess = browser.Document.All["submit_access"];
            //                submitAccess.SetAttribute("value", "true");
            //                approveForm.InvokeMember("submit");
            //                doEvents = true;
            //                while (doEvents)
            //                    Application.DoEvents();
            //            }
            //            else
            //            {
            //                error = "Approve form is not found in \n" + browser.Document.Body.InnerHtml;
            //                return;
            //            }
            //            HtmlElement code = browser.Document.All["code"];
            //            if (code != null)
            //                authorizationCode = code.GetAttribute("value");
            //            else
            //                error = "Authorization code is not found in \n" + browser.Document.Body.InnerHtml;
            //        }
            //        catch (Exception ex)
            //        {
            //            error = ex.Message;
            //        }
            //        finally
            //        {
            //            f.Close();
            //        }
            //    };
            //    Application.Run(f);
            //    are0.Set();
            //});
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();
            //are0.WaitOne();
            //if (error != null)
            //{
            //    throw new Exception(error);
            //}

            return authorizationCode;
        }
    }
}
