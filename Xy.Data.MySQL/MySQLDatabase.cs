using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace Xy.Data {
    public class MySQLDatabase : Xy.Data.IDataBase {

        private MySqlConnection _con = null;
        private StringBuilder _errorString;
        private MySqlTransaction _trans = null;
        private Xy.DataSetting.DataSettingItem _dbi = null;

        public string ErrorString {
            get {
                if (_errorString != null) {
                    return _errorString.ToString();
                } else {
                    return null;
                }
            }
            private set {
                if (_errorString == null) {
                    _errorString = new StringBuilder();
                }
                _errorString.AppendLine(value);
            }
        }

        public bool IsInTransaction {
            get { return _trans != null; }
        }

        public void Init(Xy.DataSetting.DataSettingItem Item) {
            _dbi = Item;
            _con = new MySqlConnection(_dbi.ConnectionString);
        }

        public void StartTransaction() {
            if (_trans == null) {
                if (_con.State == ConnectionState.Closed || _con.State == ConnectionState.Broken) {
                    throw new TransException("it is not a useable connection");
                } else {
                    _trans = _con.BeginTransaction();
                }
            } else {
                throw new TransException("already exist a transaction");
            }

        }

        public void CommitTransaction() {
            if (_trans != null) {
                _trans.Commit();
                _trans.Dispose();
                _trans = null;
            } else
                throw new TransException("didn't have transaction to do it");
        }

        public void RollbackTransation() {
            if (_trans != null) {
                _trans.Rollback();
                _trans.Dispose();
                _trans = null;
            } else
                throw new TransException("didn't have transaction to do it");
        }

        public void Open() {
            if (_con.State != ConnectionState.Open) _con.Open();
        }

        public void Close() {
            _con.Close();
        }

        //public int InvokeCommand(string Command) {
        //    MySqlCommand _command = new MySqlCommand(Command, _con);
        //    _command.CommandType = CommandType.Text;
        //    this.Open();
        //    int editRow = 0;
        //    try {
        //        if (_trans != null) _command.Transaction = _trans;
        //        editRow = _command.ExecuteNonQuery();
        //    } catch (Exception ex) {
        //        ErrorString = ex.Message;
        //        throw ex;
        //    } finally {
        //        if (_trans == null) _con.Close();
        //    }
        //    return editRow;
        //}

        //public DataTable InvokeCommandFill(string Command) {
        //    MySqlCommand _command = new MySqlCommand(Command, _con);
        //    _command.CommandType = CommandType.Text;
        //    DataSet _dataSet = new DataSet();
        //    MySqlDataAdapter _dataAdapter = new MySqlDataAdapter(_command);
        //    try {
        //        if (_trans != null) _command.Transaction = _trans;
        //        _dataAdapter.Fill(_dataSet);
        //    } catch (Exception ex) {
        //        ErrorString = "在存储过程中发生了错误:" + ex.Message;
        //        throw ex;
        //    }
        //    if (_dataSet.Tables.Count > 0) {
        //        return _dataSet.Tables[0];
        //    } else {
        //        return new DataTable();
        //    }
        //}

        public int InvokeProcedure(Procedure inProcedure) {
            MySqlCommand _command;
            if (string.IsNullOrEmpty(inProcedure.Command)) {
                _command = new MySqlCommand(inProcedure.Name, _con);
                _command.CommandType = CommandType.StoredProcedure;
            } else {
                _command = new MySqlCommand(inProcedure.Command, _con);
                _command.CommandType = CommandType.Text;
            }
            _command.CommandTimeout = _dbi.Timeout;
            if (inProcedure.HasParameter) {
                AddParameters(_command, inProcedure);
            }
            this.Open();
            int editRow = 0;
            try {
                if (_trans != null) _command.Transaction = _trans;
                editRow = _command.ExecuteNonQuery();
            } catch (Exception ex) {
                ErrorString = ex.Message;
                throw ex;
            } finally {
                if (_trans == null) _con.Close();
            }
            getOutputParameters(_command, inProcedure);
            return editRow;
        }


        public object InvokeProcedureResult(Procedure inProcedure) {
            MySqlCommand _command;
            if (string.IsNullOrEmpty(inProcedure.Command)) {
                _command = new MySqlCommand(inProcedure.Name, _con);
                _command.CommandType = CommandType.StoredProcedure;
            } else {
                _command = new MySqlCommand(inProcedure.Command, _con);
                _command.CommandType = CommandType.Text;
            }
            _command.CommandTimeout = _dbi.Timeout;
            if (inProcedure.HasParameter) {
                AddParameters(_command, inProcedure);
            }
            object _result = null;
            if (!inProcedure.HasReturnParameter)
                _command.Parameters.AddWithValue("xy_Procedure_Return", _result).Direction = ParameterDirection.ReturnValue;
            this.Open();
            try {
                if (_trans != null) _command.Transaction = _trans;
                _command.ExecuteNonQuery();
            } catch (Exception ex) {
                ErrorString = ex.Message;
                throw ex;
            } finally {
                if (_trans == null) _con.Close();
            }
            if (inProcedure.HasReturnParameter) {
                _result = getOutputParameters(_command, inProcedure);
            } else {
                _result = _command.Parameters["xy_Procedure_Return"].Value;
            }
            if (_result != null) {
                return _result;
            } else {
                throw new Exception("要求返回数值时返回为空");
            }
        }

        public DataTable InvokeProcedureFill(Procedure inProcedure) {
            int? result;
            return InvokeProcedureFill(inProcedure, out result);
        }
        public DataTable InvokeProcedureFill(Procedure inProcedure, out int? result) {
            MySqlCommand _command;
            if (string.IsNullOrEmpty(inProcedure.Command)) {
                _command = new MySqlCommand(inProcedure.Name, _con);
                _command.CommandType = CommandType.StoredProcedure;
            } else {
                _command = new MySqlCommand(inProcedure.Command, _con);
                _command.CommandType = CommandType.Text;
            }
            _command.CommandTimeout = _dbi.Timeout;
            if (inProcedure.HasParameter) {
                AddParameters(_command, inProcedure);
            }
            DataSet _dataSet = new DataSet();
            MySqlDataAdapter _dataAdapter = new MySqlDataAdapter(_command);
            try {
                if (_trans != null) _command.Transaction = _trans;
                _dataAdapter.Fill(_dataSet);
            } catch (Exception ex) {
                ErrorString = "在存储过程中发生了错误:" + ex.Message;
                throw ex;
            }
            result = (int?)getOutputParameters(_command, inProcedure);
            if (_dataSet.Tables.Count > 0) {
                return _dataSet.Tables[0];
            } else {
                return new DataTable();
            }
        }

        private void AddParameters(MySqlCommand Command, Procedure Structure) {
            foreach (ProcedureParameter _ProcedureParameter in Structure) {
                MySqlParameter _temp = new MySqlParameter(_ProcedureParameter.Name, GetMySqlDbType(_ProcedureParameter.Type));
                _temp.Value = _ProcedureParameter.Value;
                _temp.Direction = _ProcedureParameter.Direction;
                Command.Parameters.Add(_temp);
            }
        }

        private object getOutputParameters(MySqlCommand Command, Procedure Structure) {
            object _return = null;
            foreach (ProcedureParameter _ProcedureParameter in Structure) {
                if (_ProcedureParameter.Direction != ParameterDirection.Input) {
                    _ProcedureParameter.Value = Command.Parameters[_ProcedureParameter.Name].Value;
                    if (_ProcedureParameter.Direction == ParameterDirection.ReturnValue) {
                        _return = _ProcedureParameter.Value;
                    }
                }
            }
            return _return;
        }

        private MySql.Data.MySqlClient.MySqlDbType GetMySqlDbType(System.Data.DbType _dbType) {
            switch (_dbType) {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                    return MySqlDbType.VarString;
                case DbType.Binary:
                    return MySqlDbType.Binary;
                case DbType.Boolean:
                    return MySqlDbType.Bit;
                case DbType.Byte:
                    return MySqlDbType.UByte;
                case DbType.Currency:
                case DbType.Single:
                    return MySqlDbType.Float;
                case DbType.Date:
                    return MySqlDbType.Date;
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    return MySqlDbType.DateTime;
                case DbType.Decimal:
                    return MySqlDbType.Decimal;
                case DbType.Double:
                    return MySqlDbType.Double;
                case DbType.Guid:
                    return MySqlDbType.Guid;
                case DbType.Int16:
                    return MySqlDbType.Int16;
                case DbType.Int32:
                    return MySqlDbType.Int32;
                case DbType.Int64:
                    return MySqlDbType.Int64;
                case DbType.Object:
                    return MySqlDbType.Binary;
                case DbType.SByte:
                    return MySqlDbType.Byte;
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Xml:
                    return MySqlDbType.String;
                case DbType.Time:
                    return MySqlDbType.Time;
                case DbType.UInt16:
                    return MySqlDbType.UInt16;
                case DbType.UInt32:
                    return MySqlDbType.UInt32;
                case DbType.UInt64:
                    return MySqlDbType.UInt64;
                case DbType.VarNumeric:
                    return MySqlDbType.VarBinary;
                default:
                    throw new Exception("error DB type.");
            }
        }
    }
}
