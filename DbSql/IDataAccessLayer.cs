using System;
using System.Collections.Generic;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace Stk.DbSql
{
    public interface IDataAccessLayer
    {

        DataSet GetDataSet(string sql);

        DataSet GetDataSet(string sql, List<FbParameter> parms,
                                           CommandType commandType = CommandType.Text);

        string FbExecuteScalar(String sql);
        string FbExecuteScalar(string sql, List<FbParameter> parms);
        int FbExecuteNonQuery(TransactionSetCollection ts);
        int FbExecuteNonQuery(String sql);
        int FbExecuteNonQuery(string sql, List<FbParameter> parms, CommandType commandType = CommandType.Text);

        /// <summary>
        ///     Retruen  FbDataReader  You must Custom Close Data By Db.CloseFbData()
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        FbDataReader FbExecuteReader(string sql, List<FbParameter> parms,
                                                     CommandType commandType = CommandType.Text);
        void OpenFbData();
        void CloseFbData();
       //void DisposeAll();
        void Dispose();

        void BeginTransaction();
        void BeginTransaction(IsolationLevel iso);
        void CommitTransaction();
        void RollBackTransaction();




    }
}