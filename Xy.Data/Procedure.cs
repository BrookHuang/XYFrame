using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Data {
    public delegate void beforeInvokeHandler(Procedure procedure, DataBase DB);
    public delegate void afterInvokeHandler(ProcedureResult result, Procedure procedure, DataBase DB);

    public class ProcedureResult {
        public object IntResult { get; set; }
        public System.Data.DataTable DataResult { get; set; }
    }

    public class Procedure {
        public event beforeInvokeHandler BeforeInvoke;
        public event afterInvokeHandler AfterInvoke;
        
        private List<ProcedureParameter> _value = new List<ProcedureParameter>();
        private string _name;
        private DataBase _defaultDB = null;
        private string _command;
        private bool _hasReturn = false;

        public string Name { get { return _name; } }
        public string Command { get { return _command; } }
        public bool HasParameter { get { return _value.Count > 0; } }
        public bool HasReturnParameter { get { return _hasReturn; } }
        public List<ProcedureParameter> Parameters { get { return _value; } }

        private Procedure() { }
        public Procedure(string inName) : this(inName, string.Empty, null) { }
        public Procedure(string inName, string inCommand) : this(inName, inCommand, null) { }
        public Procedure(string inName, params ProcedureParameter[] inParamlist) : this(inName, string.Empty, inParamlist) { }
        public Procedure(string inName, string inCommand, params ProcedureParameter[] inParamlist) {
            if (string.IsNullOrEmpty(inName)) throw new ArgumentNullException("inName", "can not building a procedure without Name");
            _name = inName;
            _command = inCommand;
            if (inParamlist != null) {
                if (_value == null) _value = new List<ProcedureParameter>();
                _value.AddRange(inParamlist);
            }
        }

        public void SetDataBase(DataBase inDB) {
            _defaultDB = inDB;
        }

        public Procedure Clone() {
            Procedure _temp = new Procedure() { 
                _name = _name,
                _defaultDB = _defaultDB,
                _command = _command,
                _hasReturn = _hasReturn,
                BeforeInvoke = BeforeInvoke,
                AfterInvoke = AfterInvoke
            };
            for (int i = 0; i < _value.Count; i++) {
                _temp._value.Add(_value[i].Clone());
            }
            return _temp;
        }

        public void Fill(System.Collections.Specialized.NameValueCollection NVC) {
            foreach (ProcedureParameter _item in _value) {
                if (!string.IsNullOrEmpty(NVC[_item.Name])) {
                    _item.Value = NVC[_item.Name];
                }
            }
        }

        /// <summary>
        /// 往存储过程中注入参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DbType">参数类型</param>
        public void AddItem(string Name, System.Data.DbType DbType) {
            AddItem(Name, DbType, string.Empty, System.Data.ParameterDirection.Input);
        }

        /// <summary>
        /// 往存储过程中注入参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="IsOut">参数是否有输出</param>
        public void AddItem(string Name, System.Data.DbType DbType, System.Data.ParameterDirection Direction) {
            AddItem(Name, DbType, string.Empty, Direction);
        }

        /// <summary>
        /// 往存储过程中注入参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="IsOut">验证字符串</param>
        public void AddItem(string Name, System.Data.DbType DbType, string Validate) {
            AddItem(Name, DbType, Validate, System.Data.ParameterDirection.Input);
        }

        /// <summary>
        /// 往存储过程中注入参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="IsOut">参数是否有输出</param>
        public void AddItem(string Name, System.Data.DbType DbType, string Validate, System.Data.ParameterDirection Direction) {
            AddItem(new ProcedureParameter(Name, DbType, Validate) { Direction = Direction });
        }


        /// <summary>
        /// 往存储过程中注入参数
        /// </summary>
        /// <param name="Parameter">参数item</param>
        public void AddItem(ProcedureParameter Parameter) {
            if (Parameter.Direction == System.Data.ParameterDirection.ReturnValue) _hasReturn = true;
            _value.Add(Parameter);
        }

        /// <summary>
        /// 往存储过程中注入参数
        /// </summary>
        /// <param name="paramlist">参数item</param>
        public void AddItem(params ProcedureParameter[] paramlist) {
            foreach (ProcedureParameter _item in paramlist) {
                AddItem(_item);
            }
        }

        public void SetItem(string Name, object value) {
            foreach (ProcedureParameter _item in _value) {
                if (string.Compare(_item.Name, Name) == 0) {
                    _item.Value = value;
                }
            }
        }

        public object GetItem(string Name) {
            foreach (ProcedureParameter _item in _value) {
                if (string.Compare(_item.Name, Name) == 0) {
                    return _item.Value;
                }
            }
            return null;
        }

        public ProcedureParameter GetItemInstance(string Name) {
            foreach (ProcedureParameter _item in _value) {
                if (string.Compare(_item.Name, Name) == 0) {
                    return _item;
                }
            }
            return null;
        }

        public IEnumerator<ProcedureParameter> GetEnumerator() {
            return _value.GetEnumerator();
        }

        /// <summary>
        /// 执行存储过程,并返回受影响的行数
        /// </summary>
        /// <param name="ConnectionName">连接字符串名</param>
        /// <param name="Procedure">存储过程</param>
        /// <returns>受影响的行数</returns>
        public int InvokeProcedure() {
            if (_defaultDB == null) _defaultDB = new DataBase();
            return InvokeProcedure(_defaultDB);
        }

        /// <summary>
        /// 执行存储过程,并返回受影响的行数
        /// </summary>
        /// <param name="ConnectionName">连接字符串名</param>
        /// <param name="Procedure">存储过程</param>
        /// <returns>受影响的行数</returns>
        public int InvokeProcedure(string ConnectionName) {
            return InvokeProcedure(new DataBase(ConnectionName));
        }

        public int InvokeProcedure(DataBase DB) {
            if (DB == null) {
                if (_defaultDB == null) _defaultDB = new DataBase();
                DB = _defaultDB;
            }
            if (BeforeInvoke != null) BeforeInvoke(this, DB);
            ProcedureResult result = new ProcedureResult {
                IntResult = DB.InvokeProcedure(this)
            };
            if (AfterInvoke != null) AfterInvoke(result, this, DB);
            return (int)result.IntResult;
        }

        /// <summary>
        /// 执行存储过程,并返回存储过程返回的数值
        /// </summary>
        /// <param name="Procedure">存储过程</param>
        /// <returns>存储过程返回的值</returns>
        public object InvokeProcedureResult() {
            if (_defaultDB == null) _defaultDB = new DataBase();
            return InvokeProcedureResult(_defaultDB);
        }

        /// <summary>
        /// 执行存储过程,并返回存储过程返回的数值
        /// </summary>
        /// <param name="ConnectionName">连接字符串名</param>
        /// <param name="Procedure">存储过程</param>
        /// <returns>存储过程返回的值</returns>
        public object InvokeProcedureResult(string ConnectionName) {
            return InvokeProcedureResult(new DataBase(ConnectionName));
        }

        public object InvokeProcedureResult(DataBase DB) {
            if (DB == null) {
                if (_defaultDB == null) _defaultDB = new DataBase();
                DB = _defaultDB;
            }
            if (BeforeInvoke != null) BeforeInvoke(this, DB);
            ProcedureResult result = new ProcedureResult {
                IntResult = DB.InvokeProcedureResult(this)
            };
            if (AfterInvoke != null) AfterInvoke(result, this, DB);
            return result.IntResult;
        }

        /// <summary>
        /// 执行存储过程,并返回存储过程返回的数值
        /// </summary>
        /// <param name="Procedure">存储过程</param>
        /// <returns>存储过程返回的值</returns>
        public System.Data.DataTable InvokeProcedureFill() {
            if (_defaultDB == null) _defaultDB = new DataBase();
            return InvokeProcedureFill(_defaultDB);
        }

        /// <summary>
        /// 执行存储过程,并返回存储过程返回的数值
        /// </summary>
        /// <param name="ConnectionName">连接字符串名</param>
        /// <param name="Procedure">存储过程</param>
        /// <returns>存储过程返回的值</returns>
        public System.Data.DataTable InvokeProcedureFill(string ConnectionName) {
            return InvokeProcedureFill(new DataBase(ConnectionName));
        }

        public System.Data.DataTable InvokeProcedureFill(DataBase DB) {
            if (BeforeInvoke != null) BeforeInvoke(this, DB);
            if (DB == null) {
                if (_defaultDB == null) _defaultDB = new DataBase();
                DB = _defaultDB;
            }
            ProcedureResult result = new ProcedureResult {
                DataResult = DB.InvokeProcedureFill(this)
            };
            if (AfterInvoke != null) AfterInvoke(result, this ,DB);
            return result.DataResult;
        }
    }
}
