using System;
using System.Drawing;

namespace BB.UI.Web.MVC.Controllers.Utils
{
    public class ImageDecoder
    {
        public static Image DecodeBase64String(string base64)
        {
            byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(base64));
            var streamBitmap = new System.IO.MemoryStream(bitmapData);
            var bitImage = Image.FromStream(streamBitmap);
  
            return bitImage;
        }

        public static string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", string.Empty);
            sbText.Replace(" ", string.Empty);
            return sbText.ToString();
        }
    }
}