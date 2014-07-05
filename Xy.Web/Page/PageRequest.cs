using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Collections;

namespace Xy.Web.Page {
    public class PageRequest {
        System.Web.HttpRequest _innerRequest;

        private System.Collections.Specialized.NameValueCollection _values = null;
        System.Collections.Specialized.NameValueCollection _groupNvc;

        internal PageRequest(ThreadEntity entity) {
            _innerRequest = entity.WebContext.Request;
            _values = new System.Collections.Specialized.NameValueCollection();
            _groupNvc = new System.Collections.Specialized.NameValueCollection();

            System.Text.RegularExpressions.GroupCollection _groupMatched = entity.URLItem.Regex.Match(entity.URL.Path).Groups;
            for (int i = 0; i < entity.URLItem.URLGroupsName.Length; i++) {
                string _key = entity.URLItem.URLGroupsName[i];
                _groupNvc.Add(_key, _groupMatched[_key].Value);
            }

            _values.Add(_innerRequest.QueryString);
            _values.Add(_innerRequest.Form);
            _values.Add(_groupNvc);
        }

        #region MS Function
        public string[] AcceptTypes { get { return _innerRequest.AcceptTypes; } }
        
        public string AnonymousID { get { return _innerRequest.AnonymousID; } }
        
        public string ApplicationPath { get { return _innerRequest.ApplicationPath; } }
        
        public string AppRelativeCurrentExecutionFilePath { get { return _innerRequest.AppRelativeCurrentExecutionFilePath; } }

        public System.Web.HttpBrowserCapabilities Browser { get { return _innerRequest.Browser; } set { _innerRequest.Browser = value; } }

        public System.Web.HttpClientCertificate ClientCertificate { get { return _innerRequest.ClientCertificate; } }

        public Encoding ContentEncoding { get { return _innerRequest.ContentEncoding; } set { _innerRequest.ContentEncoding = value; } }
        
        public int ContentLength { get { return _innerRequest.ContentLength; } }

        public string ContentType { get { return _innerRequest.ContentType; } set { _innerRequest.ContentType = value; } }

        public System.Web.HttpCookieCollection Cookies { get { return _innerRequest.Cookies; } }
        
        public string CurrentExecutionFilePath { get { return _innerRequest.CurrentExecutionFilePath; } }
        
        public string FilePath { get { return _innerRequest.FilePath; } }

        public System.Web.HttpFileCollection Files { get { return _innerRequest.Files; } }

        public System.IO.Stream Filter { get { return _innerRequest.Filter; } set { _innerRequest.Filter = value; } }
        
        public NameValueCollection Form { get { return _innerRequest.Form; } }
        
        public NameValueCollection Headers { get { return _innerRequest.Headers; } }
        public System.Security.Authentication.ExtendedProtection.ChannelBinding HttpChannelBinding { get { return _innerRequest.HttpChannelBinding; } }
        
        public string HttpMethod { get { return _innerRequest.HttpMethod; } }
        
        public System.IO.Stream InputStream { get { return _innerRequest.InputStream; } }
        
        public bool IsAuthenticated { get { return _innerRequest.IsAuthenticated; } }
        
        public bool IsLocal { get { return _innerRequest.IsLocal; } }
        
        public bool IsSecureConnection { get { return _innerRequest.IsSecureConnection; } }
        
        public System.Security.Principal.WindowsIdentity LogonUserIdentity { get { return _innerRequest.LogonUserIdentity; } }
        
        public NameValueCollection Params { get { return _innerRequest.Params; } }
        
        public string Path { get { return _innerRequest.Path; } }
        
        public string PathInfo { get { return _innerRequest.PathInfo; } }
        
        public string PhysicalApplicationPath { get { return _innerRequest.PhysicalApplicationPath; } }
        public string PhysicalPath { get { return _innerRequest.PhysicalPath; } }
        
        public NameValueCollection QueryString { get { return _innerRequest.QueryString; } }
        
        public string RawUrl { get { return _innerRequest.RawUrl; } }

        public string RequestType { get { return _innerRequest.RequestType; } set { _innerRequest.RequestType = value; } }
        
        public NameValueCollection ServerVariables { get { return _innerRequest.ServerVariables; } }
        
        public int TotalBytes { get { return _innerRequest.TotalBytes; } }
        
        public Uri Url { get { return _innerRequest.Url; } }
        
        public Uri UrlReferrer { get { return _innerRequest.UrlReferrer; } }
        
        public string UserAgent { get { return _innerRequest.UserAgent; } }
        
        public string UserHostAddress { get { return _innerRequest.UserHostAddress; } }
        
        public string UserHostName { get { return _innerRequest.UserHostName; } }
        
        public string[] UserLanguages { get { return _innerRequest.UserLanguages; } }

        
        //public string this[string key] { get { return _innerRequest[key]; } }


        public byte[] BinaryRead(int count) { return _innerRequest.BinaryRead(count); }

        public int[] MapImageCoordinates(string imageFieldName) { return _innerRequest.MapImageCoordinates(imageFieldName); }

        public string MapPath(string virtualPath) { return _innerRequest.MapPath(virtualPath); }

        public string MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping) { return _innerRequest.MapPath(virtualPath, baseVirtualDir, allowCrossAppMapping); }

        public void SaveAs(string filename, bool includeHeaders) { _innerRequest.SaveAs(filename, includeHeaders); }

        public void ValidateInput() { _innerRequest.ValidateInput(); }
        #endregion

        public string this[string index] {
            get { return _values[index]; }
        }

        public System.Collections.Specialized.NameValueCollection GroupString {
            get { return _groupNvc; }
        }

        public System.Collections.Specialized.NameValueCollection Values {
            get { return _values; }
        }

        public void AddValue(string inName, string inValue) {
            _values.Add(inName, inValue);
        }

        public void AddValues(System.Collections.Specialized.NameValueCollection inValues) {
            _values.Add(inValues);
        }
    }
}
