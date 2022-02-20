using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; //omogucuje citanje i pisanje u file

namespace Notepad_IvanMornar
{
    public partial class Form1 : Form
    {
        //Dialogs
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private FontDialog fontDialog;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fontDialog = new FontDialog(); //vamo da se alocira jednom a ne vise puta
        }

        //Creates a new file
        private void NewFile()
        {
            try
            {
                if(!string.IsNullOrEmpty(this.richTextBox1.Text))
                {
                    DialogResult d = MessageBox.Show("Do you want to save the file", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //MessageBox.Show("You need to save first!");
                    if(d.Equals(DialogResult.Yes))
                    {
                        SaveFile();
                    }
                    else
                    {
                        this.richTextBox1.Text = string.Empty;
                        this.Text = "Untitled";
                    }
                }
                else
                {
                    this.richTextBox1.Text = string.Empty;
                    this.Text = "Untitled";
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
        }

        private void OpenFile()
        {
            try
            {
                openFileDialog = new OpenFileDialog();

                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.richTextBox1.Text = File.ReadAllText(openFileDialog.FileName);
                    this.Text = openFileDialog.FileName;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error, can't open file!");
            }
            finally
            {
                openFileDialog = null;
            }
        }

        private void SaveFile()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.richTextBox1.Text) && this.Text != "Notepad" && this.Text != "Untitled")//ako je vec negdi Save-an da
                {                                                                                                      //neradimo save u novi file
                    File.WriteAllText(this.Text, this.richTextBox1.Text);
                    MessageBox.Show("Content is saved to the existing file.\nIf you want to save it to new file go to File->SaveAs","Info");
                }
                else if (!string.IsNullOrEmpty(this.richTextBox1.Text))
                {
                    saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text File (*.txt) | *.txt";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(saveFileDialog.FileName, this.richTextBox1.Text);
                        this.Text = saveFileDialog.FileName;
                    }
                }
                else
                {
                    MessageBox.Show("The file is empty!");
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
        }

        private void SaveFileAs()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.richTextBox1.Text))
                {
                    saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text File (*.txt) | *.txt | All Files (*.*)| *.*";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(saveFileDialog.FileName, this.richTextBox1.Text);
                        this.Text = saveFileDialog.FileName;
                    }
                }
                else
                {
                    MessageBox.Show("The file is empty!");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.richTextBox1.Text))
            {
                DialogResult d = MessageBox.Show("Do you want to save the file first", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
               
                if (d.Equals(DialogResult.Yes))
                {
                    SaveFile();
                }
            }
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(this.richTextBox1.Text))
                {
                    DialogResult d = MessageBox.Show("Do you want to save the file", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (d.Equals(DialogResult.Yes))
                    {
                        SaveFile();
                        this.Close();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if(fontDialog.ShowDialog() == DialogResult.OK)
                {
                    this.richTextBox1.Font = fontDialog.Font;
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(frmAbout frm = new frmAbout()) //using osigurava da se unisti frm nakon izlaska iz bloka
            {
                frm.ShowDialog();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string[] words = txtSearch.Text.Split(','); //niz stringova
            foreach(string word in words)
            {
                int startIndex = 0;
                while(startIndex < richTextBox1.TextLength)
                {
                    int wordStartIndex = richTextBox1.Find(word, startIndex, RichTextBoxFinds.None);
                    if (wordStartIndex != -1) //ako funkcija find ne nade rijec vraca -1
                    {
                        richTextBox1.SelectionStart = wordStartIndex;
                        richTextBox1.SelectionLength = word.Length;
                        richTextBox1.SelectionBackColor = Color.Orange;
                    }
                    else
                        break;
                    startIndex += wordStartIndex + word.Length;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.White;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(richTextBox1.CanUndo == true)
            {
                richTextBox1.Undo();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanRedo == true)
            {
                richTextBox1.Redo();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(richTextBox1.SelectionLength > 0)
            {
                richTextBox1.Copy();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(richTextBox1.SelectedText != "")
            {
                richTextBox1.Cut();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)//provjerava jeli to sta zelimo kopirati tekst
            {
                richTextBox1.Paste();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Select();
        }

        private void dateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            //richTextBox1.Text += dt.ToString();
            richTextBox1.Text = richTextBox1.Text.Insert(richTextBox1.SelectionStart, dt.ToString());
        }
    }
}
