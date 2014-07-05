using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Control {
    public class EmptyControl : Xy.Web.Control.IControl {
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

        public void Init(System.Collections.Specialized.NameValueCollection CreateTag, int Index) {
            _id = Xy.Web.Control.ControlFactory.UNNAME + "EmptyControl" + Index;
        }
        public void Handle(ThreadEntity CurrentThreadEntity, Page.Page CurrentPageClass) { _threadEntity = CurrentThreadEntity; }
        public string BuildHtmlString() {
            return string.Empty;
        }

        public EmptyControl() {
            _isNeedData = false;
            _insertIndex = 0;
            _innerHtml = new List<byte>();
            _innerData = new List<byte>();
        }

        public IControl GetInstance() {
            return new EmptyControl();
        }
    }
}
