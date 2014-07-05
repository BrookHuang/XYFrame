using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Xy.Web.Control {
    public class PageControl : Xy.Web.Control.IControl {
        private string _map;
        public string Map { get { return _map; } set { _map = value; } }
        private int _insertIndex;
        public int InsertIndex { get { return _insertIndex; } set { _insertIndex = value; } }
        private bool _isNeedData;
        public bool IsNeedData { get { return _isNeedData; } }
        private HTMLContainer _innerHTML;
        public HTMLContainer InnerHTML { get { return _innerHTML; } }
        private HTMLContainer _innerData;
        public HTMLContainer InnerData { get { return _innerData; } set { _innerData = value; } }
        private bool _cached;
        public bool Cached { get { return _cached; } set { _cached = value; } }

        private ThreadEntity _threadEntity;
        private bool _enableScript = false;
        private bool _enableCache = false;
        private string _file;
        private string _type;
        private System.Collections.Specialized.NameValueCollection _extValues = null;
        private Xy.WebSetting.WebSettingItem _webSetting = null;

        public PageControl() {
            _isNeedData = false;
            _insertIndex = 0;
        }

        public void Init(System.Collections.Specialized.NameValueCollection CreateTag, HTMLContainer content,string map, int Index) {
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
                            _enableCache = true;
                        }
                        break;
                    case "Value":
                        _extValues = Xy.Tools.Control.Tag.Decode(CreateTag[i]);
                        break;
                    case "Config":
                        _webSetting = Xy.WebSetting.WebSettingCollection.GetWebSetting(CreateTag[i]);
                        break;
                }
            }
            _map = map + "PageControl" + Index;
            _innerHTML = content;
        }

        public void Handle(ThreadEntity CurrentThreadEntity, Page.PageAbstract CurrentPageClass) {
            _threadEntity = CurrentThreadEntity;
            _innerHTML.Clear();
            _threadEntity.ControlIndex++;
            Xy.Tools.Web.UrlAnalyzer _url = CurrentThreadEntity.URL;
            if (_webSetting == null)
                _webSetting = CurrentPageClass.WebSetting;
            if (_webSetting.AutoUrl) _webSetting.UpdateDomain(CurrentThreadEntity.URL.Site);
            URLManage.URLItem _urlItem = new URLManage.URLItem(_file, _type, _threadEntity.URLItem.Regex.ToString(), _threadEntity.URLItem.Mime, _enableScript, _enableCache, _threadEntity.URLItem.ContentType.ToString(), _threadEntity.URLItem.Age.Minutes);
            ThreadEntity _entity = new ThreadEntity(CurrentThreadEntity.WebContext, _webSetting, _urlItem, _url, true);
            _entity.Handle(_extValues);
            if (_entity.Content.HasContent)
                _innerHTML.Write(_entity.Content);
            _threadEntity.ControlIndex--;
        }

        public string BuildHtmlString() {
            return _threadEntity.WebSetting.Encoding.GetString(_innerHTML.ToArray());
        }
    }
}
