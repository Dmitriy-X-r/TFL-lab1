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
        internal struct Word
        {
            internal Word(StructScanEnum StructScanEnum, string str, int start, int end)
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

        StructStepEnum _step = StructStepEnum.Search;
        public string buffer { get => _buffer; }

        public void Check()
        {
            Clear();
            Scan();
            //Filter();
            CheckProcess();
        }
        private Word GetNextWord()
        {
            int start = _curChr;
            while (_curChr < _buffer.Length && Char.IsLetterOrDigit(_buffer[_curChr]))
                _curChr++;
            if (start == _curChr)
            {
                if (_curChr == _buffer.Length)
                    return new Word(StructScanEnum.NONE, "", start, _curChr);
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
                    return new Word(StructScanEnum.Space, subString, start, end);
                }
                else
                    return new Word(StructScanEnum.ERROR, subString, start, end);
            }

            if(subString == "\"" && !_searchPP)
            {
                Word word = GetNextWord();
                _searchPP = true;
                while(!(word.StructScanEnum == StructScanEnum.NONE) && word.str != "\"")
                    word = GetNextWord();
                _searchPP= false;
                if (word.StructScanEnum == StructScanEnum.ERROR)
                {
                    subString = buffer.Substring(start, word.end - start);
                    return new Word(StructScanEnum.String, subString, start, word.end);
                }
                else
                    OutputError("Ожидается \"");
            }

            if (Char.IsNumber(subString[0]))
            {
                //Word word = GetNextWord();
                //bool point = false;
                //while (!(word.StructScanEnum == StructScanEnum.NONE))
                //    if (word.str == ".")
                //    {
                //        point = true;
                //        break;
                //        //error
                //    }
                //word = GetNextWord();
                //if (word.StructScanEnum == StructScanEnum.ERROR)
                //{
                //    subString = buffer.Substring(start, word.end - start);
                //    return new Word(StructScanEnum.Int, subString, start, word.end);
                //}
                //else
                //    OutputError("Ожидается \"");
                StructScanEnum type = StructScanEnum.Int;
                if(_curChr < buffer.Length && _buffer[_curChr] == '.' && !_searcNum)
                {
                    _searcNum = true;
                    type = StructScanEnum.Float;
                    _curChr++;
                    Word num = GetNextWord();
                    if(num.StructScanEnum!= StructScanEnum.Int)
                    {
                        subString += '.' + num.str;
                        end = num.end;
                    }
                    else 
                    {
                        return num;
                    }
                }
                _searcNum = false;
                return new Word(type, subString, start, end);
            }

            for (int i = 1; i < structScanDescription.GetLength(0)-3; i++)
            {
                if (subString == structScanDescription[i, 1])
                    return new Word((StructScanEnum)i, subString, start, end);
            }
            return new Word(StructScanEnum.ERROR, subString, start, end); // может не так
        }
        private void NextWordAndSkipSpace(ref Word word)
        {
            do
            {
                word = GetNextWord();
                OutputResult(word);
            } while (word.StructScanEnum == StructScanEnum.Space && (word.StructScanEnum != StructScanEnum.NONE || word.StructScanEnum == StructScanEnum.ERROR));
        }
        private void CheckProcess()
        {
            _step = StructStepEnum.Search;
            int levelDeep = 0;
            Word word;
            do
            {
                word = GetNextWord();
                OutputResult(word);
            } while (word.StructScanEnum != StructScanEnum.NONE && word.StructScanEnum != StructScanEnum.ERROR);
            if (word.StructScanEnum == StructScanEnum.ERROR)
                OutputError("Ошибка ликсемы - " + word.str);
        }

        private void OutputResult(Word word)
        {
            if (word.StructScanEnum == StructScanEnum.ERROR || word.StructScanEnum == StructScanEnum.NONE)
                return;
            if (_form.OutputTextBox.Text.Length > 0)
                _form.OutputTextBox.AppendText("\n");
            if (word.str == " ")
                word.str = "(пробел)";
            else if (word.str == "\n")
                word.str = "(конец строки)";
            else if (word.str == "\t")
                word.str = "(табуляция)";
            _form.OutputTextBox.AppendText($"{(int)word.StructScanEnum} - {structScanDescription[(int)word.StructScanEnum, 0]} - {word.str} - c {word.start} по {word.end} символ");
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
