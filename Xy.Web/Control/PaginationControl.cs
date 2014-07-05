using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Control {
    public class PaginationControl : Xy.Web.Control.IControl {
        private string _map;
        public string Map { get { return _map; } set { _map = value; } }
        private bool _isNeedData;
        public bool IsNeedData { get { return _isNeedData; } }
        private HTMLContainer _innerData;
        public HTMLContainer InnerData { get { return _innerData; } set { _innerData = value; } }


        public PaginationControl() {
            _isNeedData = true;
        }

        private ThreadEntity _threadEntity;
        private string _xsltstring;
        private string _index;
        private string _max;
        private string _size;
        private string _count;
        private string _linktemplate;
        private string _datastring;

        public void Init(System.Collections.Specialized.NameValueCollection createTag, string map, int index) {
            for (int i = 0; i < createTag.Count; i++) {
                switch (createTag.Keys[i]) {
                    case "Index":
                        _index = createTag[i];
                        break;
                    case "Max":
                        _max = createTag[i];
                        break;
                    case "Size":
                        _size = createTag[i];
                        break;
                    case "Count":
                        _count = createTag[i];
                        break;
                    case "Xslt":
                        _xsltstring = createTag[i];
                        break;
                    case "Link":
                        _linktemplate = createTag[i];
                        break;
                }
            }
            _map = string.Concat(map, "PaginationControl", index);
            _isNeedData = string.IsNullOrEmpty(_xsltstring);
        }

        
        public void Handle(ThreadEntity CurrentThreadEntity, Page.PageAbstract CurrentPageClass, HTMLContainer ContentContainer) {
            _threadEntity = CurrentThreadEntity;
            _threadEntity.ControlIndex += 1;

            _index = Xy.Tools.Control.Tag.TransferValue("PageIndex", _index, CurrentPageClass);
            _size = Xy.Tools.Control.Tag.TransferValue("PageSize", _size, CurrentPageClass);
            _max = Xy.Tools.Control.Tag.TransferValue("PageMax", _max, CurrentPageClass);
            _count = Xy.Tools.Control.Tag.TransferValue("PageCount", _count, CurrentPageClass);

            if (_innerData == null || (!string.IsNullOrEmpty(_xsltstring) && _threadEntity.WebSetting.DebugMode)) {
                if (!string.IsNullOrEmpty(_xsltstring)) {
                    using (System.IO.FileStream fs = new System.IO.FileStream(_threadEntity.WebSetting.XsltDir + _xsltstring, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)) {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(fs)) {
                            _innerData.Write(sr.ReadToEnd());
                            sr.Close();
                        }
                        fs.Close();
                    }
                }
            }
            HandleData();
            System.Xml.XPath.XPathDocument xpathDoc = new System.Xml.XPath.XPathDocument(new System.IO.StringReader(_datastring));

            System.Xml.Xsl.XslCompiledTransform xsl = HandleXSLT();

            ContentContainer.Write(Xy.Tools.IO.XML.TransfromXmlStringToHtml(xsl, xpathDoc));
            _threadEntity.ControlIndex -= 1;
        }

        private System.Xml.Xsl.XslCompiledTransform HandleXSLT() {
            string xsltindata = _innerData.ToString();
            StringBuilder xsltsb = new StringBuilder();
            if (!string.IsNullOrEmpty(_xsltstring)) {
                xsltsb.Append(xsltindata);
            } else {
                xsltsb.Append("<?xml version=\"1.0\" encoding=\"").Append(_threadEntity.WebSetting.Encoding.BodyName).Append("\"?>");
                xsltsb.Append("<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\">");
                xsltsb.Append("<xsl:output method=\"html\" omit-xml-declaration=\"yes\" version=\"1.0\" encoding=\"").Append(_threadEntity.WebSetting.Encoding.BodyName).Append("\" />");
                xsltsb.Append("<xsl:template match=\"/\">");
                xsltsb.Append(xsltindata);
                xsltsb.Append("</xsl:template>");
                xsltsb.Append("</xsl:stylesheet>");
            }
            if (!string.IsNullOrEmpty(_linktemplate)) {
                xsltsb.Replace("{link}", string.Format(_linktemplate, _index, _max));
            }
            string _xsltTemplate = xsltsb.ToString();
            System.Xml.Xsl.XslCompiledTransform xslTransform = Xy.Web.Cache.XslCompiledTransform.Get(_xsltTemplate);
            return xslTransform;
        }

        private void HandleData() {
            int PageIndex = Convert.ToInt32(_index);
            int PageCount = Convert.ToInt32(_count);
            int PageMax = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(_max) / Convert.ToDecimal(_size)));

            if (PageCount % 2 == 0) { throw new Exception("PageCount只能为奇数"); }
            
            StringBuilder _tempsb = new StringBuilder();
            int pagestart = PageIndex - Convert.ToInt32(Math.Floor(PageCount / 2f));
            int pageend = PageIndex + Convert.ToInt32(Math.Floor(PageCount / 2f));
            if (PageMax < 0) {
                throw new Exception("页码中出现了负数");
            } else if (PageMax <= 1) {
                _datastring = "<Pages Max=\"1\" Current=\"1\"><Page Index=\"1\" /></Pages>";
                return;
            } else if (PageMax <= PageCount) {
                pagestart = 1;
                pageend = PageMax;
            } else {
                int temp = 0;
                if (pagestart < 1) {
                    temp = 1 - pagestart;
                    pagestart = 1;
                    pageend += temp;
                }
                if (pageend > PageMax) {
                    temp = pageend - PageMax;
                    pageend = PageMax;
                    pagestart -= temp;
                }
                if (pagestart < 1) { pagestart = 1; }
            }
            _tempsb.Append(string.Format("<Pages Max=\"{0}\" Current=\"{1}\">", PageMax, PageIndex));
            for (int i = pagestart; i <= pageend; i++) {
                _tempsb.Append(string.Format("<Page Index=\"{0}\" />", i));
            }
            _tempsb.Append("</Pages>");
            _datastring = _tempsb.ToString();
        }
    }
}
