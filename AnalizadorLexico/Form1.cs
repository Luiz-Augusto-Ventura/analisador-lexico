using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalizadorLexico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAnalisar_Click(object sender, EventArgs e)
        {
            txtTokens.Clear();
            Lexer lex = new Lexer(txtFonte.Text);

            string token = "";
            while(token != "<end>")
            {
                token = lex.scan();
                txtTokens.Text += token;
            }

            dataGridView1.DataSource = lex.listaSimbolos;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                txtFonte.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
        }
    }
}
