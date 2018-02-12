using System;

namespace StkLib.Io.TextFile
{
    public class TxtFile
    {



        public string GenerateFileName(int length, string folderPath)
        {
            // selected characters
            const string chars = "2346789ABCDEFGHJKLMNPQRTUVWXYZabcdefghjkmnpqrtuvwxyz";
            // create random generator
            var rnd = new Random();
            //do
            //{
                // create name
                string name = string.Empty;
                while (name.Length < length)
                {
                    name += chars.Substring(rnd.Next(chars.Length), 1);
                }
                // add extension
            //    name += ".txt";
            //    // check against files in the folder
            //} while (File.Exists(Path.Combine(folderPath, name)));

            return name;
        }


         


    }
}
