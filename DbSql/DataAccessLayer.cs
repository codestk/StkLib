using System;
using System.Collections.Generic;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;

namespace Stk.DbSql
{
    public class DataAccessLayer : IDataAccessLayer, IDisposable
    {


       
        private int _CommandTimeout = 120;
        private FbCommand _com;
        private FbConnection _con;
        private readonly string _constr;
        private FbDataAdapter _adapter;
        private Exception _gloEx;


        private static FbConnection _conTransaction; //Share For Transaction
        private static FbTransaction _tran;//Share For Transaction

        public DataAccessLayer(string connectionString)
        {
            //_con = null;
            _com = null;
            _gloEx = null;
            _adapter = null;
            //_constr = "";

            _constr = connectionString;


            
            //Set innit Command name Time Out
            InnitCommandTimeOut();
        }

        #region DataSet

        private void InnitCommandTimeOut()
        {
            if (ConfigurationManager.AppSettings["CommandTimeOut"] != null)
            {
                _CommandTimeout = (Convert.ToInt32( ConfigurationManager.AppSettings["CommandTimeOut"].ToString()));
            }
                 
        }


        //Get DataSet 
        public DataSet GetDataSet(string sql)
        {
            // Description: fill DataSet via OleDbDataAdapter, connect Firebird, Interbase
            using (var ds = new DataSet())
            {
                try
                {
                    OpenFbData();
                    //using (_com = GetCommandDb(sql))
                    //{
                    //    _adapter = GetAdapterDb(_com);  
                    //    _adapter.Fill(ds);
                    //}

                    //Using not work with transaction
                    _com = GetCommandDb(sql);
                    _adapter = GetAdapterDb(_com);
                    _adapter.Fill(ds);

                }
                catch (Exception ex)
                {
                    _gloEx = ex;
                }
                finally
                {
                    CloseFbData();
                    DisposeAll();
                }
                if (_gloEx != null)
                {
                    throw _gloEx;
                }

                return ds;
            }
        }


        //Get DataSet 
        public virtual DataSet GetDataSet(string sql, List<FbParameter> parms,
                                          CommandType commandType = CommandType.Text)
        {
            // Description: fill DataSet via OleDbDataAdapter, connect Firebird, Interbase
            using (var ds = new DataSet())
            {
                try
                {
                    OpenFbData();

                    //using (_com = GetCommandDb(sql, commandType))
                    //{
                    //    SetParameter(parms, ref _com);
                    //    _adapter = GetAdapterDb(_com);           
                    //    _adapter.Fill(ds);
                    //}


                    _com = GetCommandDb(sql, commandType);

                    SetParameter(parms, ref _com);
                    _adapter = GetAdapterDb(_com);
                    _adapter.Fill(ds);



                }
                catch (Exception ex)
                {
                    _gloEx = ex;
                }
                finally
                {
                    CloseFbData();
                    DisposeAll();
                }

                if (_gloEx != null)
                {
                    throw _gloEx;
                }
                return ds;
            }
        }

        #endregion

        #region ExecuteScalar

        public string FbExecuteScalar(String sql)
        {
            // Description: ExecuteScalar - gets a single value. Firebird, Interbase .Net provider (c#)
            string output = null;

            //FbTransaction  trans = con.BeginTransaction();


            try
            {
                OpenFbData();
                //com = new FbCommand(sql, con);
                //using (_com = GetCommandDb(sql))
                //{
                //    output = Convert.ToString(_com.ExecuteScalar());
                //}

                //Usirng detroy value of time out
                _com = GetCommandDb(sql);
                output = Convert.ToString(_com.ExecuteScalar());

            }
            catch (Exception ex)
            {
                _gloEx = ex;
            }
            finally
            {
                CloseFbData();
                DisposeAll();
            }

            if (_gloEx != null)
            {
                throw _gloEx;
            }


            return output;
        }


