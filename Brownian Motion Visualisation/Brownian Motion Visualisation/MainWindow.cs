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

            //класс Thread не очень хорошо работает в оконных приложениях
            //в C# есть несколько способов организации парраллельных вычислений, например, Task Parallel Library
            //Task - задача, запускаемая в одном из потоков пула потоков
            //эта часть https://metanit.com/sharp/tutorial/12.5.php может Вам
            //понадобиться для отмены задачи
            var ControlExistingMoleculeTask = Task.Run(() => ControlExistingMolecule());
            var ControlNewMoleculeTask = Task.Run(() => ControlNewMolecule());
        }
        public void TempBar_Scroll(object sender, EventArgs e)
        {
            TempLbl.Text = TempBar.Value.ToString() + " K";
        }

        public void ControlExistingMolecule()
        {
            while (true)
            {
                Thread.Sleep(10);
                ExistingMolecule.Invoke(new MethodInvoker(() => { ExistingMolecule.MoveParticle(); }));
            }
        }
        public void ControlNewMolecule()
        {
            var mol = new Particle();
            mol.Location = new Point(2, 2); //если взять меньше 2, молекула будет "дрожать" на месте
            //причина - в реализации метода MoveParticle
            this.Invoke(new MethodInvoker(() => { this.Controls.Add(mol); }));
            while (true)
            {
                Thread.Sleep(10);
                mol.Invoke(new MethodInvoker(() => { mol.MoveParticle(); }));
            }
        }
    }
}
