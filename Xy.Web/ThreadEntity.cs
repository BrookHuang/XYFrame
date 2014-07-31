using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web {
    public class ThreadEntity {
        public enum ThreadType {
            Application,
            Resource,
            Error
        }

        private Xy.Tools.Web.UrlAnalyzer _url;
        public Xy.Tools.Web.UrlAnalyzer URL { get { return _url; } }

        internal Xy.WebSetting.WebSettingItem _webSetting;
        internal Xy.WebSetting.WebSettingItem WebSetting { get { return _webSetting; } }

        private System.Web.HttpContext _webContext;
        public System.Web.HttpContext WebContext { get { return _webContext; } }

        private URLManage.URLItem _urlItem;
        public URLManage.URLItem URLItem { get { return _urlItem; } }

        private HTMLContainer _content;
        public HTMLContainer Content { get { return _content; } }

        private int _controlIndex;
        private int _controlCount;
        private int _controlDeep;
        public int ControlIndex {
            get {
                return _controlIndex;
            }
            set {
                _controlIndex = value;
                if (_controlDeep < _controlIndex) {
                    _controlDeep = _controlIndex;
                }
            }
        }
        public int ControlCount { get { return _controlCount; } set { _controlCount = value; } }
        public int ControlDeep { get { return _controlDeep; } }

        public ThreadEntity(System.Web.HttpContext webApp, WebSetting.WebSettingItem webSetting, URLManage.URLItem urlItem, Xy.Tools.Web.UrlAnalyzer currentURL) {
            _webContext = webApp;
            _webSetting = webSetting;
            _urlItem = urlItem;
            _url = currentURL;
            _content = new HTMLContainer(_webSetting.Encoding);
        }

        public void Handle() {
            if (!string.IsNullOrEmpty(_urlItem.Mime))
                _webContext.Response.ContentType = _urlItem.Mime;
            switch (_urlItem.ContentType) {
                case URLManage.URLType.MainContent:
                    //_webContext.Response.ContentType += "; charset=" + _webSetting.Encoding.WebName;
                    //_webContext.Response.HeaderEncoding = _webSetting.Encoding;
                    Xy.Web.Page.PageAbstract _page = Runtime.Web.PageClassLibrary.Get(_urlItem.PageClassName);
#if DEBUG
                    Xy.Tools.Debug.Log.WriteEventLog("page created");
#endif
                    _page.Init(this, this._webSetting);
#if DEBUG
                    Xy.Tools.Debug.Log.WriteEventLog("page inited");
#endif
                    try {
                        _page.Handle("@PageDir:" + _urlItem.PagePath, _urlItem.PagePath, _urlItem.EnableScript, false);
#if DEBUG
                        Xy.Tools.Debug.Log.WriteEventLog("page handled");
#endif
                    } catch (Page.PageEndException) {
                        _content = _page.HTMLContainer;
                        return;
                    } catch (Exception ex) {
                        try {
                            Page.PageErrorState _pes = _page.onError(ex);
                            if ((_pes & Page.PageErrorState.WriteLog) == Page.PageErrorState.WriteLog) {
                                string _errorLogContent = _buildErrorString(_page, ex);
                                Xy.Tools.Debug.Log.WriteErrorLog(ex.Message);
                            }
                            if ((_pes & Page.PageErrorState.ThrowOut) == Page.PageErrorState.ThrowOut) {
                                throw new Exception("A error occur on " + _urlItem.PageClassName, ex);
                            }
                        } catch (Page.PageEndException) {
                            return;
                        }
                    }
                    _content = _page.HTMLContainer;
                    break;
                case URLManage.URLType.ResourceFile:
                    DateTime _modifiedTime = System.IO.File.GetLastWriteTime(_webContext.Request.PhysicalPath);
                    if (Xy.Tools.IO.File.IsClientCached(_webContext.Request.Headers["If-Modified-Since"], _modifiedTime) && !_webSetting.DebugMode) {
                        WebContext.Response.StatusCode = 304;
                        WebContext.Response.SuppressContent = true;
                    } else {
                        WebContext.Response.Cache.SetLastModified(DateTime.Now);
                        if (_urlItem.Age.TotalMinutes > 0) {
                            WebContext.Response.Cache.SetMaxAge(_urlItem.Age);
                            WebContext.Response.Cache.SetExpires(DateTime.Now.Add(_urlItem.Age));
                            WebContext.Response.Expires = Convert.ToInt32(_urlItem.Age.TotalMinutes);
                            WebContext.Response.ExpiresAbsolute = DateTime.Now.Add(_urlItem.Age);
                            WebContext.Response.AddHeader("Cache-Control", "max-age=" + _urlItem.Age.TotalMinutes);
                        }
                        _webContext.Response.TransmitFile(_webContext.Request.PhysicalPath);
                    }
                    break;
                case URLManage.URLType.Prohibit:
                    throw new Exception("Access denied " + URL.ToString());
            }
        }

        private string _buildErrorString(Page.PageAbstract _page, Exception ex) {
            StringBuilder _errorString = new StringBuilder();
            Exception exception = ex;
            _errorString.Append("WrongTime:").AppendLine(DateTime.Now.ToString());
            _errorString.Append("ClientIP:").AppendLine(_page.Request.UserHostAddress);
            _errorString.Append("ClientBrowser:").AppendLine(string.Format("{0} | {1}", _page.Request.Browser.Type, _page.Request.Browser.Browser));
            _errorString.Append("UserAgent:").AppendLine(_page.Request.UserAgent);
            _errorString.Append("URL:").AppendLine(_page.Request.Url.ToString());
            _errorString.Append("Message:").AppendLine(exception.Message);

            Exception inex = exception;
            int i = 1;
            while (inex != null) {
                _errorString.AppendLine("=============================Exception No." + i++ + "=============================");
                _errorString.AppendLine("Message:" + inex.Message);
                _errorString.AppendLine("Source:" + inex.Source);
                _errorString.AppendLine("TargetSite:" + inex.TargetSite);

                StringBuilder _tsb = new StringBuilder();
                if (inex.Data != null) {
                    foreach (object _entry in inex.Data.Keys) {
                        _tsb.Append(string.Format("{0}:{1}" + Environment.NewLine, _entry.ToString(), inex.Data[_entry]));
                    }
                    _errorString.AppendLine("Data:" + _tsb.ToString());
                }

                _errorString.AppendLine("StackTrace:" + inex.StackTrace);
                inex = inex.InnerException;
            }
            return _errorString.ToString();
        }
    }
}
