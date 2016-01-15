using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StkLib.Io
{
    public class ImageUtiliy
    {

        //Convert image for sqvw To Db
        //"Insert into [msg] (msgid, pic1) Values (1, @Pic)", conn);
        //insertCommand.Parameters.Add("Pic", SqlDbType.Image, 0).Value =
        //ConvertImageToByteArray(imag, System.Drawing.Imaging.ImageFormat.Jpeg);
        public static   byte[] ConvertImageToByteArray( Image imageToConvert,ImageFormat formatOfImage)
        {
            byte[] ret;
            using (var ms = new MemoryStream())
            {
                imageToConvert.Save(ms, formatOfImage);
                ret = ms.ToArray();
            }
            return ret;
        }
    }
}
