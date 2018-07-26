using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace SngPropMod
{
    public partial class frmFileProp : Form
    {
        //-----------------------------------------------------------------------
        //Registry key info for MP3 files
        //https://superuser.com/questions/1026067/cant-add-context-menu-item-to-mp3-files-through-regedit

        //How to add an icon to a windows custom context menu item
        //http://www.winhelponline.com/blog/add-icon-to-right-click-menu-windows-7/
        //-----------------------------------------------------------------------

        //Registry Keys in C#
        //http://www.authorcode.com/how-to-add-a-context-menu-to-the-windows-explorer-in-c/
        //-----------------------------------------------------------------------

        string[] lstSongs;
        bool bModified = false;
        bool bDirectory = false;
        Mp3Lib.Mp3File mp3;

        bool bDiferent = false;

        string Title = "";
        string Album = "";
        string Artist = "";
        Image Picture = null;

        public frmFileProp(string[] Songs, bool Dir = false)
        {
            InitializeComponent();
            lstSongs = Songs;
            bDirectory = Dir;
            Init();
        }

        private void Init()
        {
            this.Text = "Properties: ";
            foreach (string sng in lstSongs)
            {
                this.Text += Path.GetFileName(sng) + ", ";
                mp3 = new Mp3Lib.Mp3File(sng);

                if (Title == "")
                    Title = mp3.TagHandler.Title;

                if (Album == "")
                    Album = mp3.TagHandler.Album;

                if (Artist == "")
                    Artist = mp3.TagHandler.Artist;

                if (Picture == null)
                    Picture = mp3.TagHandler.Picture;

                if (Album != mp3.TagHandler.Album)
                {
                    if (mp3.TagHandler.Album != "")
                        bDiferent = true;
                }

                if (Artist != mp3.TagHandler.Artist)
                {
                    if (mp3.TagHandler.Artist != "")
                        bDiferent = true;
                }

                if (bDiferent)
                {
                    Album = "";
                    Artist = "";
                    Title = "";
                    Picture = null;
                    break;
                }
            }

            this.Text = this.Text.Substring(0, this.Text.Length - 2);

            pbImage.Image = Picture;
            txtAlbum.Text = Album;
            txtArtist.Text = Artist;
            txtTitle.Text = Title;

            if (bDirectory)
            {
                lblTitle.Visible = false;
                txtTitle.Visible = false;
                btApply.Enabled = true;
                bModified = false;
            }
            else
            {
                btApply.Enabled = false;
                bModified = false;
            }
        }

        private void btAccept_Click(object sender, System.EventArgs e)
        {
            if (bModified == true)
            {
                Save();
            }
            this.Close();
        }

        private void btCancel_Click(object sender, System.EventArgs e)
        {
            if (bModified == true)
            {
                if (MessageBox.Show("Está seguro que desea descartar los cambios realizados?", "Descartar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    this.Close();
            }
            else
                this.Close();
        }

        private void btApply_Click(object sender, System.EventArgs e)
        {
            Save();
        }

        private void pbImage_DoubleClick(object sender, System.EventArgs e)
        {
            if (ofdPicture.ShowDialog() == DialogResult.OK && ofdPicture.FileName != "")
            {
                if (Image.FromFile(ofdPicture.FileName).Width <= 800 && Image.FromFile(ofdPicture.FileName).Height <= 800)
                {
                    pbImage.Image = Image.FromFile(ofdPicture.FileName);
                    ActiveModified();
                }
                else
                {
                    MessageBox.Show("The dimensions of image file should be less than 800px", "Image file error");
                }
            }
        }

        private void txtTitle_TextChanged(object sender, System.EventArgs e)
        {
            ActiveModified();
        }

        private void txtArtist_TextChanged(object sender, System.EventArgs e)
        {
            ActiveModified();
        }

        private void txtAlbum_TextChanged(object sender, System.EventArgs e)
        {
            ActiveModified();
        }

        private void Save()
        {
            foreach (string sng in lstSongs)
            {
                mp3 = new Mp3Lib.Mp3File(sng);
                mp3.TagHandler.Album = txtAlbum.Text;
                mp3.TagHandler.Artist = txtArtist.Text;

                if (!bDirectory)
                {
                    mp3.TagHandler.Title = txtTitle.Text;
                    mp3.TagHandler.Song = mp3.TagHandler.Title;
                }

                mp3.TagHandler.Picture = pbImage.Image;
                mp3.Update();
            }

            bModified = false;
        }

        private void ActiveModified()
        {
            bModified = true;
            btApply.Enabled = true;
        }
    }
}
