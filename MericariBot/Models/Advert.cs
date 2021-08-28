using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MericariBot.Models
{
    public class Advert
    {
        public int AdvertId { get; set; }
        public string AdvertUrl { get; set; }
        public PictureBoxSizeMode ImageSizeMode { get; set; }
    }
}