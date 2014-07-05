using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Data {
    public interface IDataModel {
        void Fill(System.Data.DataRow inRow);
        Procedure FillProcedure(Procedure inItem);
    }

    public interface IDataModelDisplay {
        string[] GetAttributesName();
        object GetAttributesValue(string inName);
        System.Xml.XPath.XPathDocument GetXml();
    }

    public interface IDataBase {

        int InvokeProcedure(Procedure inProcedure);

        object InvokeProcedureResult(Procedure inProcedure);

        System.Data.DataTable InvokeProcedureFill(Procedure inProcedure);
        System.Data.DataTable InvokeProcedureFill(Procedure inProcedure, out int? result);

        void Init(Xy.DataSetting.DataSettingItem Item);
        void StartTransaction();
        void CommitTransaction();
        void RollbackTransation();
        void Open();
        void Close();
        string ErrorString { get; }
        bool IsInTransaction { get; }
    }
}