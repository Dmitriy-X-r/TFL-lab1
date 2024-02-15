using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFL_lab1.Interfaces
{
    internal interface IFile
    {
        void CreateFile();
        void OpenFile();
        void SaveFile();
        void SaveFileAs();
        void Exit();
    }
}
