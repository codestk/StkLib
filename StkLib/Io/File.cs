using System;

namespace StkLib.Io
{
    public class StkFile
    {
      
            /// <summary>
            /// Function to get byte array from a file
            /// </summary>
            /// <param name="fileName">File name to get byte array</param>
            /// <returns>Byte Array</returns>
            public static byte[] FileToByteArray(string fileName)
            {
                byte[] buffer;

                // Open file for reading
                var fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                // attach filestream to binary reader
                using (var binaryReader = new System.IO.BinaryReader(fileStream))
                {
                    var totalBytes = new System.IO.FileInfo(fileName).Length;

                    // read entire file into buffer
                    buffer = binaryReader.ReadBytes((Int32)totalBytes);

                    // close file reader
                    fileStream.Close();
                    fileStream.Dispose();
                    binaryReader.Close();
                }

                return buffer;
            }

        }
    }
 
