using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace Stk.DbSql
{
    public class TransactionSet
    {

          public TransactionSet()
        {
            

        }

        string _sqlCommand;
        public string SqlCommand
        {
            get
            {
                return _sqlCommand.Trim();
            }
            set
            {
                _sqlCommand = value;
            }
        }

        public List<FbParameter> Parameter { get; set; }

        [DefaultValue(CommandType.Text)]
        public CommandType ExecuteType{ get; set; }

    }
}