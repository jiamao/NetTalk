using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LanTalk
{
    /// <summary>
    /// 峰
    /// </summary>
    class InputBox
    {
        public static string InputTextBox(string Caption, string Hint, string Default)
        {
            //by 
            Form InputForm = new Form();
            InputForm.MinimizeBox = false;
            InputForm.MaximizeBox = false;
            InputForm.Opacity = 0.95;
            InputForm.BackColor = System.Drawing.Color.Teal;
            InputForm.StartPosition = FormStartPosition.CenterParent;
            InputForm.Width = 220;
            InputForm.Height = 150;
            //InputForm.Font.Name = "宋体";
            //InputForm.Font.Size = 10;

            InputForm.Text = Caption;
            Label lbl = new Label();
            lbl.Text = Hint;
            lbl.Left = 10;
            lbl.Top = 20;
            lbl.Parent = InputForm;
            lbl.AutoSize = true;
            TextBox tb = new TextBox();
            tb.Left = 30;
            tb.Top = 45;
            tb.Width = 160;
            tb.Parent = InputForm;
            tb.Text = Default;
            tb.SelectAll();
            //tb.KeyDown += keydown;
            tb.KeyPress += KeyPress;
            Button btnok = new Button();
            btnok.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            btnok.Left = 30;
            btnok.Top = 80;
            btnok.Parent = InputForm;
            btnok.Text = "确定";
            InputForm.AcceptButton = btnok;//回车响应

            btnok.DialogResult = DialogResult.OK;
            Button btncancal = new Button();
            btncancal.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            btncancal.Left = 120;
            btncancal.Top = 80;
            btncancal.Parent = InputForm;
            btncancal.Text = "取消";
            btncancal.DialogResult = DialogResult.Cancel;
            try
            {
                if (InputForm.ShowDialog() == DialogResult.OK)
                {
                    return tb.Text;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                InputForm.Dispose();
            }

        }
        private static void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar < 48 && e.KeyChar != '.' && e.KeyChar != '\b') || e.KeyChar > 57)
                {
                    e.Handled = true;
                }
                if (e.KeyChar == '.' && (sender as TextBox).Text.Split('.').Length >= 4)
                {
                    e.Handled = true;
                }
                if (e.KeyChar != '.' && e.KeyChar != '\b' && (((sender as TextBox).Text.Split('.')[((sender as TextBox).Text.Split('.').Length - 1)]).Length >= 3 || (int.Parse((sender as TextBox).Text.Split('.')[((sender as TextBox).Text.Split('.').Length - 1)] + e.KeyChar.ToString())) > 255))
                {
                    e.Handled = true;
                }
            }
            catch
            { }
        }
        private static void keydown(object sender, KeyEventArgs e)
        {
            if ( e.KeyValue < 48 || (e.KeyValue > 57 && e.KeyValue != 190))
            {
                e.Handled = true;
                return;
            }
        }
    }
}
