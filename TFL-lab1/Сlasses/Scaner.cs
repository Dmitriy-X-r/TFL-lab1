using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFL_lab1.Interfaces;
using TFL_lab1.Сlasses;

namespace TFL_lab1.Сlasses
{
    internal class Scaner : IScaner
    {   
        internal Lexem _lexem;
        public Lexem lexem { get => _lexem; }
        FormCompiler _form;
        string _buffer;
        int _curLine = 0;
        int _curColumn = 0;
        int _curChr = 0;
        
        private string[,] structScanDescription = { { "", ""},
                                                    { "открывающая скобка", "(" },
                                                    { "закрывающая скобка", ")" },
                                                    { "запятая", "," },
                                                    { "ключевое слово", "true" },
                                                    { "ключевое слово", "false" },
                                                    { "ключевое слово", "null" },
                                                    { "ключевое слово", "define" },
                                                    { "разделитель", " " }, 
                                                    { "знак минус", "-" },
                                                    { "конец оператора", ";" },
                                                    { "целое число", "" },
                                                    { "вещественное число", "" },
                                                    { "строка", "" }};
        char[] _spaceChars = { ' ', '\n', '\t' };
        char[] _specialChars = { '(', ';', ')', '\"', ',' };
        bool _searchPP = false;
        bool _searcNum = false;
        internal Scaner(FormCompiler form)
        {
            _form = form;
        }

        public string buffer { get => _buffer; }

        public int curLine => _curLine;

        public int curColumn => _curColumn;

        public void Check()
        {
            Clear();
            Scan();
            CheckProcess();
        }
        private bool GetNextWord()
        {
            int start = _curChr;
            while (_curChr < _buffer.Length && Char.IsLetterOrDigit(_buffer[_curChr]))
                _curChr++;
            if (start == _curChr)
            {
                if (_curChr == _buffer.Length)
                {
                    _lexem =  new Lexem(StructScanEnum.NONE, "", start, _curChr);
                    return false;
                }
                else
                    _curChr += 1;
            }
            int end = _curChr;
            string subString = _buffer.Substring(start, end - start);
            _curColumn += subString.Length;

            if (subString.Length == 1 && !(Char.IsLetter(subString[0]) || _specialChars.Contains(subString[0])))
            {
                if (_spaceChars.Contains(subString[0]))
                {
                    if (subString[0] == '\n')
                    {
                        _curLine++;
                        _curColumn = 0;
                    }
                    else
                        _curColumn++;
                    _lexem = new Lexem(StructScanEnum.Space, subString, start, end);
                    return true;
                }
                else
                {
                    _lexem = new Lexem(StructScanEnum.ERROR, subString, start, end);
                    return false;
                }
            }

            if(subString == "\"" && !_searchPP)
            {
                GetNextWord();
                _searchPP = true;
                while(!(_lexem.StructScanEnum == StructScanEnum.NONE) && _lexem.str != "\"")
                    GetNextWord();
                _searchPP= false;
                if (_lexem.StructScanEnum == StructScanEnum.ERROR)
                {
                    subString = buffer.Substring(start, _lexem.end - start);
                    _lexem = new Lexem(StructScanEnum.String, subString, start, _lexem.end);
                    return true;
                }
                else
                {
                    OutputError("Ожидается \"");
                    return false;
                }
            }

            if (Char.IsNumber(subString[0]))
            {
                StructScanEnum type = StructScanEnum.Int;
                if(_curChr < buffer.Length && _buffer[_curChr] == '.' && !_searcNum)
                {
                    _searcNum = true;
                    type = StructScanEnum.Float;
                    _curChr++;
                    GetNextWord();
                    if (buffer[_curChr] == ')')
                        _curChr--;
                    if(_lexem.StructScanEnum!= StructScanEnum.Int)
                    {
                        if (_lexem.str != ")")
                            subString += '.' + _lexem.str;
                        else _curChr--;
                        end = _lexem.end;
                    }
                    else 
                    {
                        return true;
                    }
                }
                _searcNum = false;
                _lexem = new Lexem(type, subString, start, end);
                return true;
            }

            for (int i = 1; i < structScanDescription.GetLength(0)-3; i++)
            {
                if (subString == structScanDescription[i, 1])
                {
                    _lexem = new Lexem((StructScanEnum)i, subString, start, end);
                    return true;
                }
            }
            _lexem = new Lexem(StructScanEnum.ERROR, subString, start, end); // может не так
            return false;
        }
        public bool CheckProcess()
        {
            do
            {
                GetNextWord();
                OutputResult();
            } while (_lexem.StructScanEnum != StructScanEnum.NONE && _lexem.StructScanEnum != StructScanEnum.ERROR);
            if (_lexem.StructScanEnum == StructScanEnum.ERROR)
            {
               // OutputError("Ошибка ликсемы - " + _lexem.str);
                return false;
            }
            return true;
        }

        public bool NextWordAndSkipSpace()
        {
            do
            {
                GetNextWord();
                if (_lexem.StructScanEnum == StructScanEnum.NONE)
                    return false;
                else if (_lexem.StructScanEnum != StructScanEnum.Space || _lexem.StructScanEnum == StructScanEnum.ERROR)
                    return true;
            } while (true);
        }


        private void OutputResult()
        {
            if (_lexem.StructScanEnum == StructScanEnum.ERROR || _lexem.StructScanEnum == StructScanEnum.NONE)
                return;
            if (_form.OutputTextBox.Text.Length > 0)
                _form.OutputTextBox.AppendText("\n");
            if (_lexem.str == " ")
                _lexem.str = "(пробел)";
            else if (_lexem.str == "\n")
                _lexem.str = "(конец строки)";
            else if (_lexem.str == "\t")
                _lexem.str = "(табуляция)";
            _form.OutputTextBox.AppendText($"{(int)_lexem.StructScanEnum} - {structScanDescription[(int)_lexem.StructScanEnum, 0]} - {_lexem.str} - c {_lexem.start} по {_lexem.end} символ");
        }

        private void OutputError(string massage)
        {
            _form.OutputTextBox.AppendText(massage);
        }

        public void Clear()
        {
            _buffer = "";
            _curLine = 0;
            _curColumn = 0;
            _curChr = 0;
            _form.OutputTextBox.Clear();
        }

        public void Scan()
        {
            _buffer = (string)_form.InputTextBox.Text.Clone();
        }
    }
}
