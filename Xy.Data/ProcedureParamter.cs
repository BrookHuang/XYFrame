using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Xy.Data {
    /// <summary>
    /// 利用此类可以生成一个存储过程的参数
    /// </summary>
    public class ProcedureParameter {
        private string _name;
        private object _value;
        private DbType _type;
        private string _validate = string.Empty;
        private System.Data.ParameterDirection _direction = ParameterDirection.Input;
        private static System.Text.RegularExpressions.Regex _reg;
        private System.Text.RegularExpressions.Regex _test_reg = null;

        static ProcedureParameter() {
            _reg = new System.Text.RegularExpressions.Regex(@"((?<type>[LENRIU])\((?<test>.*)\))+", System.Text.RegularExpressions.RegexOptions.Compiled);
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name {
            get { return _name; }
        }

        public DbType Type {
            get { return _type; }
        }

        public string Validate {
            get { return _validate; }
            internal set { _validate = value; }
        }

        /// <summary>
        /// 是否可输出参数
        /// </summary>
        //public bool IsOut {
        //    get { return _isOut; }
        //    set { _isOut = value; }
        //}
        public System.Data.ParameterDirection Direction {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value {
            get { return _value; }
            set { _value = value; if (!validateValue()) throw new ArgumentException("this value can not match validation rule", "value"); }
        }

        private ProcedureParameter() { }
        public ProcedureParameter(string name) : this(name, DbType.Object, string.Empty, null) { }
        public ProcedureParameter(string name, DbType type) : this(name, type, string.Empty, null) { }
        public ProcedureParameter(string name, DbType type, string validate) : this(name, type, validate, null) { }
        public ProcedureParameter(string name, DbType type, string validate, object value) {
            _name = name;
            _type = type;
            if (!string.IsNullOrEmpty(validate)) {
                _validate = validate;
                if (_value != null) {
                    if (!validateValue()) throw new ArgumentException("this value can not match validation rule", "value");
                }
            }
            _value = value;
        }

        private bool validateValue() {
            if (!string.IsNullOrEmpty(_validate)) {
                foreach (string _text in _validate.Split(new string[] { "|&|" }, StringSplitOptions.RemoveEmptyEntries)) {
                    System.Text.RegularExpressions.MatchCollection _matchs = _reg.Matches(_text);
                    foreach (System.Text.RegularExpressions.Match _match in _matchs) {
                        string _type = _match.Groups["type"].Value;
                        string _test = _match.Groups["test"].Value;
                        string _mintxt, _maxtxt;
                        short _min, _max;
                        switch (_type) {
                            case "R": // match to regex
                                if (_test_reg == null) _test_reg = new System.Text.RegularExpressions.Regex(_test, System.Text.RegularExpressions.RegexOptions.Compiled);
                                if (_test_reg.IsMatch((string)_value)) return false;
                                break;
                            case "UR":// un-match to regex
                                if (_test_reg == null) _test_reg = new System.Text.RegularExpressions.Regex(_test, System.Text.RegularExpressions.RegexOptions.Compiled);
                                if (!_test_reg.IsMatch((string)_value)) return false;
                                break;
                            case "I":// include a string
                                if (_value.ToString().IndexOf(_test) < 0) return false;
                                break;
                            case "UI":
                                if (_value.ToString().IndexOf(_test) >= 0) return false;
                                break;
                            case "N":// number range
                                _mintxt = _test.Split('-')[0];
                                _maxtxt = _test.Split('-')[1];
                                if (short.TryParse(_mintxt, out _min)) if ((short)_value < _min) return false;
                                if (short.TryParse(_maxtxt, out _max)) if ((short)_value > _max) return false;
                                break;
                            case "L":// length range
                                _mintxt = _test.Split('-')[0];
                                _maxtxt = _test.Split('-')[1];
                                if (short.TryParse(_mintxt, out _min)) if (((string)_value).Length < _min) return false;
                                if (short.TryParse(_maxtxt, out _max)) if (((string)_value).Length > _max) return false;
                                break;
                            case "S":// special validate
                                switch (_test) {
                                    case "not empty":
                                        if (string.IsNullOrEmpty(_value.ToString())) return false;
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            return true;
        }

        public ProcedureParameter Clone() {
            ProcedureParameter _temp = new ProcedureParameter() {
                _name = _name,
                _type = _type,
                _value = _value,
                _direction = _direction,
                _validate = _validate,
                _test_reg = _test_reg
            };
            return _temp;
        }
    }
}
