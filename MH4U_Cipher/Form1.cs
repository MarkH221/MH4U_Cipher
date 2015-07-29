using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MH4UCipher;

namespace MH4U_Cipher_App
{
    public partial class Form1 : Form
    {
        private string Filepath;
        private byte[] SaveData;

        public Form1()
        {
            InitializeComponent();
            IEnumerable<Region> reg = Enum.GetValues(typeof (Region)).Cast<Region>();
            RegionBox.DataSource = reg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cipher(true, dlcrad.Checked);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cipher(false, dlcrad.Checked);
        }

        private bool Open()
        {
            using (var o = new OpenFileDialog())
            {
                if (o.ShowDialog() != DialogResult.OK) return false;
                Filepath = o.FileName;
            }
            return true;
        }

        private bool Save(byte[] buffer)
        {
            using (
                var s = new SaveFileDialog
                {
                    FileName = Path.GetFileName(Filepath),
                    Filter = "MH4U File|*user*;*m*.mib",
                    Title = "Save file, preferably not over the old one"
                })
            {
                if (s.ShowDialog() != DialogResult.OK) return false;
                try
                {
                    File.WriteAllBytes(s.FileName, buffer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    return false;
                }
            }
            return true;
        }

        private void Cipher(bool decrypt, bool dlc)
        {
            if (!Open()) return;
            //Decrypt then save
            if (dlc)
            {
                var file = new DLC((Region) RegionBox.SelectedIndex);
                Save((decrypt) ? file.Decrypt(Filepath) : file.Encrypt(Filepath));
            }
            else
            {
                var file = new SaveData((Region) RegionBox.SelectedIndex);
                Save((decrypt) ? file.Decrypt(Filepath) : file.Encrypt(Filepath));
            }
        }
    }
}