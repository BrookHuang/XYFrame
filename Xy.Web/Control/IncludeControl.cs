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
                        _extValues = Xy.Tools.Control.Tag.Decode(CreateTag[i]);
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
            //Xy.Tools.Web.UrlAnalyzer _url = CurrentThreadEntity.URL;
            //if (_webSetting == null)
            //    _webSetting = CurrentPageClass.WebSetting;
            //if (_webSetting.AutoUrl) _webSetting.UpdateDomain(CurrentThreadEntity.URL.Site);
            //URLManage.URLItem _urlItem = new URLManage.URLItem(_file, _type, _threadEntity.URLItem.Regex.ToString(), _threadEntity.URLItem.Mime, _enableScript, _enableCache, _threadEntity.URLItem.ContentType.ToString(), _threadEntity.URLItem.Age.Minutes);
            //ThreadEntity _entity = new ThreadEntity(CurrentThreadEntity.WebContext, _webSetting, _urlItem, _url, true);
            //_entity.Handle(_extValues);
            //if (_entity.Content.HasContent)
            //    _innerHTML.Write(_entity.Content);
            Xy.Web.Page.PageAbstract _page;
            if (string.IsNullOrEmpty(_type)) _type = "Xy.Web,Xy.Web.Page.EmptyPage";
            _page = Runtime.Web.PageClassLibrary.Get(_type);
            _page.Init(CurrentPageClass, _webSetting == null ? CurrentPageClass.WebSetting : _webSetting);
            _page.SetNewContainer(ContentContainer);
            if (_extValues != null) {
                for (int i = 0; i < _extValues.Count; i++) {
                    if (_page.Request.Values[_extValues.Keys[i]] != null) {
                        _page.Request.Values[_extValues.Keys[i]] = _extValues[i];
                    } else {
                        _page.Request.Values.Add(_extValues.Keys[i], _extValues[i]);
                    }
                }
            }
            if (_innerData != null && _innerData.HasContent) {
                _page.SetContent(_innerData);
            }
            _page.Handle(_map, _file, _enableScript, true);
            //ContentContainer.Write(_page.HTMLContainer);
            _threadEntity.ControlIndex--;
        }
    }
}
