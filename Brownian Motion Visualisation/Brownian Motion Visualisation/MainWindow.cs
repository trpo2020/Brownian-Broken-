using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Brownian_Motion_Visualisation.Forms;

namespace Brownian_Motion_Visualisation
{
    public partial class MainWindow : Form
    {
        private Thread[] _threads;
        public MainWindow()
        {
            InitializeComponent();
            Application.EnableVisualStyles(); // Думал это поможет убрать квадрат на фоне молекулы.
        }

        public void StopBtn_Click(object sender, EventArgs e)
        {
            NumOfMolecules.Enabled = true;
            StartBtn.Enabled = true;
            StopBtn.Enabled = false;
        }
        public void StartBtn_Click(object sender, EventArgs e)
        {
            NumOfMolecules.Enabled = false;
            StartBtn.Enabled = false;
            StopBtn.Enabled = true;

            /* Вот тут начинаются проблемы: 
             * 1) Я создаю и запускаю потоки как и в лекции, но они почему-то работают последовательно, 
             * никогда не передавая друг другу контроль.
             * 2) Если запустить код как есть, то сначала существующая молекула реально движется, а потом та, которую мы 
             * создаем динамически просто через какое-то время появляется в начальной точке, хотя методы одинаковые.
             * 3) Посмотрите как деформируется форма с настройками из-за движения существующей молекулы через нее. 
             * Она восстанавливается по завершению всего движения, но это же все-равно ненормально. Это можно убрать как-то?
             * 4) Так же видно что форма квадратная. Это можно изменить? В реализации класса Particle я пытался это убрать, но не вышло.
             */
            var thread1 = new Thread(ControlExistingMolecule);
            thread1.Start();

            var thread2 = new Thread(ControlNewMolecule);
            thread2.Start();
        }
        public void TempBar_Scroll(object sender, EventArgs e)
        {
            TempLbl.Text = TempBar.Value.ToString() + " K";
        }

        public void ControlExistingMolecule()
        {
            if (this.InvokeRequired) this.Invoke(new MethodInvoker(ControlExistingMolecule));
            else
            {
                for (int i = 0; i < 300; ++i)
                {
                    ExistingMolecule.MoveParticle();
                    Thread.Yield(); // это должно передать управление другому потоку, но ничего не меняется.
                    Thread.Sleep(10);
                }
            }
        }
        public void ControlNewMolecule()
        {
            if (this.InvokeRequired) this.Invoke(new MethodInvoker(ControlNewMolecule));
            else
            {

                var mol = new Particle();
                this.Controls.Add(mol);
                for (int i = 0; i < 1000; ++i)
                {
                    mol.MoveParticle();
                    Thread.Yield(); // это должно передать управление другому потоку, но ничего не меняется.
                    Thread.Sleep(10);
                }
            }
        }
    }
}