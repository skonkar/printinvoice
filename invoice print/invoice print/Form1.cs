using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace invoice_print
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // this is to load the form into focusing on textbox for name
            txtname.Focus();
            txtname.Select();
            //==============================================================
            //region date config
            txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //======================================================================
            //dictionary made for combo box which you can attach two variables (maybe 3)
            Dictionary<int,string> itemdata = new Dictionary<int,string>();
            itemdata.Add(310, "Hp");
            itemdata.Add(220, "dell");
            itemdata.Add(320, "razor");
            itemdata.Add(20, "apple");

            cbxitem.DataSource = new BindingSource(itemdata,null);
            cbxitem.DisplayMember = "value";
            cbxitem.ValueMember = "key";
            txtprice.Text = cbxitem.SelectedValue.ToString();
            
            //this is for coloring the columns
            foreach(DataGridViewColumn col in dataGridView1.Columns)
            {
                col.DefaultCellStyle.ForeColor = Color.Navy;
            }
            //extra coloring
//          dataGridView1.Columns[1].DefaultCellStyle.ForeColor = Color.Red;
            dataGridView1.Columns[3].DefaultCellStyle.ForeColor = Color.DarkGreen;
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            //this is to prevent form right clicking
            if (e.Button == MouseButtons.Right)
            {
                txtdate.ContextMenuStrip = new ContextMenuStrip();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtsum_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtsum_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void txtsum_MouseDown(object sender, MouseEventArgs e)
        {
            //this is to avoid right clicking
            if (e.Button == MouseButtons.Right)
            {
                txtsum.ContextMenuStrip = new ContextMenuStrip();
            }
        }

        private void txtname_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void txtname_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void txtname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtqty_KeyDown(object sender, KeyEventArgs e)
        {
            //this is To preform clickin on the add button
            if (e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }

        }

        private void cbxitem_KeyDown(object sender, KeyEventArgs e)
        {
            //this is to move to the next text box
            if (e.KeyData == Keys.Enter)
            {
                txtprice.Focus();
                txtprice.Select();
            }
        }

        private void txtname_KeyDown_1(object sender, KeyEventArgs e)
        {

            if(e.KeyData == Keys.Enter)
            {
                cbxitem.Focus();
                cbxitem.Select();
            }
        }

        private void txtprice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtqty.Focus();
                txtqty.Select();
            }
        }

        private void cbxitem_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtprice.Text = cbxitem.SelectedValue.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // this to prevent from adding invalid items
            if (cbxitem.SelectedIndex <= -1) return;
            //this is to add the info gathered from txtboxes to the data grid view
            string item = cbxitem.Text;
            int qty = Convert.ToInt32(txtqty.Text);
            int price = Convert.ToInt32(txtprice.Text);
            int sum = qty * price;
            object[] row = { item, qty, price, sum };
            dataGridView1.Rows.Add(row);
            txtsum.Text = (Convert.ToInt32(txtsum.Text) + sum)+"";
            cbxitem.Focus();
            cbxitem.Select();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this if you want to make the pop-up form maximized.
            ((Form)printPreviewDialog1).WindowState =FormWindowState.Maximized;
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //this is made for print page config
            //logo implementing
            float margin = 40;
            e.Graphics.DrawImage(Properties.Resources.logo, margin, margin, 200, 200);
            //font config
            Font f = new Font("Arial", 18, FontStyle.Bold);
            
            //variables taken from the form txtboxes
            string strno = "#NO. " + txtNo.Text;
            string strdate = "في تاريخ: " + txtdate.Text;
            string strname = "مطلوب من السيد: " + txtname.Text;

            //this is used later for making preheight variable
            SizeF fontsizeno = e.Graphics.MeasureString(strno, f);
            SizeF fontsizedate = e.Graphics.MeasureString(strdate, f);
            SizeF fontsizename = e.Graphics.MeasureString(strname, f);


            //this is for adding variables above to the print page
            e.Graphics.DrawString(strno, f, Brushes.Red, (e.PageBounds.Width - fontsizeno.Width) / 2, margin);
            e.Graphics.DrawString(strdate, f, Brushes.Black, e.PageBounds.Width - fontsizedate.Width - margin, margin + fontsizeno.Height);
            e.Graphics.DrawString(strname, f, Brushes.Navy, e.PageBounds.Width - fontsizename.Width - margin, margin + fontsizedate.Height + fontsizeno.Height);

            float preheights = margin + fontsizeno.Height + fontsizedate.Height + fontsizename.Height+110;

            //the table config
            e.Graphics.DrawRectangle(Pens.Black, margin, preheights, e.PageBounds.Width - margin * 2,e.PageBounds.Height-margin-preheights);

            float colheight = 60;
            float col1width = 250;
            float col2width = col1width + 125;
            float col3width = col2width+ 125;
            float col4width = col3width + 125;

            e.Graphics.DrawLine(Pens.Black, margin, preheights + colheight, e.PageBounds.Width - margin,preheights +colheight);
            
            //e.Graphics.DrawString("name",f,color,x<width-margin*2-col1width,y<preheights>>
            //e.Graphics.DrawLine(pens.color,xright <width-margin*2-col1width>,ytop <preheights>,xleft <width-margin*2-col1width>,ydown<pagebounds.height-margin>>
            e.Graphics.DrawString("الصنف", f, Brushes.Black, e.PageBounds.Width - margin * 2-col1width, preheights);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - margin * 2 - col1width, preheights, e.PageBounds.Width - margin * 2 - col1width, e.PageBounds.Height -margin);

            e.Graphics.DrawString("الكمية", f, Brushes.Black, e.PageBounds.Width - margin * 2 - col2width, preheights);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - margin * 2 - col2width, preheights, e.PageBounds.Width - margin * 2 - col2width, e.PageBounds.Height - margin);
            
            e.Graphics.DrawString("السعر", f, Brushes.Black, e.PageBounds.Width - margin * 2 - col3width, preheights);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - margin * 2 - col3width, preheights, e.PageBounds.Width - margin * 2 - col3width, e.PageBounds.Height - margin);

            //except for the last column cuz it doesn't need another line
            e.Graphics.DrawString("اجمالي الفرعي", f, Brushes.Black, e.PageBounds.Width - margin * 3 - col4width, preheights);
            //..invoice content//
            //allow user to add rows =false (a7a)
            float rowsheight = 60;
            for (int x = 0 ; x < dataGridView1.Rows.Count; x += 1)
            {
                e.Graphics.DrawString(dataGridView1.Rows[x].Cells[0].Value.ToString(), f, Brushes.Navy, e.PageBounds.Width - margin - col1width, preheights + rowsheight);
                e.Graphics.DrawString(dataGridView1.Rows[x].Cells[1].Value.ToString(), f, Brushes.Navy, e.PageBounds.Width - margin - col2width, preheights + rowsheight);
                e.Graphics.DrawString(dataGridView1.Rows[x].Cells[2].Value.ToString(), f, Brushes.Navy, e.PageBounds.Width - margin - col3width, preheights + rowsheight);
                e.Graphics.DrawString(dataGridView1.Rows[x].Cells[3].Value.ToString(), f, Brushes.Navy, e.PageBounds.Width - margin - col4width, preheights + rowsheight);

                e.Graphics.DrawLine(Pens.Black, margin, preheights + rowsheight + colheight, e.PageBounds.Width - margin, preheights + rowsheight+colheight);

                rowsheight += 60;
            }
                e.Graphics.DrawString("الإجمالي الكلي", f, Brushes.Red, e.PageBounds.Width - margin*2- col3width, preheights + rowsheight);
                e.Graphics.DrawString(txtsum.Text, f, Brushes.Navy, e.PageBounds.Width - margin- col4width, preheights + rowsheight);
            e.Graphics.DrawLine(Pens.Black, margin, preheights + rowsheight + colheight, e.PageBounds.Width - margin, preheights + rowsheight + colheight);


        }
    }

}