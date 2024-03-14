using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFL_lab1.Сlasses;

namespace TFL_lab1.Interfaces
{
    internal struct Lexem
    {
        internal Lexem(StructScanEnum StructScanEnum, string str, int start, int end)
        {
            this.StructScanEnum = StructScanEnum;
            this.str = str;
            this.start = start;
            this.end = end;
        }
        internal StructScanEnum StructScanEnum;
        internal string str;
        internal int start;
        internal int end;
    }
    internal interface IScaner
    {
        int curLine {  get; }
        int curColumn { get; }
        string buffer { get; }
        Lexem lexem { get; }
        void Scan();
        void Check();
        void Clear();
        bool NextWordAndSkipSpace();
        bool CheckProcess();
    }
}
