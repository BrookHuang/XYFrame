using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Page {
    public class PageData : IEnumerable {
        Dictionary<string, PageDataItem> _dataList;

        internal PageData() {
            _dataList = new Dictionary<string, PageDataItem>();
        }

        public PageDataItem this[string index] {
            get {
                PageDataItem pdi;
                _dataList.TryGetValue(index, out pdi);
                return pdi;
            }
        }

        public PageDataItem Add(PageDataItem dataItem) {
            return inAdd(dataItem.Name, dataItem);
        }

        public PageDataItem Add(string name, System.Xml.XPath.XPathDocument dataXml) {
            return inAdd(name, new PageDataItem(name, dataXml));
        }

        public PageDataItem Add(string name, System.Data.DataSet dataSet) {
            return inAdd(name, new PageDataItem(name, dataSet));            
        }

        public PageDataItem Add(string name, System.Data.DataTable dataTable) {
            return inAdd(name, new PageDataItem(name, dataTable));
        }

        public PageDataItem Add(string name, long msg) {
            return inAdd(name, new PageDataItem(name, msg.ToString()));
        }

        public PageDataItem Add(string name, string msg) {
            return inAdd(name, new PageDataItem(name, msg));
        }

        public PageDataItem Add(string name, Array array) {
            return inAdd(name, new PageDataItem(name, array));
        }
        public PageDataItem Add(string name, Xy.Data.IDataModelDisplay XyDataModel) {
            return inAdd(name, new PageDataItem(name, XyDataModel.GetXml()));
        }

        public void AddSplitedXyDataModel(string name, Xy.Data.IDataModelDisplay XyDataModel) {
            string[] _attrs = XyDataModel.GetAttributesName();
            for (int i = 0; i < _attrs.Length; i++) {
                string _attrName = _attrs[i];
                inAdd(name + "." + _attrName, new PageDataItem(name + "." + _attrName, Convert.ToString(XyDataModel.GetAttributesValue(_attrName))));
            }
        }

        public bool HasKey(string name) {
            return _dataList.ContainsKey(name);
        }

        public bool TryGetValue(string name, out PageDataItem data) {
            if (string.IsNullOrEmpty(name)) { data = null; return false; }
            return _dataList.TryGetValue(name, out data);
        }

        private PageDataItem inAdd(string name, PageDataItem instance) {
            if (!_dataList.ContainsKey(name)) {
                _dataList.Add(name, instance);
            } else {
                _dataList[name] = instance;
            }
            return instance;
        }

        internal System.Xml.XPath.XPathDocument GetXmlData(string name) {
            return this[name].GetDataXml();
        }

        internal System.Data.DataTable GetDataTable(string name) {
            return this[name].GetDataTable();
        }

        internal string GetDataString(string name) {
            return this[name].GetDataString();
        }

        public IEnumerator GetEnumerator() {
            return _dataList.GetEnumerator();
        }
    }
}
