using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace LanTalk
{
    class buttonitem:DevComponents.DotNetBar.ButtonItem
    {
        Image initimage;
        public Image InitImage
        {
            set { initimage = value; }
            get { return initimage; }
        }
        public void start()
        {
            if (initimage != null)
            {
                this.Image = initimage;
                if (System.Drawing.ImageAnimator.CanAnimate(initimage))
                {
                    ImageAnimator.Animate(initimage,new EventHandler(onframechange));
                }
            }
        }
        private void onframechange(object sender, EventArgs e)
        {
            try
            {
                this.Refresh();
            }
            catch
            { }
        }
    }
}