        public string FbExecuteScalar(string sql, List<FbParameter> parms)
        {
            string output = "";

            try
            {
                OpenFbData();
                //using (_com = GetCommandDb(sql))
                //{
                //    SetParameter(parms, ref _com);
                //    output = _com.ExecuteScalar().ToString();
                //}

                //Using not work wit transaction
                _com = GetCommandDb(sql);

                SetParameter(parms, ref _com);
                output = _com.ExecuteScalar().ToString();


            }

            catch (Exception ex)
            {
                _gloEx = ex;
            }
            finally
            {
                CloseFbData();
                DisposeAll();
            }

            if (_gloEx != null)
            {
                throw _gloEx;
            }

            return output;
        }

        #endregion

        #region ExecuteNonQuery

        public int FbExecuteNonQuery(TransactionSetCollection ts)
        {
            int rowAffect = 0;
            try
            {
                OpenFbData();
                foreach (TransactionSet t in ts)
                {
                    //com = new FbCommand(t.SqlCommand, con);
                    //com.Transaction = trans;

                    _com = GetCommandDb(t.SqlCommand, t.ExecuteType);

                    if (t.Parameter != null)
                    {
                        SetParameter(t.Parameter, ref _com);
                    }
                    rowAffect += _com.ExecuteNonQuery();
                }
                //trans.Commit();
            }
            catch (Exception ex)
            {
                // trans.Rollback();
                _gloEx = ex;
                rowAffect = 0;
            }
            finally
            {
                CloseFbData();
                DisposeAll();
            }


            if (_gloEx != null)
            {
                throw _gloEx;
            }

            return rowAffect;
        }

        public int FbExecuteNonQuery(String sql)
        {
            // Description: ExecuteScalar - gets a single value. Firebird, Interbase .Net provider (c#)

            int affectRow = 0;

            try
            {
                OpenFbData();
                //using (_com = GetCommandDb(sql))
                //{
                //    affectRow = _com.ExecuteNonQuery();
                //}


                _com = GetCommandDb(sql);

                affectRow = _com.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                _gloEx = ex;
            }
            finally
            {
                CloseFbData();
                DisposeAll();
            }

            if (_gloEx != null)
            {
                throw _gloEx;
            }

            return affectRow;
        }


