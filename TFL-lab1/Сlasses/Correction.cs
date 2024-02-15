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
    internal class Correction : ICorrection
    {
        FormCompiler form;
        public Correction(FormCompiler form)
        {
            this.form = form;
        }
        public void Cancel()
        {
            form.InputTextBox.Undo();
        }

        public void Copy()
        {
            form.InputTextBox.Copy();
        }

        public void Cut()
        {
            form.InputTextBox.Cut();
        }

        public void Delete()
        {
            form.InputTextBox.Text = form.InputTextBox.Text.Remove(form.InputTextBox.SelectionStart, form.InputTextBox.SelectionLength);
        }

        public void Insert()
        {
            form.InputTextBox.Paste();
        }

        public void Repeat()
        {
            form.InputTextBox.Redo();
        }

        public void SelectAll()
        {
            form.InputTextBox.SelectAll();
        }
    }
}
