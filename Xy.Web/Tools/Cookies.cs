using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Xy.Tools.Web {
    public static class Cookie {
        public static void AddCookie(HttpCookie Cookie) {
            System.Web.HttpContext hcr = System.Web.HttpContext.Current;
            if (hcr.Response.Cookies[Cookie.Name] == null) {
                hcr.Response.Cookies.Add(Cookie);
            } else {
                System.Web.HttpCookie _existCookie = hcr.Response.Cookies[Cookie.Name];
                _existCookie.Value = Cookie.Value;
                _existCookie.Expires = Cookie.Expires;
                //hcr.Response.SetCookie(Cookie);
            }
        }

        public static void Add(string Name, string Value) {
            Add(Name, Value, 20, "/");
        }

        public static void Add(string Name, string Value, int Expire) {
            Add(Name, Value, Expire, "/");
        }

        public static void Add(string Name, string Value, int Expire, string Domain) {
            System.Web.HttpCookie _cookie = new System.Web.HttpCookie(Name, Value);
            if (Expire != 0) _cookie.Expires = DateTime.Now.AddMinutes(Expire);
            if (!string.IsNullOrEmpty(Domain)) _cookie.Domain = Domain;
            AddCookie(_cookie);
        }

        public static void Del(string Name) {
            HttpCookie hc = GetCookie(Name);
            if (hc != null) {
                hc.Value = string.Empty;
                hc.Expires = DateTime.Now.AddDays(-1);
                AddCookie(hc);
            }
        }

        public static string Get(string Name) {
            System.Web.HttpCookie hc = GetCookie(Name);
            if (hc != null) {
                return hc.Value;
            }
            return string.Empty;
        }

        public static HttpCookie GetCookie(string Name) {
            System.Web.HttpContext hcr = System.Web.HttpContext.Current;
            if (hcr.Request.Cookies[Name] != null) {
                return hcr.Request.Cookies[Name];
            }
            return null;

        }
    }
}
