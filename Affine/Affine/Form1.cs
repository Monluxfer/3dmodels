using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;

namespace Affine
{


    public enum Axis { AXIS_X, AXIS_Y, AXIS_Z, LINE };
    public enum Projection { PERSPECTIVE = 0, ISOMETRIC, ORTHOGR_X, ORTHOGR_Y, ORTHOGR_Z };
    public enum Clipping { Clipp = 0, ZBuffer, Gouraud, Texture, Graph};

    public partial class Form1 : Form
    {
        Graphics g;
        Polyhedron figure = null;
        double step = 0.2;
        int left_range = -6;
        int right_range = 6;

        Camera camera = new Camera(50,50);

        Color fill_color = Color.Red;
        byte[] rgbValuesTexture; // for picturebox and texture
        Bitmap texture;
        public Bitmap bmp;
        BitmapData bmpDataTexture; // for picturebox and texture
        public BitmapData bmpData;
        public IntPtr ptr; // pointer to the rgbValues
        public int bytes; // length of rgbValues

        Graphic Graph = null;

        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);


            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = pictureBox1.CreateGraphics();
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);



            texture = Image.FromFile("../../texture1.jpg") as Bitmap;
            Rectangle rectTexture = new Rectangle(0, 0, texture.Width, texture.Height);
            bmpDataTexture = texture.LockBits(rectTexture, ImageLockMode.ReadWrite, texture.PixelFormat);
            int bytesTexture = Math.Abs(bmpDataTexture.Stride) * texture.Height;
            rgbValuesTexture = new byte[bytesTexture];
            System.Runtime.InteropServices.Marshal.Copy(bmpDataTexture.Scan0, rgbValuesTexture, 0, bytesTexture);

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            Graph = new Graphic(comboBox2.SelectedIndex);

            figure = Graph;

            g.Clear(Color.White);
            Graph.isGraph = true;
            //Graph.Show(g, 0);
            Graph.picture = pictureBox1;
            Graph.DrawGraphic(step, left_range, right_range);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Graph != null)
            {
                if (e.KeyCode == Keys.A)
                    Graph.psi -= 10;
                else if (e.KeyCode == Keys.D)
                    Graph.psi += 10;
                else if (e.KeyCode == Keys.W)
                    Graph.phi -= 10;
                else if (e.KeyCode == Keys.S)
                    Graph.phi += 10;
                Graph.DrawGraphic(step, left_range, right_range);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e) { }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Graph != null)
            {
                Graph.psi += 2;
                Graph.DrawGraphic(step, left_range, right_range);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            step = double.Parse(textBox1.Text);
            left_range = int.Parse(textBox2.Text);
            right_range = int.Parse(textBox3.Text);
            Graph.DrawGraphic(step, left_range, right_range);
        }
    }
}