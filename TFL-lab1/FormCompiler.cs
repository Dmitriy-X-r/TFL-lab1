using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public FormCompiler()
        {
            InitializeComponent();
            file = new File(this);
            correction = new Correction(this);
            reference = new Reference();
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
    }
}
