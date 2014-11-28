using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Control {

    public class ControlAnalyze {

        private byte[] _indexStartBytes;
        private byte[] _indexEndBytes;
        private byte[] _content;
        private ThreadEntity _currentThreadEntity;
        private int _point = 0;
        private string _map;
        private static byte[] bomBuffer = new byte[] { 0xef, 0xbb, 0xbf };

        private List<IControl> _controlCollection;
        public List<IControl> ControlCollection { get { return _controlCollection; } }
        private AnalyzeResultCollection _analyzeResultCollection;
        public AnalyzeResultCollection AnalyzeResultCollection { get { return _analyzeResultCollection; } }
        private bool _isHandled = false;
        public bool IsHandled { get { return _isHandled; } }

        public ControlAnalyze(ThreadEntity currentTheadEntity, string map, bool useInnerMark = false) {
            if (useInnerMark) {
                _indexStartBytes = AppSetting.INNER_START_MARK;
                _indexEndBytes = AppSetting.INNER_END_MARK;
            } else {
                _indexStartBytes = AppSetting.START_MARK;
                _indexEndBytes = AppSetting.END_MARK;
            }
            _currentThreadEntity = currentTheadEntity;
            _analyzeResultCollection = new Control.AnalyzeResultCollection();
            _controlCollection = new List<IControl>();
            _content = new byte[0];
            _map = map;
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog(_map + " control analyze created");
#endif
        }

        public void SetContent(byte[] content) {
            _content = content;
            if (_content.Length > 3
               && _content[0] == bomBuffer[0]
               && _content[1] == bomBuffer[1]
               && _content[2] == bomBuffer[2]) {
                _point = 3;
            } else {
                _point = 0;
            }
            _isHandled = false;
            _analyzeResultCollection = new Control.AnalyzeResultCollection();
            _controlCollection = new List<IControl>();
        }

        public void Analyze() {
            if (_content.Length == 0) return;
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog(_map + " start control analyze");
#endif
            int _controlStartPoint = 0;
            int _controlEndPoint = 0;
            int _controlDeep = 0;
            int _lastPoint = _point;
            int _controlEndPointHold = 0;
            IControl _controlHold = null;
            string _controlCreateString;
            byte[] _tempBytes;
            HTMLContainer _container;

            while ((_controlStartPoint = findNextStartTag()) > 0 && _lastPoint < _content.Length) {
                _controlEndPoint = findNextEndTag();
                if (_controlEndPoint == -1) throw new Exception("Unclosed tag");
                _tempBytes = new byte[_controlEndPoint - _controlStartPoint];
                Buffer.BlockCopy(_content, _controlStartPoint, _tempBytes, 0, _tempBytes.Length);
                _controlCreateString = _currentThreadEntity.WebSetting.Encoding.GetString(_tempBytes).Trim();
                if (_controlDeep > 0) {
                    if (_controlCreateString.IndexOf("@End") == 0) {
                        _controlDeep--;
                        if (_controlDeep == 0) {
                            _tempBytes = new byte[_controlStartPoint - 2 - _controlEndPointHold];
                            Buffer.BlockCopy(_content, _controlEndPointHold, _tempBytes, 0, _tempBytes.Length);
                            _container = new HTMLContainer(_currentThreadEntity.WebSetting.Encoding);
                            _container.Write(_tempBytes);
                            _controlHold.InnerData = _container;
                        }
                    } else {
                        if (CreateControl(_controlCreateString, true).IsNeedData) _controlDeep++;
                    }
                } else {
                    _container = new HTMLContainer(_currentThreadEntity.WebSetting.Encoding);
                    _tempBytes = new byte[_controlStartPoint - 2 - _lastPoint];
                    Buffer.BlockCopy(_content, _lastPoint, _tempBytes, 0, _tempBytes.Length);
                    _container.Write(_tempBytes);
                    _analyzeResultCollection.Add(new AnalyzeResult() {
                        Type = AnalyzeResultType.PureHTML,
                        PureHTML = _container,
                        Map = _map + ".PurlHTML" + _lastPoint + "_" + (_controlStartPoint - 2)
                    });
                    //_contentList.Add(_container);

                    //_container = new HTMLContainer(_currentThreadEntity.WebSetting.Encoding);
                    //_contentList.Add(_container);
                    _controlHold = CreateControl(_controlCreateString);
                    if (_controlHold.IsNeedData) { _controlDeep++; _controlEndPointHold = _controlEndPoint + 2; }
                    _controlCollection.Add(_controlHold);
                    _analyzeResultCollection.Add(new AnalyzeResult() {
                        Type = AnalyzeResultType.Control,
                        Control = _controlHold,
                        Map = _controlHold.Map
                    });
                }
                _lastPoint = _controlEndPoint + 2;
            }
            if (_lastPoint < _content.Length) {
                _container = new HTMLContainer(_currentThreadEntity.WebSetting.Encoding);
                _tempBytes = new byte[_content.Length - _lastPoint];
                Buffer.BlockCopy(_content, _lastPoint, _tempBytes, 0, _tempBytes.Length);
                _container.Write(_tempBytes);
                //_contentList.Add(_container);
                _analyzeResultCollection.Add(new AnalyzeResult() {
                    Type = AnalyzeResultType.PureHTML,
                    PureHTML = _container,
                    Map = _map + "PurlHTML" + _lastPoint + "_" + _content.Length
                });
            }
            _isHandled = true;
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog(_map + " finish control analyze");
#endif
        }

        public int findNextStartTag() {
            while (_point < _content.Length && (_point = Array.IndexOf<byte>(_content, _indexStartBytes[1], _point)) >= 0) {
                if (_content[_point - 1].CompareTo(_indexStartBytes[0]) == 0) {
                    _point++;
                    return _point;
                }
                _point++;
            }
            return -1;
        }

        public int findNextEndTag() {
            while (_point < _content.Length && (_point = Array.IndexOf<byte>(_content, _indexEndBytes[0], _point)) >= 0) {
                if (_content[_point + 1].CompareTo(_indexEndBytes[1]) == 0) {
                    _point++;
                    return _point - 1;
                }
                _point++;
            }
            return -1;
        }

        public IControl CreateControl(string createString, bool HoldException = false) {
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog(_map + " found a control " + createString);
#endif
            System.Collections.Specialized.NameValueCollection _tags = Xy.Tools.Control.Tag.Decode(createString);
            IControl _control = Xy.Runtime.Web.ControlFactory.Get(_tags.Keys[0]);
            try { 
                _control.Init(_tags, string.Concat(_map, '.'), _currentThreadEntity.ControlCount++);
            } catch (Exception e) {
                if (!HoldException) throw e;
            }
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog(_map + " " + createString + " control inited get map:" + _control.Map);
#endif
            return _control;
        }

        public void Handle(Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            if (!_isHandled) {
                contentContainer.Write(_content);
                return;
            }
            for (int i = 0; i < _analyzeResultCollection.Count; i++) {
//                if (_isHandled && _controlCollection[i].Cached) {
//                    continue;
//                }
//                try {
//#if DEBUG
//                    Xy.Tools.Debug.Log.WriteEventLog(_controlCollection[i].Map + " control handle start");
//#endif
//                    _controlCollection[i].Handle(_currentThreadEntity, currentPageClass);
//#if DEBUG
//                    Xy.Tools.Debug.Log.WriteEventLog(_controlCollection[i].Map + " control handle end");
//#endif
//                } catch (Exception ex) {
//                    throw new Exception("meet a error on handle control: " + _controlCollection[i].Map, ex);
//                }
                try {
#if DEBUG
                    Xy.Tools.Debug.Log.WriteEventLog(_analyzeResultCollection[i].Map + " control handle start");
#endif
                    _analyzeResultCollection[i].Handle(currentPageClass.ThreadEntity, currentPageClass, contentContainer);
#if DEBUG
                    Xy.Tools.Debug.Log.WriteEventLog(_analyzeResultCollection[i].Map + " control handle end");
#endif
                } catch (Exception ex) {
                    throw new Exception("meet an error on handle content: " + _analyzeResultCollection[i].Map, ex);
                }
            }
        }

    }
}
