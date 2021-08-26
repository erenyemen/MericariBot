using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MericariBot.WinForms
{
    public partial class frmAdvert : Form
    {
        PictureBox _pbBox;
        PictureBox pbBox;

        public frmAdvert(PictureBox pbBox)
        {
            InitializeComponent();

            this.pbBox = pbBox;
            _pbBox = new PictureBox();
            _pbBox.Image = pbBox.Image;
            _pbBox.SizeMode = pbBox.SizeMode;
        }

        private void frmAdvert_Load(object sender, EventArgs e)
        {
            cmbSizeMode.SelectedIndex = 0;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            pbBox.ImageLocation = DownloadImage();
        }

        private string DownloadImage()
        {
            string TempFolderPath = $"{Environment.CurrentDirectory}/advertTemp";

            if (Directory.Exists(TempFolderPath))
                Directory.Delete(TempFolderPath, true);

            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            d.CreateSubdirectory("advertTemp");

            string imageName = txtImageUrl.Text.Split('/').Last().Trim();

            if (imageName.Contains("?"))
            {
                imageName = imageName.Split('?').First().Trim();
            }

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(txtImageUrl.Text.Trim()), $@"{TempFolderPath}\{imageName}");
            }

            return $@"{TempFolderPath}\{imageName}";
        }

        private void cmbSizeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            pbBox.SizeMode = (PictureBoxSizeMode)Enum.Parse(typeof(PictureBoxSizeMode), cmbSizeMode.SelectedItem.ToString());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pbBox.Image = _pbBox.Image;
            pbBox.SizeMode = _pbBox.SizeMode;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
