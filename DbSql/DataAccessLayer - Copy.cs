using System;
using System.Collections.Generic;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
 
namespace Stk.DbSql
{ 
    public class DataAccessLayer
    {
        //SqlConnection con;
        //FbCommand com;
        private readonly string _constr;
        private FbDataAdapter _adapter;
        private static  FbCommand _com;
        private static  FbConnection _con;
        private Exception _gloEx;
        private FbTransaction _tran;

        public DataAccessLayer(string connectionString)
        {
            _con = null;
            _com = null;
            _gloEx = null;
            _adapter = null;
            _constr = "";

            //if (((ConfigurationManager.AppSettings["ConnectionString"] == null)))
            //{
            //    throw new Exception("Connection Error");
            //}
            //_constr = ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture);
            //con = new FbConnection (constr);
            _constr = connectionString;
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
                    using (_adapter = GetAdapterDb(sql, _con))
                    {
                        _adapter.Fill(ds);
                    }
                }
                catch (Exception ex)
                {
                    _gloEx = ex;
                }
                finally
                {
                    //CloseFbData();
                    //DisposeAll();
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
                    //using (com = new FbCommand(sql, con))
                    using (_com = GetCommandDb(sql, commandType))
                    {
                        SetParameter(parms, ref _com);

                        _adapter = GetAdapterDb(_com);

                        //adapter.SelectCommand = com;
                        _adapter.Fill(ds);
                    }
                }
                catch (Exception ex)
                {
                    _gloEx = ex;
                }
                finally
                {
                    //CloseFbData();
                    //DisposeAll();
                }

