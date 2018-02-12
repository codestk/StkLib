namespace Stk.DbSql.DbUtil
{
    public class SerachParameters
    {

        ////OrderBy:Column
        //public static string GetOrder(string order)
        //{   //"ดอกไม้
        //    //"ดอกไม้;Size:All"
        //    string output = "";

        //    output = GetValue(order, "OrderBy");
        //    return output;
        //}


        public static string GetValue(string filter, string parameter)
        {
            var output = "";
            var query = filter.Trim();
            var temparry = query.Split(';');
            foreach (string key in temparry)
            {
                var keyArray = key.Split(':');
                if (keyArray[0] == parameter)
                {

                    output = keyArray[1];

                }
            }
            return output.Trim();
        }
        
        //in=  "aaa,dddd,cccc"
        //out= "'aaa','dddd','cccc'"
        public static string SetSingleQuote(string str)
        {
            string[] temp = str.TrimEnd(',').Split(',');
            string output = "";
            foreach (string a in temp)
            {
                output += "'" + a + "',";

            }
            output = output.Trim(',');
            return output;
        }

    }
}
