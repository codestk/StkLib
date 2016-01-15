using System;
using System.Globalization;

namespace StkLib.CCdate
{
    public class CCDateTime
    {
        /// <summary>
        ///     datetime = dd/MM/yyyy   En
        /// </summary>
      
        /// <param name="ddMMyyyyEn">>11/12/2015</param>
        /// <returns></returns>
        public static DateTime ConvertToDate(string ddMMyyyyEn)
        {
            string[] dateString = ddMMyyyyEn.Split('/');

            int d = Convert.ToInt32(dateString[0].TrimStart('0'));

            int m = Convert.ToInt32(dateString[1].TrimStart('0'));
            int y = Convert.ToInt32(dateString[2].TrimStart('0'));
            var dt = new DateTime(y, m, d);
            return dt;
        }


        /// <summary>
        ///     ใช้เพื่อ....
        /// </summary>
 
        /// <param name="ddMMyyyyTh">11/12/2558</param>
        /// th-TH
        /// <returns></returns>
        public static DateTime TextToDateThToEn(string ddMMyyyyTh)
        {
            string[] dateArray = ddMMyyyyTh.Split('/');

            //MM/dd/yyyy
            string dateStringReformat = dateArray[0] + "/" + dateArray[1] + "/" + dateArray[2];

            var culture = new CultureInfo("th-TH", false);


            DateTime dt = DateTime.ParseExact(dateStringReformat, "d", culture);

            return dt;
        }
    }
}