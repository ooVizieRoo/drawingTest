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
        public Point CenterPoint { get; private set; }
        public Point CenterLocation { get; private set; }
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
            calculateNodeBoxCenterPoint();
            InitializeComponent();

            this.Move += calculateCenterLocation;
        }

        private void calculateNodeBoxCenterPoint()
        {
            CenterPoint = new Point(Size.Width / 2, Size.Height / 2);
        }

        private void calculateCenterLocation (object sender, EventArgs e)
        {
            CenterLocation = new Point(Location.X + CenterPoint.X, Location.Y + CenterPoint.Y);
            Console.WriteLine(CenterLocation.ToString());
        }
    }
}
