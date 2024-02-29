using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFL_lab1.Interfaces;
using TFL_lab1.Enums;

namespace TFL_lab1.Сlasses
{
    internal class Scaner : IScaner
    {
        FormCompiler _form;
        string _buffer;
        int _curLine = 0;
        int _curColumn = 0;
        int _curChr = 0;
        
        private string[,] StructScanDescription = { { "ошибка", ""},
                                                    { "открывающая скобка", "(" },
                                                    { "закрывающая скобка", ")" },
                                                    { "строка", "" }, 
                                                    { "запятая", "," },
                                                    { "ключевое слово", "true" },
                                                    { "ключевое слово", "false" },
                                                    { "ключевое слово", "null" },
                                                    { "ключевое слово", "define" },
                                                    { "разделитель", " " },
                                                    { "целое число", "" }, 
                                                    { "вещественное число", "" }, 
                                                    { "знак минус", "-" },
                                                    { "конец оператора", ";" },};
        internal Scaner(FormCompiler form)
        {
            _form = form;
        }
      
        public string buffer { get => _buffer; }

        public void Check()
        {
            Clear();
            Scan();
            //Filter();
            CheckProcess();
        }
        private (StructScanEnum StructScanEnum, string str, int start, int end) GetNextWord()
        {

        }
        private void CheckProcess()
        {

        }

        private void OutputResult(int code, string description, string str, int start, int end)
        {
            if (_form.OutputTextBox.Text.Length > 0)
                _form.OutputTextBox.AppendText("\n");
            if (str == " ")
                str = "(пробел)";
            _form.OutputTextBox.AppendText($"{code} - {description} - {str} - c {start} по {end} символ");
        }

        private void OutputError(string massage)
        {
            _form.OutputTextBox.AppendText("Ошибка лексемы " + massage);
        }

        public void Clear()
        {
            _buffer = "";
            int _curLine = 0;
            int _curColumn = 0;
            int _curChr = 0;
            _form.OutputTextBox.Clear();
        }

        public void Filter()
        {
            _buffer.Trim('\n');
            _buffer.Trim('\t');
        }

        public void Scan()
        {
            _buffer = (string)_form.InputTextBox.Text.Clone();
        }
    }
}
