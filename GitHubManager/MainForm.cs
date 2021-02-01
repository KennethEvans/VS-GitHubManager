using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitHubManager {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }

        private void textBox_TextChanged(object sender, EventArgs e) {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }

        private void OnClearClick(object sender, EventArgs e) {
            textBox.Clear();
        }

        private void OnExitClick(object sender, EventArgs e) {
            Close();
        }
    }
}
