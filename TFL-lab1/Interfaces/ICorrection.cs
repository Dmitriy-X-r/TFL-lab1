using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFL_lab1.Interfaces
{
    internal interface ICorrection
    {
        void Cancel();
        void Repeat();
        void Cut();
        void Copy();
        void Insert();
        void Delete();
        void SelectAll();
    }
}
