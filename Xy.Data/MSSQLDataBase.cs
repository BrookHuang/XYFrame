using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Xy.Data {
    public class MSSQLDataBase : IDataBase {
        private SqlConnection _con = null;
        private StringBuilder _errorString;
        private SqlTransaction _trans = null;
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
            _con = new SqlConnection(_dbi.ConnectionString);
        }

        public void StartTransaction(){
            if(_trans == null){
                if(_con.State == ConnectionState.Closed || _con.State == ConnectionState.Broken){
                    throw new TransException("It is not an available connection");
                }else{
                    _trans = _con.BeginTransaction();
                }
            }else{
                throw new TransException("Already exist a transaction");
            }
                
        }

        public void CommitTransaction(){
            if (_trans != null) {
                _trans.Commit();
                _trans.Dispose();
                _trans = null;
            } else
                throw new TransException("Didn't have transaction to do it");
        }

        public void RollbackTransation(){
            if (_trans != null) {
                _trans.Rollback();
                _trans.Dispose();
                _trans = null;
            } else
                throw new TransException("Didn't have transaction to do it");
        }

        public void Open() {
            if(_con.State != ConnectionState.Open) _con.Open();
        }

        public void Close() {
            _con.Close();
        }

        //public int InvokeCommand(string Command) {
        //    SqlCommand _command = new SqlCommand(Command, _con);
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
        //    SqlCommand _command = new SqlCommand(Command, _con);
        //    _command.CommandType = CommandType.Text;
        //    DataSet _dataSet = new DataSet();
        //    SqlDataAdapter _dataAdapter = new SqlDataAdapter(_command);
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
            SqlCommand _command;
            if (string.IsNullOrEmpty(inProcedure.Command)) {
                _command = new SqlCommand(inProcedure.Name, _con);
                _command.CommandType = CommandType.StoredProcedure;
            } else {
                _command = new SqlCommand(inProcedure.Command, _con);
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
            SqlCommand _command;
            if (string.IsNullOrEmpty(inProcedure.Command)) {
                _command = new SqlCommand(inProcedure.Name, _con);
                _command.CommandType = CommandType.StoredProcedure;
            } else {
                _command = new SqlCommand(inProcedure.Command, _con);
                _command.CommandType = CommandType.Text;
            }
            _command.CommandTimeout = _dbi.Timeout;
            if (inProcedure.HasParameter) {
                AddParameters(_command, inProcedure);
            }
            object _result = null;
            if(!inProcedure.HasReturnParameter)
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
            return InvokeProcedureFill(inProcedure,out result);
        }
        public DataTable InvokeProcedureFill(Procedure inProcedure,out int? result) {
            SqlCommand _command;
            if (string.IsNullOrEmpty(inProcedure.Command)) {
                _command = new SqlCommand(inProcedure.Name, _con);
                _command.CommandType = CommandType.StoredProcedure;
            } else {
                _command = new SqlCommand(inProcedure.Command, _con);
                _command.CommandType = CommandType.Text;
            }
            _command.CommandTimeout = _dbi.Timeout;
            if (inProcedure.HasParameter) {
                AddParameters(_command, inProcedure);
            }
            DataSet _dataSet = new DataSet();
            SqlDataAdapter _dataAdapter = new SqlDataAdapter(_command);
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

        public DataSet InvokeProcedureFillSet(Procedure inProcedure) {
            int? result;
            return InvokeProcedureFillSet(inProcedure, out result);
        }
        public DataSet InvokeProcedureFillSet(Procedure inProcedure, out int? result) {
            SqlCommand _command;
            if (string.IsNullOrEmpty(inProcedure.Command)) {
                _command = new SqlCommand(inProcedure.Name, _con);
                _command.CommandType = CommandType.StoredProcedure;
            } else {
                _command = new SqlCommand(inProcedure.Command, _con);
                _command.CommandType = CommandType.Text;
            }
            _command.CommandTimeout = _dbi.Timeout;
            if (inProcedure.HasParameter) {
                AddParameters(_command, inProcedure);
            }
            DataSet _dataSet = new DataSet();
            SqlDataAdapter _dataAdapter = new SqlDataAdapter(_command);
            try {
                if (_trans != null) _command.Transaction = _trans;
                _dataAdapter.Fill(_dataSet);
            } catch (Exception ex) {
                ErrorString = "在存储过程中发生了错误:" + ex.Message;
                throw ex;
            }
            result = (int?)getOutputParameters(_command, inProcedure);
            return _dataSet;
        }

        private void AddParameters(SqlCommand Command, Procedure Structure) {
            foreach (ProcedureParameter _ProcedureParameter in Structure) {
                System.Data.SqlClient.SqlParameter _temp = new SqlParameter(_ProcedureParameter.Name, _ProcedureParameter.Type);
                _temp.Value = _ProcedureParameter.Value;
                _temp.Direction = _ProcedureParameter.Direction;
                Command.Parameters.Add(_temp);
            }
        }

        private object getOutputParameters(SqlCommand Command, Procedure Structure) {
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
    }
}
