using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brownian_Motion_Visualisation.Forms
{
    public partial class Particle : UserControl
    {
        public static int ParticleSize { get; set; } = 150;
        public static int Temperature { get; set; } = 0;
        private static Random rand = new Random();
        private int _moveSpeedX = 1, _moveSpeedY = 1;
        

        public Particle(int startPosX = 0, int startPosY = 0)
        {
            InitializeComponent();

            // Ниже приведены мои попытки сделать убрать квадрат на фоне, но ни один способ не сработал

            /* 1)
             * SetStyle(ControlStyles.SupportsTransparentBackColor, true);
             * this.BackColor = Color.Transparent;
             * 
             * 2)
             * GraphicsPath path = new GraphicsPath();
             * path.AddEllipse(this.ClientRectangle);
             * Region rgn = new Region(path);
             * this.Region = rgn;
             * this.BorderStyle = BorderStyle.None;
            */
                       
            this.ClientSize = new Size(new Point(ParticleSize, ParticleSize));
            this.Location = new Point(startPosX, startPosY);
        }
        public Particle()
        {
            InitializeComponent();
            this.ClientSize = new Size(new Point(ParticleSize + 2, ParticleSize + 2));
            this.Location = new Point(0, 0);
            this.Paint += new PaintEventHandler(PaintParticle);

            //это работает :)
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(this.ClientRectangle);
            Region rgn = new Region(path);
            this.Region = rgn;
            this.BorderStyle = BorderStyle.None;
        }

        protected override void OnPaint(PaintEventArgs e) => PaintParticle(this, e);
        private void PaintParticle(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(this.BackColor);

            e.Graphics.FillEllipse(Brushes.Red,
                0, 0,
                ParticleSize, ParticleSize);

            e.Graphics.DrawEllipse(Pens.Black,
                0, 0,
                ParticleSize, ParticleSize);
        }

        public void MoveParticle()
        {
            int x = this.Location.X, y = this.Location.Y;
            if (x - 2 < 0 || x + ParticleSize + 2 > this.Parent.ClientSize.Width) _moveSpeedX = -_moveSpeedX;
            x += _moveSpeedX;
            if (y - 2 < 0 || y + ParticleSize + 2 > this.Parent.ClientSize.Height) _moveSpeedY = -_moveSpeedY;
            y += _moveSpeedY;

            this.Location = new Point(x, y);
        }
    }
}
