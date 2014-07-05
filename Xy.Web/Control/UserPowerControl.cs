using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Control {
    public class UserPowerControl : Xy.Web.Control.IControl {
        private string _map;
        public string Map { get { return _map; } set { _map = value; } }
        private bool _isNeedData;
        public bool IsNeedData { get { return _isNeedData; } }
        private HTMLContainer _innerData;
        public HTMLContainer InnerData { get { return _innerData; } set { _innerData = value; } }

        public UserPowerControl() {
            _isNeedData = true;
            _enableScript = false;
            _useInnerTag = false;
            _hasPowerIndex = 0;
            _hasPowerKey = string.Empty;
            _inGroupIndex = 0;
            _inGroupKey = string.Empty;
        }

        private ThreadEntity _threadEntity;
        private int _hasPowerIndex;
        private string _hasPowerKey;
        private int _inGroupIndex;
        private string _inGroupKey;
        private string _noPowerMessage;
        private bool _enableScript;
        private bool _useInnerTag;

        public void Init(System.Collections.Specialized.NameValueCollection createTag, string map, int index) {
            for (int i = 0; i < createTag.Count; i++) {
                switch (createTag.Keys[i]) {
                    case "PowerIndex":
                        _hasPowerIndex = Convert.ToInt32(createTag[i]);
                        break;
                    case "GroupIndex":
                        _inGroupIndex = Convert.ToInt32(createTag[i]);
                        break;
                    case "PowerKey":
                        _hasPowerKey = createTag[i];
                        break;
                    case "GroupKey":
                        _hasPowerKey = createTag[i];
                        break;
                    case "Message":
                        _noPowerMessage = createTag[i];
                        break;
                    case "EnableScript":
                        _enableScript = string.Compare("true", createTag[i], true) == 0;
                        break;
                    case "UseInnerTag":
                        _useInnerTag = string.Compare("true", createTag[i], true) == 0;
                        break;
                }
            }
            _map = string.Concat(map, "UserPowerControl", index);
        }

        public void Handle(ThreadEntity CurrentThreadEntity, Page.PageAbstract CurrentPageClass, HTMLContainer ContentContainer) {
            _threadEntity = CurrentThreadEntity;
            _threadEntity.ControlIndex += 1;
            HTMLContainer _innerHTML = new HTMLContainer(ContentContainer.Encoding);
            if (CurrentPageClass is Page.UserPage) {
                Page.UserPage up = (Page.UserPage)CurrentPageClass;
                if (up.CurrentUser != null) {
                    if ((_hasPowerIndex > 0 && up.CurrentUser.HasPower(_hasPowerIndex)) || (!string.IsNullOrEmpty(_hasPowerKey) && up.CurrentUser.HasPower(_hasPowerKey))) {
                        if (_inGroupIndex > 0 || !string.IsNullOrEmpty(_inGroupKey)) {
                            if (up.CurrentUser.InGroup(_inGroupIndex) || up.CurrentUser.InGroup(_inGroupKey)) {
                                _innerHTML.Write(_innerData);
                            }
                        } else {
                            _innerHTML.Write(_innerData);
                        }
                    } else if ((_inGroupIndex > 0 && up.CurrentUser.InGroup(_inGroupIndex)) || (!string.IsNullOrEmpty(_hasPowerKey) && up.CurrentUser.InGroup(_inGroupKey))) {
                        _innerHTML.Write(_innerData);
                    }
                }
            }
            if (_innerHTML.Length == 0 && !string.IsNullOrEmpty(_noPowerMessage)) {
                ContentContainer.Write(_noPowerMessage);
            } else {
                if (_enableScript) {
                    Control.ControlAnalyze _controls = Cache.PageAnalyze.GetInstance(CurrentThreadEntity, CurrentPageClass, this.Map);
                    if (!_controls.IsHandled || CurrentPageClass.WebSetting.DebugMode) {
                        _controls.SetContent(_innerHTML.ToArray());
                        _controls.Analyze();
                    }
                    _controls.Handle(CurrentPageClass, ContentContainer);
                } else {
                    ContentContainer.Write(_innerHTML);
                }
            }
            _threadEntity.ControlIndex -= 1;
        }
    }
}
