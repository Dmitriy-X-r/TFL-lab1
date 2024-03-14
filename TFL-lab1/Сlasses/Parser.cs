using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TFL_lab1.Interfaces;
using TFL_lab1.Сlasses;

namespace TFL_lab1
{
    class Parser : IParser
    {
        IScaner _scaner;
        List<PhpConstant> phpConst;
        RichTextBox _outputBox;
        int countError = 0;

        public Parser(IScaner scaner, RichTextBox outputBox)
        {
            _scaner = scaner;
            _outputBox = outputBox;
            phpConst = new List<PhpConstant>();
        }
        private void OutputMess(string massage)
        {
            _outputBox.AppendText(massage);
        }
        private void Clear()
        {
            _outputBox.Clear();
            phpConst.Clear();
        }
        public void Output()
        {
            for (int i = 0; i < phpConst.Count; i++)
            {
                OutputMess("Ошибки отсутсвуют\n");
            }
        }

        private bool NextWord(StructScanEnum type)
        {
            return _scaner.NextWordAndSkipSpace() && _scaner.lexem.StructScanEnum == type;
        }
        public void Parse()
        {
            Clear();
            countError = 0;
            _scaner.Clear();
            _scaner.Scan();
            bool result = true;
            while (_scaner.NextWordAndSkipSpace())
            {
                result = _scaner.lexem.StructScanEnum == StructScanEnum.Define;
                if (result)
                {
                    result = NewPhpConstant();
                    if (!result)
                    {
                        OutputMess(" cтрока: " + _scaner.curLine + " стобец: " + _scaner.curColumn + "\n");
                    }
                }
                else
                {
                    OutputMess("ожидается ключевое слово define cтрока: " + _scaner.curLine + " стобец: " + _scaner.curColumn + "\n");
                    countError++;
                    break;
                }
            }
            if (result)
                Output();
            OutputMess("Количество ошибок: " + countError);
        }
        private bool NewPhpConstant()
        {
            if (!NextWord(StructScanEnum.OpenBracket))
            {
                OutputMess("Ожидается открывающая скобка");
                countError++;
                return false;              
            }

            if (!NextWord(StructScanEnum.String))
            {
                OutputMess("Ожидается идентификатор");
                countError++;
                return false;
            }
            string name = _scaner.lexem.str.Trim('\"');
            
            if (!NextWord(StructScanEnum.Сomma))
            {
                OutputMess("Ожидается запятая");
                countError++;
                return false;
            }
            
            Type type;
            object value;
            if (_scaner.NextWordAndSkipSpace())
            {
                switch (_scaner.lexem.StructScanEnum)
                {
                    case StructScanEnum.Int:
                        type = typeof(int);
                        value = Convert.ToInt32(_scaner.lexem.str);
                        break;
                    case StructScanEnum.Float:
                        type = typeof(float);
                        value = Convert.ToSingle(_scaner.lexem.str.Replace(".", ","));
                        break;
                    case StructScanEnum.String:
                        type = typeof(string);
                        value = _scaner.lexem.str;
                        break;
                    case StructScanEnum.True:
                        type = typeof(bool);
                        value = true;
                        break;
                    case StructScanEnum.False:
                        type = typeof(bool);
                        value = false;
                        break;
                    case StructScanEnum.Null:
                        type = null;
                        value = null;
                        break;
                    default:
                        OutputMess("Ожидается значение");
                        countError++;
                        return false;
                }                    
            }
            else
            {
                OutputMess("Ожидается значение");
                countError++;
                return false;
            }

            if (!NextWord(StructScanEnum.CloseBracket))
            {
                OutputMess("Ожидается закрывающая скобка");
                countError++;
                return false;
            }

            if (NextWord(StructScanEnum.EndOperator))
            {
                phpConst.Add(new PhpConstant(name, type, value));
                return true;
            }
            else
            {
                OutputMess("Ожидается завершающая точка с запятой");
                countError++;
                return false;
            }           
        }
    }
}
