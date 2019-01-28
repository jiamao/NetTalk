using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ImageResources
{
    public class Face
    {
        public static System.Drawing.Image getGifByName(string gifname)
        {
            try
            {
                object obj = FaceSource.ResourceManager.GetObject("_" + gifname);
                System.Drawing.Image img = (System.Drawing.Image)(obj);
                string giftmppath = Application.StartupPath + "\\FaceImage\\" + gifname + ".gif";
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(giftmppath))) System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(giftmppath));
                if (System.IO.File.Exists(giftmppath)) System.IO.File.Delete(giftmppath);
                img.Save(giftmppath, System.Drawing.Imaging.ImageFormat.Gif);
                img.Dispose();
                return System.Drawing.Image.FromFile(giftmppath);
            }
            catch
            {
                return null;
            }
        }
    }
}
