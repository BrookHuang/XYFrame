using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Page {
    public enum PageDataType {
        DataTable,
        String,
        Array,
        Xml
        
    }
    public class PageDataItem {

        private string _name;
        public string Name { get { return _name; } }
        private PageDataType _type;
        public PageDataType Type { get { return _type; } }

        private System.Data.DataTable _dataTable;
        private System.Xml.XPath.XPathDocument _dataXml;
        private Array _dataArray;
        private string _dataString;
        private string _pagination;
        internal bool _pageable;


        public PageDataItem(string name, System.Data.DataSet dataSet)
            : this(name, dataSet.Tables[0]) {
        }

        public PageDataItem(string name, System.Data.DataTable dataTable) {
            _type = PageDataType.DataTable;
            _name = name;
            _dataTable = dataTable;
        }

        public PageDataItem(string name, string dataString) {
            _type = PageDataType.String;
            _name = name;
            _dataString = dataString;
        }

        public PageDataItem(string name, System.Xml.XPath.XPathDocument dataXml) {
            _type = PageDataType.Xml;
            _name = name;
            _dataXml = dataXml;
        }

        public PageDataItem(string name, Array array) {
            _type = PageDataType.Array;
            _name = name;
            _dataArray = array;
        }

        public string CreatePagination(int CurrentPage, int PageSize, int DataCount, int PageCount) {
            if (_type != PageDataType.DataTable && _type != PageDataType.Xml) {
                throw new Exception("only DataTable or XPathDocument can be create pagination");
            }
            _pagination = string.Empty;
            if (PageCount % 2 == 0) { throw new Exception("PageCount should just odd"); }
            int PageMax = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(DataCount) / Convert.ToDecimal(PageSize)));
            int PageIndex = CurrentPage + 1;
            StringBuilder _tempsb = new StringBuilder();
            int pagestart = PageIndex - Convert.ToInt32(Math.Floor(PageCount / 2f));
            int pageend = PageIndex + Convert.ToInt32(Math.Floor(PageCount / 2f));
            if (PageMax < 0) {
                throw new Exception("PageMax less the 0");
            } else if (PageMax <= 1) {
                _pagination = "<Pages Max=\"1\" Current=\"1\"><Page Index=\"1\" /></Pages>";
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
            if (string.IsNullOrEmpty(_pagination)) {
                _tempsb.Append(string.Format("<Pages Max=\"{0}\" Current=\"{1}\">", PageMax, PageIndex));
                for (int i = pagestart; i <= pageend; i++) {
                    _tempsb.Append(string.Format("<Page Index=\"{0}\" />", i));
                }
                _tempsb.Append("</Pages>");
                _pagination = _tempsb.ToString();
            }
            if (_type == PageDataType.Xml && !_pageable) {
                System.Xml.XPath.XPathNavigator _xPathNavigator = _dataXml.CreateNavigator();
                System.Xml.XmlDocument _xmlDocument = new System.Xml.XmlDocument();
                _xmlDocument.LoadXml(_xPathNavigator.OuterXml);
                _xPathNavigator = _xmlDocument.CreateNavigator();
                _xPathNavigator.MoveToFirstChild();
                _xPathNavigator.AppendChild(_pagination);
                _dataXml = new System.Xml.XPath.XPathDocument(new System.IO.StringReader(_xPathNavigator.OuterXml));
            }
            _pageable = true;
            return _pagination;
        }

        public System.Data.DataTable GetDataTable() {
            switch (_type) {
                case PageDataType.DataTable:
                    return _dataTable;
                case PageDataType.Xml:
                    return Xy.Tools.IO.XML.ConvertXMLFileToDataSet(_dataXml.CreateNavigator().OuterXml);
                case PageDataType.String:
                case PageDataType.Array:
                    System.Data.DataTable _tempTable = new System.Data.DataTable();
                    _tempTable.Columns.Add(_name);
                    if (_type == PageDataType.String) {
                        System.Data.DataRow _tempRow = _tempTable.NewRow();
                        _tempRow[_name] = _dataString;
                        _tempTable.Rows.Add(_tempRow);
                    } else {
                        for (int i = 0; i < _dataArray.Length; i++) {
                            System.Data.DataRow _tempRow = _tempTable.NewRow();
                            _tempRow[_name] = _dataArray.GetValue(i).ToString();
                            _tempTable.Rows.Add(_tempRow);
                        }
                    }
                    return _tempTable;
            }
            return null;
        }

        public string GetDataString() {
            switch (_type) {
                case PageDataType.String:
                    return _dataString;
                case PageDataType.DataTable:
                    if (_pageable) {
                        return Xy.Tools.IO.XML.ConvertDataTableToXML(_dataTable, _pagination);
                    } else {
                        return Xy.Tools.IO.XML.ConvertDataTableToXML(_dataTable);
                    }
                case PageDataType.Xml:
                    return _dataXml.CreateNavigator().OuterXml;
                case PageDataType.Array:
                    StringBuilder _tempBuilder = new StringBuilder();
                    for (int i = 0; i < _dataArray.Length; i++) {
                        if (i > 0) _tempBuilder.Append(',');
                        _tempBuilder.Append(_dataArray.GetValue(i).ToString());
                    }
                    return _tempBuilder.ToString();
            }
            return string.Empty;
        }

        public System.Xml.XPath.XPathDocument GetDataXml() {
            switch (_type) {
                case PageDataType.Xml:
                    return _dataXml;
                case PageDataType.DataTable:
                    if (_pageable) {
                        return new System.Xml.XPath.XPathDocument(new System.IO.StringReader(Xy.Tools.IO.XML.ConvertDataTableToXML(_dataTable, _pagination)));
                    } else {
                        return new System.Xml.XPath.XPathDocument(new System.IO.StringReader(Xy.Tools.IO.XML.ConvertDataTableToXML(_dataTable)));
                    }
                case PageDataType.String:
                    return new System.Xml.XPath.XPathDocument(new System.IO.StringReader("<" + _name + ">" + _dataString + "</" + _name + ">"));
                case PageDataType.Array:
                    StringBuilder _xmlBuilder = new StringBuilder();
                    _xmlBuilder.Append("<DataTable>");
                    for (int i = 0; i < _dataArray.Length; i++) {
                        _xmlBuilder.AppendFormat("<{0}><![CDATA[{1}]]></{0}>", _name, _dataArray.GetValue(i).ToString());
                    }
                    _xmlBuilder.Append("</DataTable>");
                    return new System.Xml.XPath.XPathDocument(new System.IO.StringReader(_xmlBuilder.ToString()));
            }
            return new System.Xml.XPath.XPathDocument(new System.IO.StringReader("<Empty></Empty>"));
        }
    }
}
