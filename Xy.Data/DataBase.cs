using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Data {
    public sealed class DataBase {
        IDataBase _db;

        public DataBase()
            : this(Xy.DataSetting.DataSettingCollection.DEFAULTCONNECTIONNAME) {
        }

        public DataBase(string ConnectionName) : this(Xy.DataSetting.DataSettingCollection.GetDataBaseItem(ConnectionName)) { }

        public DataBase(DataSetting.DataSettingItem DataBaseItem) {
            _db = Runtime.DataProvideLibrary.Get(DataBaseItem.DataProvider);
            _db.Init(DataBaseItem);
        }

        public string ErrorString {
            get { return _db.ErrorString; }
        }

        public bool IsInTransaction {
            get { return _db.IsInTransaction; }
        }

        /// <summary>
        /// 执行存储过程,并返回受影响的行数
        /// </summary>
        /// <param name="ProcedureName">存储过程名</param>
        /// <param name="Structure">参数结构</param>
        /// <returns>受影响的行数</returns>
        public int InvokeProcedure(Procedure Structure) {
            return _db.InvokeProcedure(Structure);
        }

        /// <summary>
        /// 执行存储过程,并返回存储过程返回的数值
        /// </summary>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="Structure">参数结构</param>
        /// <returns>存储过程返回的值</returns>
        public object InvokeProcedureResult(Procedure Structure) {
            return _db.InvokeProcedureResult(Structure);
        }

        /// <summary>
        /// 执行存储过程,并填充一个DataSet对象
        /// </summary>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="Structure">参数结构</param>
        /// <returns>DataSet对象</returns>
        public System.Data.DataTable InvokeProcedureFill(Procedure Structure) {
            return _db.InvokeProcedureFill(Structure);
        }

        /// <summary>
        /// 执行查询语句,并返回受影响的行数
        /// </summary>
        /// <param name="ProcedureName">存储过程名</param>
        /// <param name="Structure">参数结构</param>
        /// <returns>受影响的行数</returns>
        //public int InvokeCommand(string Command) {
        //    return _db.InvokeCommand(Command);
        //}

        /// <summary>
        /// 执行查询语句,并填充一个DataSet对象
        /// </summary>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="Structure">参数结构</param>
        /// <returns>DataSet对象</returns>
        //public System.Data.DataTable InvokeCommandFill(string Command) {
        //    return _db.InvokeCommandFill(Command);
        //}

        public void StartTransaction() {
            _db.StartTransaction();
        }

        public void CommitTransaction() {
            _db.CommitTransaction();
        }

        public void RollbackTransation() {
            _db.RollbackTransation();
        }

        public void Open() {
            _db.Open();
        }

        public void Close() {
            _db.Close();
        }
    }
}
