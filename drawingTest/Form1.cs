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
        List<NodeBox> selectedNBs;
        Graphics g;
        List<Edge> listOfEdges;

        public Form1()
        {
            InitializeComponent();

            state = State.Select;
            btnTopologySelect.Enabled = false;

            selectedNBs = new List<NodeBox>();
            g = panel1.CreateGraphics();
            listOfEdges = new List<Edge>();
            DoubleBuffered = true;
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
                            nb.MouseUp          -= new MouseEventHandler(nodeBox_MoveFinished);

                            nb.MouseClick       -= new MouseEventHandler(nodeBox_MouseClickOnConnect);
                            nb.MouseClick       -= new MouseEventHandler(nodeBox_MouseClickOnDelete);
                            nb.MouseDown        += new MouseEventHandler(nodeBox_MouseDown);
                            nb.MouseHover       += new EventHandler(nodeBox_MouseHover);
                            nb.MouseLeave       += new EventHandler(nodeBox_MouseLeave);
                            nb.MouseMove        += new MouseEventHandler(nodeBox_MouseMove);
                            nb.MouseUp          += new MouseEventHandler(nodeBox_MouseUp);
                            nb.MouseUp          += new MouseEventHandler(nodeBox_MoveFinished);
                            //nb.Move             += new EventHandler(nodeBox_MoveStarted);
                            nb.Paint            += new PaintEventHandler(nodeBox_OnPaint);

                            panel1.Click        -= new EventHandler(AddNode);
                            break;
                        }
                    case State.AddNode:
                        {
                            nb.MouseUp          -= new MouseEventHandler(nodeBox_MoveFinished);

                            nb.MouseClick       -= new MouseEventHandler(nodeBox_MouseClickOnConnect);
                            nb.MouseClick       -= new MouseEventHandler(nodeBox_MouseClickOnDelete);
                            nb.MouseDown        -= new MouseEventHandler(nodeBox_MouseDown);
                            nb.MouseHover       -= new EventHandler(nodeBox_MouseHover);
                            nb.MouseLeave       -= new EventHandler(nodeBox_MouseLeave);
                            nb.MouseMove        -= new MouseEventHandler(nodeBox_MouseMove);
                            nb.MouseUp          -= new MouseEventHandler(nodeBox_MouseUp);
                            nb.MouseUp          += new MouseEventHandler(nodeBox_MoveFinished);
                            nb.Move             -= new EventHandler(nodeBox_MoveStarted);

                            break;
                        }
                    case State.Connect:
                        {
                            nb.MouseClick       += new MouseEventHandler(nodeBox_MouseClickOnConnect);
                            nb.MouseClick       -= new MouseEventHandler(nodeBox_MouseClickOnDelete);
                            nb.MouseDown        -= new MouseEventHandler(nodeBox_MouseDown);
                            nb.MouseHover       += new EventHandler(nodeBox_MouseHover);
                            nb.MouseLeave       += new EventHandler(nodeBox_MouseLeave);
                            nb.MouseMove        -= new MouseEventHandler(nodeBox_MouseMove);
                            nb.MouseUp          -= new MouseEventHandler(nodeBox_MouseUp);
                            nb.MouseUp          -= new MouseEventHandler(nodeBox_MoveFinished);
                            nb.Move             -= new EventHandler(nodeBox_MoveStarted);


                            panel1.Click        -= new EventHandler(AddNode);

                            break;
                        }
                    case State.Delete:
                        {
                            nb.MouseClick       -= new MouseEventHandler(nodeBox_MouseClickOnConnect);
                            nb.MouseClick       += new MouseEventHandler(nodeBox_MouseClickOnDelete);
                            nb.MouseDown        -= new MouseEventHandler(nodeBox_MouseDown);
                            nb.MouseHover       += new EventHandler(nodeBox_MouseHover);
                            nb.MouseLeave       += new EventHandler(nodeBox_MouseLeave);
                            nb.MouseMove        -= new MouseEventHandler(nodeBox_MouseMove);
                            nb.MouseUp          -= new MouseEventHandler(nodeBox_MouseUp);
                            nb.MouseUp          -= new MouseEventHandler(nodeBox_MoveFinished);
                            nb.Move             -= new EventHandler(nodeBox_MoveStarted);

                            panel1.Click        -= new EventHandler(AddNode);

                            break;
                        }
                    default: return;
                }
            }

            if (state == State.AddNode)
            {
                panel1.Click += new EventHandler(AddNode);
            }
        }
        
        private void resetSelection()
        {
            var nbs = panel1.Controls.OfType<NodeBox>().Cast<NodeBox>().Where(nb => nb.IsSelected == true).ToList();

            foreach(NodeBox nb in nbs)
            {
                nb.IsSelected = false;
                nb.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void nodeBox_MouseDown(object sender, MouseEventArgs e)
        {
            var nb = (NodeBox)sender;
            nb.IsDown = true;

            mouseOffset = e.Location;
        }

        private void nodeBox_MouseUp(object sender, MouseEventArgs e)
        {
            var nb = (NodeBox)sender;
            nb.IsDown = false;
        }

        private void nodeBox_MouseHover(object sender, EventArgs e)
        {
            var nb = (NodeBox)sender;
            
            if (!nb.IsSelected)
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
            
            //Удаление ребер для несуществующих связей
            foreach(Edge edge in listOfEdges.Where(_e => _e.listOfNodeBoxes.Contains(nb)).ToList())
            {
                listOfEdges.Remove(edge);
            }
            panel1.Invalidate();

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
            resetSelection();
        }

        private void AddNode(object sender, EventArgs e)
        {
            NodeBox nb = new NodeBox();

            // Вычисление точки, для помещения центра узла посередине курсора
            Point mousePosition = new Point(MousePosition.X - nb.CenterPoint.X, MousePosition.Y - nb.CenterPoint.Y);
            nb.Location = panel1.PointToClient(mousePosition);

            panel1.Controls.Add(nb);
        }

        private void nodeBox_OnPaint(object sender, PaintEventArgs e)
        {
            panel1.Invalidate(true);

            foreach (Edge edge in listOfEdges)
                g.DrawLine(new Pen(Color.Red, 3), edge.listOfNodeBoxes[0].CenterLocation, edge.listOfNodeBoxes[1].CenterLocation);
        }

        private void btnTopologyAddDependency_Click(object sender, EventArgs e)
        {
            state = State.Connect;

            btnTopologyAdd.Enabled = true;
            btnTopologyAddDependency.Enabled = false;
            btnTopologyDelete.Enabled = true;
            btnTopologySelect.Enabled = true;

            setNBEvents();
            resetSelection();
        }

        private void nodeBox_MouseClickOnConnect(object sender, MouseEventArgs e)
        {
            var nb = (NodeBox)sender;
            nb.IsSelected = !nb.IsSelected;
            
            if(nb.IsSelected)
            {
                nb.BorderStyle = BorderStyle.Fixed3D;
                selectedNBs.Add(nb);

                switch (selectedNBs.Count)
                {
                    case 0: { break; }
                    case 1: { break; }
                    case 2:
                        {
                            g.DrawLine(new Pen(Color.Red, 3), selectedNBs[0].CenterLocation, selectedNBs[1].CenterLocation);

                            selectedNBs[0].IsSelected = false;
                            //selectedNBs[0].listOfNbDependencies.Add(selectedNBs[1]);
                            selectedNBs[0].resetGraphicalSelection();

                            selectedNBs[1].IsSelected = false;
                            //selectedNBs[1].listOfNbDependencies.Add(selectedNBs[0]);
                            selectedNBs[1].resetGraphicalSelection();

                            listOfEdges.Add(new Edge(selectedNBs[0], selectedNBs[1]));

                            selectedNBs = new List<NodeBox>();
                            break;
                        }
                    default: { throw new Exception("Эксепшон!!!1"); }
                }
            }
            else
            {
                nb.BorderStyle = BorderStyle.FixedSingle;
                selectedNBs.Remove(nb);
            }
        }

        private void btnTopologySelect_Click(object sender, EventArgs e)
        {
            state = State.Select;

            btnTopologyAdd.Enabled = true;
            btnTopologyAddDependency.Enabled = true;
            btnTopologyDelete.Enabled = true;
            btnTopologySelect.Enabled = false;

            setNBEvents();
            resetSelection();
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

        private void nodeBox_MoveStarted(object sender, EventArgs e)
        {
            //var nb = (NodeBox)sender;
            //panel1.Refresh();

            //foreach (NodeBox _nb in nb.listOfNbDependencies)
            //    g.DrawLine(new Pen(Color.Red, 3), nb.CenterLocation, _nb.CenterLocation);

            //foreach(Edge edge in listOfEdges)
            //    g.DrawLine(new Pen(Color.Red, 3), edge.listOfNodeBoxes[0].CenterLocation, edge.listOfNodeBoxes[1].CenterLocation);

        }

        private void nodeBox_MoveFinished(object sender, MouseEventArgs e)
        {
            var nb = (NodeBox)sender;

            //while (notTruePosition)
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

        private void btnTopologyDelete_Click(object sender, EventArgs e)
        {
            state = State.Delete;

            btnTopologyAdd.Enabled = true;
            btnTopologyAddDependency.Enabled = true;
            btnTopologyDelete.Enabled = false;
            btnTopologySelect.Enabled = true;

            setNBEvents();
            resetSelection();
        }
    }
}
