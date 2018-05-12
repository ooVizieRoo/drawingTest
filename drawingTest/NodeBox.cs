using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace drawingTest
{
    
    public partial class NodeBox : PictureBox
    {
        /// <summary>
        /// Нажата ли лев. кнопка мыши на данном контроле.
        /// </summary>
        public bool IsDown { get; set; }
        public bool IsSelected { get; set; }
        public List<NodeBox> listOfNbDependencies;


        public NodeBox() : base()
        {
            Image = Properties.Resources.pc;
            Size = new Size(64, 64);
            SizeMode = PictureBoxSizeMode.Zoom;
            BorderStyle = BorderStyle.FixedSingle;
            listOfNbDependencies = new List<NodeBox>();

            

            IsDown = false;
            IsSelected = false;
            InitializeComponent();
        }
    }
}