        public int FbExecuteNonQuery(string sql, List<FbParameter> parms, CommandType commandType = CommandType.Text)
        {
            int affectRow = 0;
            try
            {
                OpenFbData();
                //using (_com = GetCommandDb(sql, commandType))
                //{
                //    SetParameter(parms, ref _com);

                //    affectRow = _com.ExecuteNonQuery();
                //}

                _com = GetCommandDb(sql, commandType);

                SetParameter(parms, ref _com);

                affectRow = _com.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                _gloEx = ex;
            }
            finally
            {
                CloseFbData();
                DisposeAll();
            }

            if (_gloEx != null)
            {
                throw _gloEx;
            }


            //Command Can Effect one or more rows.
            return affectRow;
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        ///     Retruen  FbDataReader  You must Custom Close Data By Db.CloseFbData()
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public FbDataReader FbExecuteReader(string sql, List<FbParameter> parms,
                                            CommandType commandType = CommandType.Text)
        {
            FbDataReader fbrd = null;

            try
            {
                OpenFbData();
                //using (_com = GetCommandDb(sql, commandType))
                //{
                //    SetParameter(parms, ref _com);
                //    fbrd = _com.ExecuteReader();
                //}


                //Using not work with transaction
                _com = GetCommandDb(sql, commandType);

                SetParameter(parms, ref _com);
                fbrd = _com.ExecuteReader();

            }

            catch (Exception ex)
            {
                _gloEx = ex;
                CloseFbData();
            }

            if (_gloEx != null)
            {
                throw _gloEx;
            }

            return fbrd;
        }

        #endregion

        #region Transactions

        public virtual void BeginTransaction()
        {
            //OpenFbData();

            _tran = _con.BeginTransaction();


        }
        //Must OpenFbData
        /// <summary>
        /// 	Chaos	The pending changes from more highly isolated transactions cannot be overwritten.
        //    ReadCommitted	Shared locks are held while the data is being read to avoid dirty reads, but the data can be changed before the end of the transaction, resulting in non-repeatable reads or phantom data.
        //    ReadUncommitted	A dirty read is possible, meaning that no shared locks are issued and no exclusive locks are honored.
        //    RepeatableRead	Locks are placed on all data that is used in a query, preventing other users from updating the data. Prevents non-repeatable reads but phantom rows are still possible.
        //    Serializable	A range lock is placed on the DataSet, preventing other users from updating or inserting rows into the dataset until the transaction is complete.
        //    Snapshot	Reduces blocking by storing a version of data that one application can read while another is modifying the same data. Indicates that from one transaction you cannot see changes made in other transactions, even if you requery.
        //    Unspecified	A different isolation level than the one specified is being used, but the level cannot be determined.
        //When using OdbcTransaction, if you do not set IsolationLevel or you set IsolationLevel to Unspecified, the transaction executes according to the isolation level that is determined by the driver that is being used.
        //        /// </summary>
        /// <param name="isolevel"></param>
        public virtual void BeginTransaction(IsolationLevel isolevel)
        {
            //OpenFbData();

            _tran = _con.BeginTransaction(isolevel);


        }

        public virtual void CommitTransaction()
        {
            _tran.Commit();
            _tran = null;
        }


        public virtual void RollBackTransaction()
        {
            _tran.Rollback();
            _tran = null;
        }

        #endregion



        public virtual void OpenFbData()
        {
            if (_tran != null)
            {
                _con = _conTransaction; //Set OldTra Saction

                return;
            }

            //Check Null
            if (_con == null)
            {
                _con = new FbConnection(_constr);

                _conTransaction = _con; // Save Connect for Transaction


            }


            if (_con.State == ConnectionState.Closed)
            {
                _con.Open();
            }

            //return con;
        }


        public virtual void CloseFbData()
        {
            //in case Open Tran Section
            if (_tran != null)
            {
                return;
            }

            if (_con.State == ConnectionState.Open)
            {
                _con.Close();// Close  _conTransaction

            }


        }


        private void DisposeAll()
        {
            //Prevent Dispos if have transaction
            if (_tran != null && _tran.Connection != null)
            {
                return;
            }

            if (_tran != null)
            {
                _tran.Dispose();
                _tran = null;
            }


            if (_com != null)
            {
                _com.Dispose();
                _com = null;
            }

            if (_con != null)
            {
                _con.Dispose();
                _con = null;
            }


            if (_adapter != null)
            {
                _adapter.Dispose();
                _adapter = null;
            }
        }


        private FbCommand GetCommandDb(string sql, CommandType commandType = CommandType.Text
            )
        {
            if (_com == null)
            {
                //_com = new FbCommand(sql, _con);
                _com = new FbCommand();
            }

            //_com.Connection = _con;
            if (_tran != null)
            {
                _com.Transaction = _tran;
            }

            _com.Connection = _con;
            _com.CommandText = sql;
            _com.CommandType = commandType;
            _com.CommandTimeout = _CommandTimeout;
            return _com;
        }


        private FbDataAdapter GetAdapterDb(FbCommand com)
        {
            if (_adapter == null)
            {
                _adapter = new FbDataAdapter();
            }

            _adapter.SelectCommand = com;
            return _adapter;
        }


        private void SetParameter(IEnumerable<FbParameter> parms, ref FbCommand com)
        {
            com.Parameters.Clear();
            foreach (FbParameter p in parms)
            {
                com.Parameters.Add(p);
            }
        }

        public void Dispose()
        {
            DisposeAll();
        }
    }
}