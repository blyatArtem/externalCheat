using MaterialSkin;
using MaterialSkin.Controls;
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
    public partial class KeyReaderForm : MaterialForm
    {
        public KeyReaderForm()
        {
            InitializeComponent();
            MaximumSize = Size;
            MinimumSize = Size;
            UIForm.materialSkinManager.AddFormToManage(this);

            foreach (Keys keys in Enum.GetValues(typeof(Keys)))
            {
                ToolStripMenuItem item = new ToolStripMenuItem(keys.ToString());
                item.Tag = keys;
                item.Click += (s, e) =>
                {
                    ToolStripMenuItem sender = (s as ToolStripMenuItem);
                    Keys key = (Keys)sender.Tag;
                    MaterialListBoxItem listBoxItem = new MaterialListBoxItem(key.ToString());
                    listBoxItem.Tag = key;
                    materialListBox1.Items.Add(listBoxItem);
                };
                (contextMenuStrip1.Items[0] as ToolStripMenuItem).DropDownItems.Add(item);
            }
        }

        public bool Cancel = true;
        public List<Keys> outKeys = new List<Keys>();

        private void materialButton1_Click(object sender, EventArgs e)
        {
            if (materialListBox1.Items.Count > 0)
            {
                Cancel = false;
                materialListBox1.Items.ToList().ForEach(x => outKeys.Add((Keys)x.Tag));
            }
            Close();
        }

        private void убратьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (materialListBox1.SelectedItem != null)
                materialListBox1.RemoveItem(materialListBox1.SelectedItem);
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
