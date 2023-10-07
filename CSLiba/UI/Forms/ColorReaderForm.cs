using MaterialSkin.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLiba.UI.Forms
{
    public partial class ColorReaderForm : MaterialForm
    {
        public ColorReaderForm()
        {
            InitializeComponent();

            MaximumSize = Size;
            MinimumSize = Size;
            UIForm.materialSkinManager.AddFormToManage(this);

            materialSlider1.onValueChanged += (s, e) => InvalidateColor();
            materialSlider2.onValueChanged += (s, e) => InvalidateColor();
            materialSlider3.onValueChanged += (s, e) => InvalidateColor();
            materialSlider4.onValueChanged += (s, e) => InvalidateColor();
        }

        public bool cancel = true;
        public Vector4 color = new Vector4(255, 0, 0, 255);

        public static Vector4 GetColor(PictureBox pb)
        {
            ColorReaderForm form = new ColorReaderForm();
            form.ShowDialog();
            if (!form.cancel)
            {
                pb.BackColor = System.Drawing.Color.FromArgb((int)form.color.X, (int)form.color.Y, (int)form.color.Z);
                if (form.materialCheckbox1.Checked)
                    return new Vector4(-1, -1, -1, form.color.W);
                return form.color;
            }
            return Vector4.Zero;
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            cancel = false;
            Close();
        }

        private void InvalidateColor()
        {
            color = new Vector4(materialSlider1.Value, materialSlider2.Value, materialSlider3.Value, materialSlider4.Value);
            pictureBox1.BackColor = System.Drawing.Color.FromArgb(materialSlider1.Value, materialSlider2.Value, materialSlider3.Value);
        }

        private void materialSlider1_Click(object sender, EventArgs e)
        {
            InvalidateColor();
        }

        private void materialSlider2_Click(object sender, EventArgs e)
        {
            InvalidateColor();
        }

        private void materialSlider3_Click(object sender, EventArgs e)
        {
            InvalidateColor();
        }

        private void materialSlider4_Click(object sender, EventArgs e)
        {
            InvalidateColor();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckbox1.Checked)
            {
                materialSlider1.Value = 0;
                materialSlider2.Value = 0;
                materialSlider3.Value = 0;
                materialSlider1.Enabled = false;
                materialSlider2.Enabled = false;
                materialSlider3.Enabled = false;
            }
            else
            {
                materialSlider1.Value = 255;
                materialSlider3.Value = 0;
                materialSlider2.Value = 0;
                materialSlider3.Enabled = true;
                materialSlider1.Enabled = true;
                materialSlider2.Enabled = true;
            }
            InvalidateColor();
        }
    }
}
