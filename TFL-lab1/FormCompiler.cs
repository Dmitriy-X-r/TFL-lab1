using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TFL_lab1.Interfaces;
using TFL_lab1.Сlasses;

namespace TFL_lab1
{
    public partial class FormCompiler : Form
    {
        IFile file;
        internal IFile File { get => file; }

        ICorrection correction;
        internal ICorrection Correction { get => correction; }

        IReference reference;
        internal IReference Reference { get => reference; }

        IScaner scaner;
        internal IScaner Scaner { get => scaner; }

        IParser parser;
        internal IParser Parser { get => parser; }


        public FormCompiler()
        {
            InitializeComponent();
            file = new TFL_lab1.Сlasses.File(this);
            correction = new Correction(this);
            reference = new Reference();
            scaner = new Scaner(this);
            parser = new Parser(scaner, OutputTextBox);
        }

        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file.CreateFile();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file.OpenFile();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file.SaveFile();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file.SaveFileAs();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file.Exit();
        }

        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            correction.Cancel();
        }

        private void RepeatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            correction.Repeat();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            correction.Cut();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            correction.Copy();
        }

        private void InsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            correction.Insert();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            correction.Delete();
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            correction.SelectAll();
        }

        private void FormCompiler_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
            file.Exit();
        }

        private void CallingHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reference.CallingHelp();
        }

        private void AboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reference.AboutProgram();
        }

        private void FormCompiler_DragEnter(object sender, DragEventArgs e)
        {
            // Проверка, что перетаскивается файл             if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        private void FormCompiler_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            parser.Parse();
        }
    }
}
