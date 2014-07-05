using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Page {
    public interface IPageSessionCollection {
        IPageSession CreateSession(ThreadEntity te);
        IPageSession GetSession(ThreadEntity te);
    }
    public interface IPageSession {
        string this[string index] { get; set; }
    }
    public class PageSessionCollection : IPageSessionCollection {

        private static Dictionary<string, IPageSession> _sessionCollection;
        private static PageSessionCollection _instance;

        private PageSessionCollection() {
            _sessionCollection = new Dictionary<string, IPageSession>();
        }

        public static PageSessionCollection GetInstance() {
            if (_instance == null) {
                _instance = new PageSessionCollection();
            }
            return _instance;
        }

        public IPageSession CreateSession(ThreadEntity threadEntity) {
            string Key = Guid.NewGuid().ToString();
            System.Web.HttpContext hcr = threadEntity.WebContext;
            hcr.Response.Cookies.Add(new System.Web.HttpCookie(threadEntity.WebSetting.UserKeyCookieName, Key) { Expires = DateTime.Now.AddMinutes(threadEntity.WebSetting.SessionOutTime) });
            return CreateSession(Key);
        }

        private IPageSession CreateSession(string Key) {
            IPageSession ips = new PageSession();
            _sessionCollection.Add(Key, ips);
            return ips;
        }

        public IPageSession GetSession(ThreadEntity threadEntity) {
            if (threadEntity.WebContext.Request.Cookies[threadEntity.WebSetting.UserKeyCookieName] == null) {
                return CreateSession(threadEntity);
            } else {
                string Key = threadEntity.WebContext.Request.Cookies[threadEntity.WebSetting.UserKeyCookieName].Value;
                if (!_sessionCollection.ContainsKey(Key)) {
                    return CreateSession(Key);
                }
                return _sessionCollection[Key];
            }
        }
    }

    public class PageSession : IPageSession {

        private System.Collections.Specialized.NameValueCollection _session;

        internal PageSession() {
            _session = new System.Collections.Specialized.NameValueCollection();
        }

        public string this[string index] {
            get {
                return _session[index];
            }
            set {
                if (_session[index] == null) {
                    _session.Add(index, value);
                } else {
                    _session[index] = value;
                }
            }
        }
    }
}