                if (_gloEx != null)
                {
                    throw _gloEx;
                }
                return ds;
            }
        }


        public string FbExecuteScalar(String sql)
        {
            // Description: ExecuteScalar - gets a single value. Firebird, Interbase .Net provider (c#)
            string output = null;

            //FbTransaction  trans = con.BeginTransaction();


            try
            {
                OpenFbData();
                //com = new FbCommand(sql, con);
                using (_com = GetCommandDb(sql))
                {
                    output = Convert.ToString(_com.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                _gloEx = ex;
            }
            finally
            {
                //CloseFbData();
                //DisposeAll();
            }

            if (_gloEx != null)
            {
                throw _gloEx;
            }


            return output;
        }


        public bool FbExecuteNonQuery(String sql)
        {
            // Description: ExecuteScalar - gets a single value. Firebird, Interbase .Net provider (c#)
            bool output = false;
            int affectRow = 0;
            _gloEx = null;

            //FbTransaction trans = con.BeginTransaction();

            try
            {
                OpenFbData();
                using (_com = GetCommandDb(sql))
                {
                    affectRow = _com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _gloEx = ex;
            }
            finally
            {
                //CloseFbData();
                //DisposeAll();
            }

            if (_gloEx != null)
            {
                throw _gloEx;
            }

            //Command Can Effect one or more rows.
            if (affectRow > 0)
            {
                output = true;
            }


            return output;
        }


        public int FbExecuteNonQuery(string sql, List<FbParameter> parms, CommandType commandType = CommandType.Text)
        {
          
            int affectRow = 0;


            try
            {
                //OpenFbData();
                //com = new FbCommand(sql, con);
                //com.CommandType = commandType;
                //foreach (var p in parms)
                //{

                //    com.Parameters.Add(p);

                //}
                //OpenFbData();
                using (_com = GetCommandDb(sql, commandType))
                {
                    SetParameter(parms, ref _com);

                    affectRow = _com.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                _gloEx = ex;
            }
            finally
            {
                //CloseFbData();
                //DisposeAll();
            }

            if (_gloEx != null)
            {
                throw _gloEx;
            }


            //Command Can Effect one or more rows.
           
            return affectRow;
        }


        public string FbExecuteScalar(string sql, List<FbParameter> parms)
        {
            string output = "";


            try
            {
                OpenFbData();
                using (_com = GetCommandDb(sql))
                {
                    SetParameter(parms, ref _com);
                    output = _com.ExecuteScalar().ToString();
                }
            }

            catch (Exception ex)
            {
                _gloEx = ex;
            }
            finally
            {
                //CloseFbData();
                //DisposeAll();
            }

            if (_gloEx != null)
            {
                throw _gloEx;
            }

            return output;
        }

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
                using (_com = GetCommandDb(sql, commandType))
                {
                    SetParameter(parms, ref _com);
                    //fbrd = com.ExecuteReader(CommandBehavior.SingleRow);
                    fbrd = _com.ExecuteReader();
                }
            }

            catch (Exception ex)
            {
                _gloEx = ex;
                //CloseFbData();
            }

            if (_gloEx != null)
            {
                throw _gloEx;
            }

            return fbrd;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
       public int FbExecuteTransaction(TransactionSetCollection ts)
        {

            int rowAffect = 0;

            //OpenFbData();
             //_tran = _con.BeginTransaction();
            try
            {
                foreach (TransactionSet t in ts)
                {
                    //com = new FbCommand(t.SqlCommand, con);
                    //com.Transaction = trans;
                    _com = GetCommandDb(t.SqlCommand, CommandType.Text);

                    if (t.Parameter != null)
                    {
                        SetParameter(t.Parameter, ref _com);
                    }
                   rowAffect += _com.ExecuteNonQuery();
                }
                //_tran.Commit();
                //CommitTransaction();
            }
            catch (Exception ex)
            {
                //_tran.Rollback();
                //RollBackTransaction();
                _gloEx = ex;
                rowAffect = 0;
            }
            finally
            {
                //CloseFbData();
                //DisposeAll();
            }


            if (_gloEx != null)
            {
                throw _gloEx;
            }

            return rowAffect;
        }
       public int FbExecuteTransaction( string sql, List<FbParameter> parms,
                                            CommandType commandType = CommandType.Text )
        {
            int affectRow = 0;


            try
            {
                //OpenFbData();
                //com = new FbCommand(sql, con);
                //com.CommandType = commandType;
                //foreach (var p in parms)
                //{

                //    com.Parameters.Add(p);

                //}
            
                using (_com = GetCommandDb(sql, commandType ))
                {
                    SetParameter(parms, ref _com);

                    affectRow = _com.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                _gloEx = ex;
            }
            //finally
            //{
            //    //CloseFbData();
            //    //DisposeAll();
            //}

            if (_gloEx != null)
            {
                throw _gloEx;
            }


            //Command Can Effect one or more rows.

            return affectRow;
        }
      




        public virtual void OpenFbData()
        {
            //Check Null
            if (_con == null)
            {
                _con = new FbConnection(_constr);
            }


            if (_con.State == ConnectionState.Closed)
            {
                _con.Open();
            }

            //return con;
        }


        public virtual void CloseFbData()
        {
            if (_con.State == ConnectionState.Open)
            {
                _con.Close();
                _con.Dispose();
            }
           
        }

       
        public virtual void BeginTransaction()
        {
             //OpenFbData();
            _tran = _con.BeginTransaction();

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





        public  void DisposeAll()
        {
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


            _tran = null;
        }


        //private FbCommand GetCommandDb(string sql, CommandType commandType = CommandType.Text,
        //                               FbTransaction tansaction = null)
          private FbCommand GetCommandDb(string sql, CommandType commandType = CommandType.Text)
                          
        {
          
            if (_com == null)
            {
                //_com = new FbCommand(sql, _con);
                _com = new FbCommand();
            }

            _com.Connection = _con;
            if (_tran != null)
            {
                _com.Transaction = _tran;
            }

            _com.CommandText = sql;
            _com.CommandType = commandType;
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


        private FbDataAdapter GetAdapterDb(string sql, FbConnection con)
        {
            return _adapter ?? (_adapter = new FbDataAdapter(sql, con));
        }

        private void SetParameter(IEnumerable<FbParameter> parms, ref FbCommand com)
        {
            com.Parameters.Clear();
            foreach (FbParameter p in parms)
            {
                com.Parameters.Add(p);
            }
        }
    }
}