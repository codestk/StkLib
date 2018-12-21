using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace StkLib.Json
{
    static class JsonData
    {
        /// <summary>
        ///  >> {"Email":"lkcccdflkfsd@hotmail.comcc","Name":"A10001","Pop":"dsds","Sat":"dsdsds"} ,{"Email":"lkcccdflkfsd@hotmail.comcc","Name":"A10001","Pop":"dsds","Sat":"dsdsds"} 
        /// </summary>
        /// <param name="jsonString"></param>
        /// 
        /// <returns></returns>
        public static DataTable JsonToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            //strip out bad characters
            string[] jsonParts = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");

            //hold column names
            List<string> dtColumns = new List<string>();

            //get columns
            foreach (string jp in jsonParts)
            {
                //only loop thru once to get column names
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",");
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1);
                        string v = rowData.Substring(idx + 1);
                        if (!dtColumns.Contains(n))
                        {
                            dtColumns.Add(n.Replace("\"", ""));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", rowData));
                    }

                }
                break; // TODO: might not be correct. Was : Exit For
            }

            //build dt
            foreach (string c in dtColumns)
            {
                dt.Columns.Add(c);
            }
            //get table data
            foreach (string jp in jsonParts)
            {
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string v = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[n] = v;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
                dt.Rows.Add(nr);
            }
            return dt;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static object[] DataTableToJsonArray(DataTable dt)
        {
            var temp = dt.AsEnumerable()
          .Select(r => r.Table.Columns.Cast<DataColumn>()
                  .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                 ).ToDictionary(z => z.Key, z => z.Value)
          ).ToArray();

            return temp;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonToString(object obj)
        {
            var js = new JavaScriptSerializer();

            var jsonText = js.Serialize(obj);

            return jsonText;
        }

    }
}
