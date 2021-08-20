using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Encoder = System.Drawing.Imaging.Encoder;

namespace Tools_Injector_Mod_Menu
{
    public static class Utility
    {
        #region Image Encoder

        //https://stackoverflow.com/a/45673201/8902883
        public static string ImageToBase64(this Image image, ImageFormat imageFormat)
        {
            using var ms = new MemoryStream();
            image.Save(ms, imageFormat);
            var imageBytes = ms.ToArray();
            return Convert.ToBase64String(imageBytes);
        }

        public static Image Base64ToImage(this string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            using var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            return Image.FromStream(ms, true);
        }

        public static string GetMimeType(this string fileName)
        {
            var mimeType = "application/unknown";
            var ext = Path.GetExtension(fileName).ToLower();
            var regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey?.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        //https://stackoverflow.com/a/24651073/8902883
        public static Image CompressImage(this string fileName, int newQuality)
        {
            using var image = new Bitmap(fileName);
            using var memImage = new Bitmap(image);

            var myEncoderParameters = new EncoderParameters(1)
            {
                Param = { [0] = new EncoderParameter(Encoder.Quality, newQuality) }
            };

            var memStream = new MemoryStream();
            memImage.Save(memStream, GetEncoderInfo(GetMimeType(fileName)), myEncoderParameters);
            var newImage = Image.FromStream(memStream);
            var imageAttributes = new ImageAttributes();
            using var g = Graphics.FromImage(newImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0, newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
            return newImage;
        }

        public static ImageCodecInfo GetEncoderInfo(this string mimeType)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();
            return Array.Find(encoders, ici => ici.MimeType == mimeType);
        }

        #endregion Image Encoder

        public static string SmaliCountToName(int count, bool move = false)
        {
            if (move)
            {
                return $"smali_classes{count + 1}";
            }

            return count == 1 ? "smali" : $"smali_classes{count}";
        }

        public static string SmaliCountToClass(int count)
        {
            return $"classes{count}.dex";
        }

        public static string GetApkName(string apk)
        {
            try
            {
                var split = apk.Split('\\');
                return split[split.Length - 1];
            }
            catch
            {
                return null;
            }
        }
    }
}