using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TFL_lab1.Interfaces;

namespace TFL_lab1.Сlasses
{
    internal class File : IFile
    {
        FormCompiler form;
        string currentFile = "";
        bool fileChanged = false;
        public File(FormCompiler form)
        {
            this.form = form;
            this.form.InputTextBox.TextChanged += new EventHandler((sender, earg) => { fileChanged = true; });
        }
        public void CreateFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files(*.txt) |*.txt| All files(*.*) | *.*";
            saveFileDialog.Title = "Создать файл";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
                streamWriter.Close();
                currentFile = saveFileDialog.FileName;
            }
        }
        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files(*.txt) |*.txt| All files(*.*) | *.*";
            openFileDialog.Title = "Открыть файл";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    string content = streamReader.ReadToEnd();
                    form.InputTextBox.Text = content;
                }
                currentFile = filePath;
            }
        }
        public void SaveFile()
        {
            fileChanged = false;
            using (StreamWriter streamWriter = new StreamWriter(currentFile))
            {
                streamWriter.Write(form.InputTextBox.Text);
            }
        }
        public void SaveFileAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files(*.txt) |*.txt| All files(*.*) | *.*";
            saveFileDialog.Title = "Сохранить файл как";
            saveFileDialog.FileName = currentFile.Split('\\').Last();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    streamWriter.Write(form.InputTextBox.Text);
                }
                currentFile = filePath;
            }
        }
        public void Exit()
        {
            if(CheckClose())
                Application.Exit();
        }
        internal bool CheckClose()
        {
            if (fileChanged)
            {
                DialogResult result = MessageBox.Show("Сохранить изменения в файле?", "Неприменённые изменения", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    return true;
                }
                else if (result == DialogResult.No)
                {
                    fileChanged = false;
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }
    }
}
