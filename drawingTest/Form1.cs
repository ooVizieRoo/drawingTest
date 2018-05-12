using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace drawingTest
{
    enum State
    {
        /// <summary>
        /// Выбор элементов схемы.
        /// </summary>
        Select,
        /// <summary>
        /// Добавление узла.
        /// </summary>
        AddNode,
        /// <summary>
        /// Установление связей между узлами.
        /// </summary>
        Connect,
        /// <summary>
        /// Удаление элементов схемы.
        /// </summary>
        Delete
    }

    public partial class Form1 : Form
    {
        /// <summary>
        /// Смещение координат относительно начала координат контрола при клике на него.
        /// </summary>
        Point mouseOffset;
        /// <summary>
        /// Выбранный инструмент редактора.
        /// </summary>
        State state;
        
        public Form1()
        {
            InitializeComponent();

            state = State.Select;
            btnTopologySelect.Enabled = false;
        }

        private void setNBEvents()
        {
            var nbs = panel1.Controls.OfType<NodeBox>().Cast<NodeBox>().ToList();

            if (nbs == null)
                return;

            foreach (NodeBox nb in nbs)
            {
                switch (state)
                {
                    case State.Select:
                        {
                            //nb.MouseClick     -= new MouseEventHandler(nodeBox_MouseClick);
                            nb.MouseClick       -= new MouseEventHandler(nodeBox_MouseClickOnDelete);
                            nb.MouseDown        += new MouseEventHandler(nodeBox_MouseDown);
                            nb.MouseMove        += new MouseEventHandler(nodeBox_MouseMove);
                            nb.MouseUp          += new MouseEventHandler(nodeBox_MouseUp);
                            nb.Move             += new EventHandler(nodeBox_Move);
                            nb.MouseHover       += new EventHandler(nodeBox_MouseHover);
                            nb.MouseLeave       += new EventHandler(nodeBox_MouseLeave);

                            panel1.Click        -= new EventHandler(AddNode);

                            break;
                        }
                    case State.AddNode:
                        {
                            //nb.MouseClick     += new MouseEventHandler(nodeBox_MouseClick);
                            nb.MouseClick       -= new MouseEventHandler(nodeBox_MouseClickOnDelete);
                            nb.MouseDown        -= new MouseEventHandler(nodeBox_MouseDown);
                            nb.MouseMove        -= new MouseEventHandler(nodeBox_MouseMove);
                            nb.MouseUp          -= new MouseEventHandler(nodeBox_MouseUp);
                            nb.Move             -= new EventHandler(nodeBox_Move);
                            nb.MouseHover       -= new EventHandler(nodeBox_MouseHover);
                            nb.MouseLeave       -= new EventHandler(nodeBox_MouseLeave);


                            break;
                        }
                    case State.Connect:
                        {

                            //nb.MouseClick     += new MouseEventHandler(nodeBox_MouseClick);
                            nb.MouseClick       -= new MouseEventHandler(nodeBox_MouseClickOnDelete);
                            nb.MouseDown        -= new MouseEventHandler(nodeBox_MouseDown);
                            nb.MouseMove        -= new MouseEventHandler(nodeBox_MouseMove);
                            nb.MouseUp          -= new MouseEventHandler(nodeBox_MouseUp);
                            nb.Move             -= new EventHandler(nodeBox_Move);
                            nb.MouseHover       += new EventHandler(nodeBox_MouseHover);
                            nb.MouseLeave       += new EventHandler(nodeBox_MouseLeave);

                            panel1.Click        -= new EventHandler(AddNode);

                            break;
                        }
                    case State.Delete:
                        {
                            //nb.MouseClick     -= new MouseEventHandler(nodeBox_MouseClick);
                            nb.MouseClick       += new MouseEventHandler(nodeBox_MouseClickOnDelete);
                            nb.MouseDown        -= new MouseEventHandler(nodeBox_MouseDown);
                            nb.MouseMove        -= new MouseEventHandler(nodeBox_MouseMove);
                            nb.MouseUp          -= new MouseEventHandler(nodeBox_MouseUp);
                            nb.Move             -= new EventHandler(nodeBox_Move);
                            nb.MouseHover       += new EventHandler(nodeBox_MouseHover);
                            nb.MouseLeave       += new EventHandler(nodeBox_MouseLeave);

                            panel1.Click        -= new EventHandler(AddNode);

                            break;
                        }
                    default: return;
                }
            }

            if (state == State.AddNode)
                panel1.Click += new EventHandler(AddNode);
        }


        private void panel1_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            
            g.DrawLine(new Pen(Color.Red), 10, 10, 100, 100);
        }

        private void nodeBox_MouseDown(object sender, MouseEventArgs e)
        {
            var nb = (NodeBox)sender;
            nb.IsDown = true;

            mouseOffset = e.Location;
        }

        private void nodeBox_MouseMove(object sender, MouseEventArgs e)
        {
            var nb = (NodeBox)sender;

            if (nb.IsDown)
            {
                Point mouse = MousePosition;

                Point pnt = new Point(mouse.X - (mouseOffset.X), mouse.Y - (mouseOffset.Y));

                nb.Location = panel1.PointToClient(pnt);
            }
        }

        private void nodeBox_MouseUp(object sender, MouseEventArgs e)
        {
            var nb = (NodeBox)sender;
            nb.IsDown = false;
        }

        private void nodeBox_Move(object sender, EventArgs e)
        {
            var nb = (NodeBox)sender;

            if (nb.Location.X < 0)
            {
                nb.Location = new Point(0, nb.Location.Y);
                nb.IsDown = false;
            }

            if (nb.Location.Y < 0)
            {
                nb.Location = new Point(nb.Location.X, 0);
                nb.IsDown = false;
            }

            int critSize = panel1.Size.Width - nb.Size.Width - 4;
            if (nb.Location.X > critSize)
            {
                nb.Location = new Point(critSize, nb.Location.Y);
                nb.IsDown = false;
            }

            critSize = panel1.Size.Height - nb.Size.Height - 4;
            if (nb.Location.Y > critSize)
            {
                nb.Location = new Point(nb.Location.X, critSize);
                nb.IsDown = false;
            }
        }

        private void nodeBox_MouseHover(object sender, EventArgs e)
        {
            var nb = (NodeBox)sender;
            
            nb.BorderStyle = BorderStyle.Fixed3D;
        }

        private void nodeBox_MouseLeave(object sender, EventArgs e)
        {
            var nb = (NodeBox)sender;

            if (!nb.IsSelected)
                nb.BorderStyle = BorderStyle.FixedSingle;
        }

        private void nodeBox_MouseClickOnDelete(object sender, MouseEventArgs e)
        {
            var nb = (NodeBox)sender;

            panel1.Controls.Remove(nb);
            nb = null;
        }

        private void btnTopologyAdd_Click(object sender, EventArgs e)
        {
            state = State.AddNode;

            btnTopologyAdd.Enabled = false;
            btnTopologyAddDependency.Enabled = true;
            btnTopologyDelete.Enabled = true;
            btnTopologySelect.Enabled = true;

            setNBEvents();
        }

        private void AddNode(object sender, EventArgs e)
        {
            NodeBox nb = new NodeBox();
            //nb.MouseDown += new MouseEventHandler(nodeBox_MouseDown);
            //nb.MouseMove += new MouseEventHandler(nodeBox_MouseMove);
            //nb.MouseUp += new MouseEventHandler(nodeBox_MouseUp);
            //nb.Move += new EventHandler(nodeBox_Move);
            //nb.MouseHover += new EventHandler(nodeBox_MouseHover);
            //nb.MouseLeave += new EventHandler(nodeBox_MouseLeave);

            nb.Location = MousePosition;

            panel1.Controls.Add(nb);
        }

        private void btnTopologyAddDependency_Click(object sender, EventArgs e)
        {
            state = State.Connect;

            btnTopologyAdd.Enabled = true;
            btnTopologyAddDependency.Enabled = false;
            btnTopologyDelete.Enabled = true;
            btnTopologySelect.Enabled = true;

            setNBEvents();
        }

        private void btnTopologySelect_Click(object sender, EventArgs e)
        {
            state = State.Select;

            btnTopologyAdd.Enabled = true;
            btnTopologyAddDependency.Enabled = true;
            btnTopologyDelete.Enabled = true;
            btnTopologySelect.Enabled = false;

            setNBEvents();
        }

        private void btnTopologyDelete_Click(object sender, EventArgs e)
        {
            state = State.Delete;

            btnTopologyAdd.Enabled = true;
            btnTopologyAddDependency.Enabled = true;
            btnTopologyDelete.Enabled = false;
            btnTopologySelect.Enabled = true;

            setNBEvents();
        }
    }
}
