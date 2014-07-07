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
    }
}
