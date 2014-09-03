using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Page {
    //public enum PageState : int {
    //    OnGetRequest,
    //    Validate,
    //    UpdateCache,
    //    LoadSourceFile,
    //    HandleTag,
    //    OutputHtml,
    //    EndRequest
    //}

    [Flags]
    public enum PageErrorState {
        Handled = 1,
        WriteLog = 2,
        ThrowOut = 4
    }

    public class PageEndException : Exception {
        public PageEndException() : base() { }
        public PageEndException(string message, Exception inner) : base(message, inner) { }
    }

    public abstract class PageAbstract {

        private byte[] _content = new byte[0];
        private bool _contentChanged = false;
        private HTMLContainer _htmlContainer;
        public HTMLContainer HTMLContainer { get { return _htmlContainer; } }

        private ThreadEntity _threadEntity;
        public ThreadEntity ThreadEntity { get { return _threadEntity; } }
        private PageResponse _response;
        public PageResponse Response { get { return _response; } }
        private PageRequest _request;
        public PageRequest Request { get { return _request; } }
        private System.Web.HttpServerUtility _server;
        public System.Web.HttpServerUtility Server { get { return _server; } }
        private IPageSession _pageSession;
        public IPageSession Session { get { return _pageSession; } }
        private PageData _pageData;
        public PageData PageData { get { return _pageData; } }
        private bool _updateLocalCache;
        public bool UpdateLocalCache { get { return _updateLocalCache; } }
        private Xy.WebSetting.WebSettingItem _webSetting;
        public Xy.WebSetting.WebSettingItem WebSetting { get { return _webSetting; } }
        private Xy.Tools.Web.UrlAnalyzer _url;
        public Xy.Tools.Web.UrlAnalyzer URL { get { return _url; } }

        

        public void Init(ThreadEntity threadEntity, WebSetting.WebSettingItem webSetting) {
            _threadEntity = threadEntity;
            _htmlContainer = new HTMLContainer(_threadEntity.WebSetting.Encoding);
            _server = _threadEntity.WebContext.Server;
            _url = threadEntity.URL;
            _updateLocalCache = false;
            _response = new PageResponse(_threadEntity, _htmlContainer);
            _request = new PageRequest(_threadEntity);
            _pageData = new PageData();
            _pageSession = PageSessionCollection.GetInstance().GetSession(_threadEntity);
            _webSetting = webSetting;
        }

        public void Init(PageAbstract page, WebSetting.WebSettingItem webSetting, HTMLContainer container) {
            _threadEntity = page._threadEntity;
            _server = page._server;
            _updateLocalCache = page._updateLocalCache;
            _request = page._request;
            _pageData = page._pageData;
            _pageSession = page._pageSession;
            _url = page._url;
            _webSetting = webSetting;
            _response = page._response;
            _response.SetNewContainer(container);
            _htmlContainer = container;
        }

        public void SetNewTemplate(string path) {
            if (System.IO.File.Exists(path)) {
                _content = System.IO.File.ReadAllBytes(path);
                _contentChanged = true;
            } else throw new System.IO.FileNotFoundException(string.Format("cannot found \"{0}\"", path));
        }

        #region Internal event
        public virtual void onGetRequest() { }

        public virtual void Validate() { }

        public virtual bool UpdateCache(string CachePath, DateTime CacheTime) { return false; }

        public virtual string LoadSourceFile(string SourceFilePath) { return SourceFilePath; }

        public virtual List<Control.IControl> HandleControl(List<Control.IControl> Controls) { return Controls; }

        public virtual HTMLContainer OutputHtml(HTMLContainer HTMLContainer) { return HTMLContainer; }

        public virtual PageErrorState onError(Exception ex) { return PageErrorState.ThrowOut; }
        #endregion

        protected void End() {
            throw new PageEndException();
        }
        protected void ChangeConfig(string name) {
            _webSetting = Xy.WebSetting.WebSettingCollection.GetWebSetting(name);
        }

        public void SetContent(HTMLContainer content) {
            _content = content.ToArray();
            _contentChanged = true;
        }

        public void Handle(string map, string filePath, bool enableScript, bool isIncludePage) {
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog("start page:" + map);
#endif
            string _staticCacheDir = string.Empty, _staticCacheFile = string.Empty, _staticCachePath = string.Empty;
            if (_threadEntity.URLItem.EnableCache && !isIncludePage) {
                _staticCacheDir = WebSetting.CacheDir + "PageCache\\" + _threadEntity.URL.Dir.Replace('/', '\\');
                _staticCacheFile = filePath + (_threadEntity.URL.HasParam ? _threadEntity.URL.Param.Replace('?', '#') : string.Empty);
                _staticCachePath = _staticCacheDir + _staticCacheFile + ".xyc.aspx";
                if (!UpdateCache(_staticCachePath, DateTime.Now)) {
                    if (System.IO.File.Exists(_staticCachePath)) {
                        DateTime _modifiedTime = System.IO.File.GetLastWriteTime(_staticCachePath);
                        if (Xy.Tools.IO.File.IsClientCached(_request.Headers["If-Modified-Since"], _modifiedTime) && !_webSetting.DebugMode) {
                            _response.StatusCode = 304;
                            _response.SuppressContent = true;
                        } else {
                            _response.Cache.SetLastModified(DateTime.Now);
                            if (_threadEntity.URLItem.Age.TotalMinutes > 0) {
                                _response.Cache.SetMaxAge(_threadEntity.URLItem.Age);
                                _response.Cache.SetExpires(DateTime.Now.Add(_threadEntity.URLItem.Age));
                                _response.Expires = Convert.ToInt32(_threadEntity.URLItem.Age.TotalMinutes);
                                _response.ExpiresAbsolute = DateTime.Now.Add(_threadEntity.URLItem.Age);
                                _response.AddHeader("Cache-Control", "max-age=" + _threadEntity.URLItem.Age.TotalMinutes);
                            }
                            _htmlContainer.Write(System.IO.File.ReadAllBytes(_staticCachePath));
                        }
                        return;
                    }
                }
            }

            onGetRequest();
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog(map + " page process:onGetRequest finished");
#endif
            Validate();
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog(map + " page process:Validate finished");
#endif
            string _sourcefile = string.Empty;
            if (!string.IsNullOrEmpty(filePath)) {
                _sourcefile = LoadSourceFile((isIncludePage ? _webSetting.IncludeDir : _webSetting.PageDir) + filePath);
#if DEBUG
                Xy.Tools.Debug.Log.WriteEventLog(map + " page process:LoadSourceFile finished");
#endif
            }
            Control.ControlAnalyze _controls = Cache.PageAnalyze.GetInstance(_threadEntity, this, map);
            if (!_controls.IsHandled || WebSetting.DebugMode){
                if (!string.IsNullOrEmpty(_sourcefile)) {
                    _content = System.IO.File.ReadAllBytes(_sourcefile);
                    _controls.SetContent(_content);
                }
            }
            if (_contentChanged) {
                _controls.SetContent(_content);
            }
            if (enableScript) {
                if (!_controls.IsHandled) _controls.Analyze();
                HandleControl(_controls.ControlCollection);
            }

#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog(map + " page process:HandleControl finished");
#endif
            _controls.Handle(this, _htmlContainer);
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog(map + " controls handled");
#endif
            OutputHtml(_htmlContainer);
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog(map + " page process:OutputHtml finished");
#endif

            if (_threadEntity.URLItem.EnableCache && !isIncludePage) {
                Xy.Tools.IO.File.ifNotExistsThenCreate(_staticCacheDir);
                using (System.IO.FileStream fs = new System.IO.FileStream(_staticCachePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs)) {
                        try {
                            sw.Write(_htmlContainer.ToString());
                            sw.Flush();
                        } finally {
                            sw.Close();
                            fs.Close();
                        }
                    }
                }
#if DEBUG
                Xy.Tools.Debug.Log.WriteEventLog(map + " page cache writed");
#endif
            }
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog(map + " end");
#endif
        }
    }
}
