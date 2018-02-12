using System;
using System.Text.RegularExpressions;

namespace StkLib.Strings
{
    public class StkString
    {

        /// <summary>
        /// Return Pure Numerric   ekoroew12334= 12334
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
       public static  string RemoveNonNumerric(string text)
        {
            var outPut = "";
            outPut += Regex.Replace(text, "[^0-9]", "");
            outPut = outPut.TrimEnd();
            return outPut;
        }


       //Remove White Spaces
       public static string TrimWhiteSpace(string str)
        {
            if (str==null)
            return "";
            return  Regex.Replace(str, @"^\s+", string.Empty);
        }



        /// <summary>
        /// True = Null
        /// False = Not Null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
       public static  bool IsNull(string str)
        {
            return String.IsNullOrEmpty(str);
        }



        public static bool IsDecimal(string val)
        {
            decimal value;
            bool result = Decimal.TryParse(val, out value);


            return result;
        }


       //Check Has Number in String 
       //Same Result
       //var patt1 = /\d/g;
       //var lenTime1 = valTime1.match(patt1);
       public static bool HasNumber(string text)
       {
           bool output = false;
           MatchCollection matches = Regex.Matches(text, @"\d+");

           if (matches.Count > 0)
               output = true;
           return output;
       }

       /*Cehck */
       public static  bool IsNumber(string text)
        {
            var regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text);
        }


       /// True =  Alpha
       /// Proved
       public static bool IsAlpha(string input)
       {
           return Regex.IsMatch(input, "^[a-zA-Z]+$");
       }

       public static bool IsAlphaNumeric(string input)
       {
           return Regex.IsMatch(input, "^[a-zA-Z0-9]+$");
       }

       public static bool IsAlphaNumericWithUnderscore(string input)
       {
           return Regex.IsMatch(input, "^[a-zA-Z0-9_]+$");
       }
    }
}