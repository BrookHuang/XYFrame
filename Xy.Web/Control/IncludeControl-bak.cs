using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Xy.Web.Control {
    public class IncludeControl : Xy.Web.Control.IControl {
        private string _id;
        public string ID { get { return _id; } set { _id = value; } }
        private int _insertIndex;
        public int InsertIndex { get { return _insertIndex; } set { _insertIndex = value; } }
        private bool _isNeedData;
        public bool IsNeedData { get { return _isNeedData; } }
        private List<byte> _innerHtml;
        public List<byte> InnerHtml { get { return _innerHtml; } }
        public int Length { get { return _innerHtml.Count; } }
        private List<byte> _innerData;
        public List<byte> InnerData { get { return _innerData; } set { _innerData = value; } }

        private ThreadEntity _threadEntity;
        private bool _enableScript = false;
        private string _file;
        private Cache.PageHtmlCacheItem _cacheItem;
        private Xy.WebSetting.WebSettingItem _webConfig = null;

        public IncludeControl() {
            _isNeedData = false;
            _insertIndex = 0;
            _innerData = new List<byte>();
            _innerHtml = new List<byte>();
        }

        public void Init(System.Collections.Specialized.NameValueCollection CreateTag, int Index) {
            for (int i = 0; i < CreateTag.Count; i++) {
                switch (CreateTag.Keys[i]) {
                    case "File":
                        _file = CreateTag[i];
                        break;
                    case "EnableScript":
                        if (string.Compare(CreateTag[i], "true", true) == 0) {
                            _enableScript = true;
                        }
                        break;
                    case "Config":
                        _webConfig = Xy.WebSetting.WebSettingCollection.GetWebSetting(CreateTag[i]);
                        break;
                    case "ID":
                        _id = CreateTag[i];
                        break;
                }
            }
            if (string.IsNullOrEmpty(ID)) _id = Xy.Web.Control.ControlFactory.UNNAME + "IncludeControl" + Index;
        }

        public void Handle(ThreadEntity CurrentThreadEntity, Page.Page CurrentPageClass) {
            _threadEntity = CurrentThreadEntity;
            _threadEntity.ControlIndex += 1;
            Xy.WebSetting.WebSettingItem _tempWebConfig = null;
            if (_webConfig != null) {
                _tempWebConfig = _threadEntity.WebSetting;
                _threadEntity.WebSetting = _webConfig;
            }
            InitSourceHtml();
            //if (_enableScript) {
            //    ControlCollection cc = new Control.ControlAnalyze(ref _innerHtml, _threadEntity).ControlCollection;
            //    cc.Handle(ref _innerHtml, ref _threadEntity);
            //}
            if (_enableScript) {
                ControlCollection cc;
                if (_threadEntity.WebSetting.DebugMode) {
                    cc = new Control.ControlAnalyze(_innerHtml, _threadEntity).ControlCollection;
                } else {
                    cc = _cacheItem._controlCollection;
                }
                cc.Handle(_innerHtml, _threadEntity, CurrentPageClass);
            }
            if (_webConfig != null) {
                _threadEntity.WebSetting = _tempWebConfig;
            }
            _threadEntity.ControlIndex -= 1;
        }

        public IControl GetInstance() {
            return new IncludeControl();
        }

        private void InitSourceHtml() {
            if (_threadEntity.WebSetting.DebugMode) {
                using (FileStream fs = new FileStream(_threadEntity.WebSetting.IncludeDir + _file, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    using (StreamReader sr = new StreamReader(fs)) {
                        _innerHtml.AddRange(new List<byte>(_threadEntity.WebSetting.Encoding.GetBytes(sr.ReadToEnd())));
                        sr.Close();
                    }
                    fs.Close();
                }
            } else {
                if (_innerHtml.Count > 0 && !_enableScript) return;
                _innerHtml = new List<byte>();
                _cacheItem = Cache.PageTemplateCache.Get(_threadEntity.WebSetting.IncludeDir + _file, _threadEntity);
                if (_enableScript) {
                    _innerHtml.AddRange(_cacheItem._analyzedHtml);
                } else {
                    _innerHtml.AddRange(_cacheItem._originalHtml);
                }
            }
            //using (FileStream fs = new FileStream(_threadEntity.WebSetting.IncludeDir + _file, FileMode.Open, FileAccess.Read, FileShare.Read)) {
            //    using (StreamReader sr = new StreamReader(fs)) {
            //        _innerHtml.Clear();
            //        _innerHtml.AddRange(new List<byte>(_threadEntity.WebSetting.Encoding.GetBytes(sr.ReadToEnd())));
            //        sr.Close();
            //    }
            //    fs.Close();
            //}
        }

        private void OnEnd() {
        }

        public string BuildHtmlString() {
            return _threadEntity.WebSetting.Encoding.GetString(_innerHtml.ToArray());
        }
    }
}
