using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Tools.Debug {
    public static class Decompose {
        public static ReflectionCollection DecomposeObject(object obj, bool publicOnly = true, bool declaredOnly = true) {
            Type _objType = obj.GetType();
            //GetFields可获取变量
            //GetMembers可获取属性包括属性的
            System.Reflection.BindingFlags _filter = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static;
            if (!publicOnly) {
                _filter = _filter | System.Reflection.BindingFlags.NonPublic;
            }
            if (!declaredOnly) {
                _filter = _filter | System.Reflection.BindingFlags.DeclaredOnly;
            }
            System.Reflection.MemberInfo[] _members = _objType.GetMembers(_filter);
            ReflectionCollection _entitys = new ReflectionCollection();
            foreach (System.Reflection.MemberInfo _item in _members) {
                ReflectionEntity _entity = new ReflectionEntity();
                _entity.Name = _item.Name;
                _entity.MemberType = _item.MemberType;
                object valueObj;
                if (_item.MemberType == System.Reflection.MemberTypes.Property) {
                    System.Reflection.PropertyInfo _propertyInfo = ((System.Reflection.PropertyInfo)_item);
                    try {
                        valueObj = _propertyInfo.GetValue(obj, null);
                        if (valueObj != null) {
                            _entity.Value = valueObj;
                        } else {
                            _entity.Value = "NULL";
                        }
                    } catch (Exception ex) {
                        _entity.Value = "error:" + ex.Message;
                    }
                    _entity.Type = _propertyInfo.PropertyType;
                }
                if (_item.MemberType == System.Reflection.MemberTypes.Field) {
                    System.Reflection.FieldInfo _fieldInfo = ((System.Reflection.FieldInfo)_item);
                    valueObj = _fieldInfo.GetValue(obj);
                    try {
                        if (valueObj != null) {
                            _entity.Value = valueObj;
                        } else {
                            _entity.Value = "NULL";
                        }
                    } catch (Exception ex) {
                        _entity.Value = "error:" + ex.Message;
                    }
                    _entity.Type = _fieldInfo.FieldType;
                }
                _entitys.Add(_entity);
            }
            return _entitys;
        }

        public class ReflectionEntity {
            public string Name { get; set; }
            public System.Reflection.MemberTypes MemberType { get; set; }
            public Type Type { get; set; }
            public object Value { get; set; }
            public override string ToString() {
                switch (this.MemberType) {
                    case System.Reflection.MemberTypes.Constructor:
                    case System.Reflection.MemberTypes.Custom:
                    case System.Reflection.MemberTypes.Event:
                    case System.Reflection.MemberTypes.Method:
                    case System.Reflection.MemberTypes.NestedType:
                    case System.Reflection.MemberTypes.TypeInfo:
                    case System.Reflection.MemberTypes.All:
                        return string.Format("{0}\t{1}\t{2}", this.Name, this.MemberType, this.Type);
                    case System.Reflection.MemberTypes.Field:
                    case System.Reflection.MemberTypes.Property:
                        return string.Format("{0}\t{1}\t{2}\t{3}", this.Name, this.MemberType, this.Type, this.Value);
                }
                return string.Empty;
            }
        }

        public class ReflectionCollection : List<ReflectionEntity> {
            public System.Data.DataTable toTable() {
                System.Data.DataTable _table = new System.Data.DataTable();
                _table.Columns.Add("Name");
                _table.Columns.Add("MemberType");
                _table.Columns.Add("Type");
                _table.Columns.Add("Value");
                for (int i = 0; i < this.Count; i++) {
                    ReflectionEntity _entity = this[i];
                    System.Data.DataRow _row = _table.NewRow();
                    _row["Name"] = _entity.Name;
                    _row["MemberType"] = _entity.MemberType;
                    _row["Type"] = _entity.Type;
                    _row["Value"] = _entity.Value;
                    _table.Rows.Add(_row);
                }
                return _table;
            }

            public string ToHTMLTable() {
                System.Data.DataTable _table = this.toTable();
                StringBuilder _sb = new StringBuilder();
                _sb.Append("<table>");
                _sb.Append("<thead>");
                _sb.Append("<tr>");
                for (int i = 0; i < _table.Columns.Count; i++) {
                    _sb.Append("<th>");
                    _sb.Append(_table.Columns[i].Caption);
                    _sb.Append("</th>");
                }
                _sb.Append("</tr>");
                _sb.Append("</thead>");
                _sb.Append("<tbody>");
                for (int i = 0; i < _table.Rows.Count; i++) {
                    System.Data.DataRow _row = _table.Rows[i];
                    _sb.Append("<tr>");
                    for (int j = 0; j < _table.Columns.Count; j++) {
                        _sb.Append("<td>");
                        _sb.Append(_row[j]);
                        _sb.Append("</td>");
                    }
                    _sb.Append("</tr>");
                }
                _sb.Append("</tbody>");
                _sb.Append("</table>");
                return _sb.ToString();
            }

            public override string ToString() {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < this.Count; i++) {
                    ReflectionEntity _entity = this[i];
                    sb.AppendFormat("{0}\t{1}\t{2}\t{3}" + Environment.NewLine, _entity.Name, _entity.MemberType, _entity.Type, _entity.Value);
                }
                return sb.ToString();
            }
        }
    }
}
