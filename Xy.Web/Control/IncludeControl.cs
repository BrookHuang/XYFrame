using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Xy.Web.Control {
    public class IncludeControl : Xy.Web.Control.IControl {
        private string _map;
        public string Map { get { return _map; } set { _map = value; } }
        private bool _isNeedData;
        public bool IsNeedData { get { return _isNeedData; } }
        private HTMLContainer _innerData = null;
        public HTMLContainer InnerData { get { return _innerData; } set { _innerData = value; } }

        private ThreadEntity _threadEntity;
        private bool _enableScript = false;
        private string _file;
        private string _type;
        private bool _cached = false;
        private System.Collections.Specialized.NameValueCollection _extValues = null;
        private Xy.WebSetting.WebSettingItem _webSetting = null;
        private string _valueString = string.Empty;

        public IncludeControl() {
            _isNeedData = false;
        }

        public void Init(System.Collections.Specialized.NameValueCollection CreateTag,string map, int index) {
            for (int i = 0; i < CreateTag.Count; i++) {
                switch (CreateTag.Keys[i]) {
                    case "File":
                        _file = CreateTag[i];
                        break;
                    case "Type":
                        _type = CreateTag[i];
                        break;
                    case "EnableScript":
                        if (string.Compare(CreateTag[i], "true", true) == 0) {
                            _enableScript = true;
                        }
                        break;
                    case "EnableCache":
                        if (string.Compare(CreateTag[i], "true", true) == 0) {
                            _cached = true;
                        }
                        break;
                    case "Value":
                        _valueString = CreateTag[i];
                        _extValues = Xy.Tools.Control.Tag.Decode(_valueString);
                        break;
                    case "Config":
                        _webSetting = Xy.WebSetting.WebSettingCollection.GetWebSetting(CreateTag[i]);
                        break;
                }
            }
            _map = string.Concat(map, "IncludeControl", index);
        }

        public void Handle(ThreadEntity CurrentThreadEntity, Page.PageAbstract CurrentPageClass, HTMLContainer ContentContainer) {
            _threadEntity = CurrentThreadEntity;
            _threadEntity.ControlIndex++;
            if (_webSetting == null) _webSetting = CurrentPageClass.WebSetting;
            HTMLContainer _container = new HTMLContainer(_webSetting.Encoding);
            Xy.Web.Page.PageAbstract _page;
            if (string.IsNullOrEmpty(_type)) _type = "Xy.Web,Xy.Web.Page.EmptyPage";
            _page = Runtime.Web.PageClassLibrary.Get(_type);
            _page.Init(CurrentPageClass, _webSetting, _container);
            if (_extValues != null) {
                for (int i = 0; i < _extValues.Count; i++) {
                    if (_page.Request.Values[_extValues.Keys[i]] != null) {
                        _page.Request.Values[_extValues.Keys[i]] = _extValues[i];
                    } else {
                        _page.Request.Values.Add(_extValues.Keys[i], _extValues[i]);
                    }
                }
            }
            string _staticCacheDir = string.Empty, _staticCacheFile = string.Empty, _staticCachePath = string.Empty;
            if (_cached) {
                _staticCacheDir = _webSetting.CacheDir + "IncludeCache\\" + _threadEntity.URL.Dir.Replace('/', '\\');
                _staticCacheFile = _file + _valueString;
                _staticCachePath = _staticCacheDir + _staticCacheFile + ".xycache";
                if (!_page.UpdateCache(_staticCachePath, DateTime.Now)) {
                    if (System.IO.File.Exists(_staticCachePath)) {
                        ContentContainer.Write(System.IO.File.ReadAllBytes(_staticCachePath));
                        return;
                    }
                }
            }
            if (_innerData != null && _innerData.HasContent) {
                _page.SetContent(_innerData);
            }
            _page.Handle(_map, _file, _enableScript, true);
            if (_cached) {
                Xy.Tools.IO.File.ifNotExistsThenCreate(_staticCachePath);
                using (System.IO.FileStream fs = new System.IO.FileStream(_staticCachePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs)) {
                        try {
                            sw.Write(_container.ToString());
                            sw.Flush();
                        } finally {
                            sw.Close();
                            fs.Close();
                        }
                    }
                }
            }
            ContentContainer.Write(_container);
            //ContentContainer.Write(_page.HTMLContainer);
            _threadEntity.ControlIndex--;
        }
    }
}
