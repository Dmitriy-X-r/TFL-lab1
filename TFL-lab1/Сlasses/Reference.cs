using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TFL_lab1.Interfaces;

namespace TFL_lab1.Сlasses
{
    internal class Reference : IReference
    {
        public void AboutProgram()
        {
            FormAboutProgram form = new FormAboutProgram();
            form.Show();
        }

        public void CallingHelp()
        {
            FormHelp form = new FormHelp();
            form.Show();
        }
    }
}
