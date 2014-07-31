using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Xy.Web.URLManage {
    public enum URLType {
        MainContent,
        ResourceFile,
        Prohibit
    }
    public class URLItem {
        private string _pagePath;
        private Regex _regex;
        private string _mime;
        private bool _enableScript;
        private bool _enableCache;
        private URLType _contentType;
        private string _pageClassName;
        private TimeSpan _age;
        private string[] _URLGroupsName;

        public string PagePath { get { return _pagePath; } }
        public string Mime { get { return _mime; } }
        public bool EnableScript { get { return _enableScript; } }
        public bool EnableCache { get { return _enableCache; } }
        public URLType ContentType { get { return _contentType; } }
        public Regex Regex { get { return _regex; } }
        public string PageClassName { get { return _pageClassName; } }
        public TimeSpan Age { get { return _age; } }
        public string[] URLGroupsName { get { return _URLGroupsName; } }

        public const int DEFAULTAGE = 1440;

        internal URLItem(System.Xml.XmlNode URLXml) {
            Init(
                URLXml.Attributes["File"] == null ? null : URLXml.Attributes["File"].Value,
                URLXml.Attributes["Page"] == null ? null : URLXml.Attributes["Page"].Value,
                URLXml.InnerText,
                URLXml.Attributes["Mime"] == null ? null : URLXml.Attributes["Mime"].Value,
                URLXml.Attributes["EnableScript"] == null ? false : (string.Compare(URLXml.Attributes["EnableScript"].Value, "true", true) == 0),
                URLXml.Attributes["EnableCache"] == null ? false : (string.Compare(URLXml.Attributes["EnableCache"].Value, "true", true) == 0),
                URLXml.Attributes["ContentType"] == null ? "MainContent" : URLXml.Attributes["ContentType"].Value,
                URLXml.Attributes["Age"] == null ? DEFAULTAGE : Convert.ToInt32(URLXml.Attributes["Age"].Value)
                );
        }

        public URLItem(string pagePath, string pageClassName, string rexStr, string mime, bool enableScript, bool enableCache, string contentType, int age) {
            Init(pagePath, pageClassName, rexStr, mime, enableScript, enableCache, contentType, age);
        }

        private void Init(string pagePath, string pageClassName, string rexStr, string mime, bool enableScript, bool enableCache, string contentType, int age) {
            _pagePath = pagePath;
            _enableScript = enableScript;
            _enableCache = enableCache;
            if (string.IsNullOrEmpty(pageClassName))
                _pageClassName = "Xy.Web,Xy.Web.Page.EmptyPage";
            else
                _pageClassName = pageClassName;
            Runtime.Web.PageClassLibrary.Add(_pageClassName);
            _age = new TimeSpan(age / 60, age % 60, 0);
            switch (contentType) {
                case "MainContent":
                    _contentType = URLType.MainContent;
                    break;
                case "Prohibit":
                    _contentType = URLType.Prohibit;
                    break;
                case "ResourceFile":
                default:
                    _contentType = URLType.ResourceFile;
                    break;
            }
            if (_enableScript) {
                _contentType = URLType.MainContent;
            }
            if (!string.IsNullOrEmpty(rexStr)) {
                _regex = new Regex(rexStr, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                List<string> _groupsName = new List<string>(_regex.GetGroupNames());
                for (int i = _groupsName.Count - 1; i >= 0; i--) {
                    if (char.IsDigit(_groupsName[i], 0)) _groupsName.RemoveAt(i);
                }
                _URLGroupsName = _groupsName.ToArray();
            }
            if (!string.IsNullOrEmpty(mime)) {
                _mime = mime;
            }
        }

        internal bool IsMatch(string url) {
            if (_regex != null) {
                return _regex.IsMatch(url);
            } else {
                return false;
            }
        }
    }
}
