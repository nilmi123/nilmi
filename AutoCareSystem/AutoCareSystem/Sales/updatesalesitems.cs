﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AutoCareSystem
{
    public partial class updatesalesitems : UserControl
    {
        public updatesalesitems()
        {
            InitializeComponent();
            CodeGenerator cd = new CodeGenerator();

            String r_code = CodeGenerator.getLastInsertId("sales", "sales_id");
            lodeGridViewforsalesbySearch(r_code);

        }


        private void LodesalesGridView()
        {
            try
            {
                Database db = new Database();
                string query = "select s.sales_id AS'SalesID',si.item_id AS 'ItemID',sc.cus_id AS 'CustomerID',sc.cus_name AS'Name',sc.cus_address AS 'Address',sc.cus_telephone AS 'Telephone',si.quantity AS 'Quantity',s.total_price AS 'Total Price' ,si.discount AS 'Discount' ,si.price AS 'Unitprice' from sales s,sales_items si,salescustomer sc  where s.sales_id=si.sales_id and sc.cus_id=s.cus_id";

                db.sqlQuery(query);
                DataTable dt = db.executeQuery();
                SalesdataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                SalesdataGrid.DataSource = dt;
                db.getConnection().Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
        }



        private void lodeGridViewforsalesbySearch(String searchkey)
        {
            try
            {
                Database db = new Database();
                string query = "select s.sales_id AS'SalesID',si.item_id AS 'ItemID',sc.cus_id AS 'CustomerID',sc.cus_name AS'Name',sc.cus_address AS 'Address',sc.cus_telephone AS 'Telephone',si.quantity AS 'Quantity',s.total_price AS 'Total Price' ,si.discount AS 'Discount' ,si.price AS 'Unitprice' from sales s,sales_items si,salescustomer sc  where s.sales_id=si.sales_id and sc.cus_id=s.cus_id and s.sales_id='" + searchkey + "'";

                db.sqlQuery(query);
                DataTable dt = db.executeQuery();
                SalesdataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                SalesdataGrid.DataSource = dt;
                db.getConnection().Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
        }


        private void resetFields()
        {
            try
            {
                SalesdataGrid.ClearSelection();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            Nametxt.Text = string.Empty;
            Quntitytxt.Text = string.Empty;
            Telephonetxt.Text = string.Empty;
            Addresstxt.Text = string.Empty;
            itemidtxt.Text = string.Empty;
            Discounttxt.Text = string.Empty;

        }







        void updateSalesCustomer(string cusID, String Name,String Addres,string telephone)
        { 

            string query = "UPDATE salescustomer SET cus_name='" + Name + "',cus_address='" + Addres + "',cus_telephone ='" + telephone + "' WHERE cus_id = '" + cusID + "'";
            Database db = new Database();
            db.sqlQuery(query);
            db.nonQuery();
            db.getConnection().Close();
            String r_code = CodeGenerator.getLastInsertId("sales", "sales_id");
            lodeGridViewforsalesbySearch(r_code);
            resetFields();




        }

        void updateSales(string Salesid, float total)
        {

            string query = "UPDATE sales SET total_price='" + total + "' WHERE sales_id = '" + Salesid + "'";
            Database db = new Database();
            db.sqlQuery(query);
            db.nonQuery();
            db.getConnection().Close();
            String r_code = CodeGenerator.getLastInsertId("sales", "sales_id");
            lodeGridViewforsalesbySearch(r_code);
            resetFields();

        }







        private void SalesdataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            int selectedrowindex = SalesdataGrid.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = SalesdataGrid.Rows[selectedrowindex];

            itemidtxt.Text = Convert.ToString(selectedRow.Cells[1].Value);
            Nametxt.Text = Convert.ToString(selectedRow.Cells[3].Value);
            Addresstxt.Text = Convert.ToString(selectedRow.Cells[4].Value);
            Telephonetxt.Text = Convert.ToString(selectedRow.Cells[5].Value);
            Discounttxt.Text = Convert.ToString(selectedRow.Cells[8].Value);
            Quntitytxt.Text = Convert.ToString(selectedRow.Cells[6].Value);

            // loadSubGridView(Convert.ToString(selectedRow.Cells[0].Value));
        }

        private void TeleTextbox_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void btnRepairRemove_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Nametxt.Text) && string.IsNullOrWhiteSpace(Addresstxt.Text) && string.IsNullOrWhiteSpace(Quntitytxt.Text) && string.IsNullOrWhiteSpace(Telephonetxt.Text) && string.IsNullOrWhiteSpace(itemidtxt.Text))
            {
                MyDialog.Show("Error...!", "Plese fill all fields");
            }

            else
            {
                int selectedrowindex = SalesdataGrid.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = SalesdataGrid.Rows[selectedrowindex];

                string sid = Convert.ToString(selectedRow.Cells[0].Value);
                string Itemid = Convert.ToString(selectedRow.Cells[1].Value);
                string cusid = Convert.ToString(selectedRow.Cells[2].Value);
                float total = float.Parse((Convert.ToString(selectedRow.Cells[7].Value)));
                float Quntity = float.Parse((Convert.ToString(selectedRow.Cells[6].Value)));
                float unitprice = float.Parse((Convert.ToString(selectedRow.Cells[9].Value)));
                float discount = float.Parse((Convert.ToString(selectedRow.Cells[8].Value)));


                float tot = total;

                var confirmResult = MessageBox.Show("Are you sure to delete this item ??",
                                       "Confirm Delete!!",
                                       MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {

                    string query = "DELETE FROM sales_items WHERE item_id = '" + Itemid + "'";
                    Database db = new Database();
                    db.sqlQuery(query);
                    db.nonQuery();
                    db.getConnection().Close();
                   
                    resetFields();
                    tot = total - Quntity * unitprice;

                }



                updateSales(sid, tot);
                String r_code = CodeGenerator.getLastInsertId("sales", "sales_id");
                lodeGridViewforsalesbySearch(r_code);
                resetFields();




            }
        }

        private void btnRepairUpdate_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(Nametxt.Text) && string.IsNullOrWhiteSpace(Addresstxt.Text) && string.IsNullOrWhiteSpace(Quntitytxt.Text) && string.IsNullOrWhiteSpace(Telephonetxt.Text) && string.IsNullOrWhiteSpace(itemidtxt.Text))
            {
                MyDialog.Show("Error...!", "Plese fill all fields");
            }

            else
            {
                int selectedrowindex = SalesdataGrid.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = SalesdataGrid.Rows[selectedrowindex];

                string sid = Convert.ToString(selectedRow.Cells[0].Value);
                string Itemid = Convert.ToString(selectedRow.Cells[1].Value);
                string cusid = Convert.ToString(selectedRow.Cells[2].Value);
                float total = float.Parse((Convert.ToString(selectedRow.Cells[7].Value)));
                float Quntity = float.Parse((Convert.ToString(selectedRow.Cells[6].Value)));
                float unitprice = float.Parse((Convert.ToString(selectedRow.Cells[9].Value)));
                float discount = float.Parse((Convert.ToString(selectedRow.Cells[8].Value)));

                float tot = total - ((Quntity * unitprice) - discount);

                tot = tot + (((float.Parse(Quntitytxt.Text) * unitprice) - float.Parse(Discounttxt.Text)));


                //string query = "UPDATE Salesview set cus_name='" + Nametxt + "' ,cus_address='" + Addresstxt.Text + "',cus_telephone = '" + Telephonetxt.Text + "',Quantity='" + Quntitytxt.Text + "',total_price ='" + tot + "' ,discount='" + Discounttxt.Text + "' where item_id ='" + Itemid + "' ";
                //Database db = new Database();
                //db.sqlQuery(query);
                updateSalesCustomer(cusid, Nametxt.Text, Addresstxt.Text, Telephonetxt.Text);
                updateSales(sid, tot);
                string query = "UPDATE sales_items SET quantity='"+ float.Parse(Quntitytxt.Text)+ ",'discount='" + Convert.ToDecimal(Discounttxt.Text) + "' WHERE item_id = '" + Itemid + "'";
                Database db = new Database();
                db.sqlQuery(query);
                if (db.nonQuery())
                {
                    MyDialog.Show("Success...!", "Sales Charges updated");
                   
                    resetFields();
                    String r_code = CodeGenerator.getLastInsertId("sales", "sales_id");
                    lodeGridViewforsalesbySearch(r_code);

                }
                else
                {
                    MyDialog.Show("Error...!", "Sales Charges not updated");
                }
                db.getConnection().Close();

            }
        }

        private void btnRepairClear_Click(object sender, EventArgs e)
        {
            //decimal x = float.Parse(Discounttxt.Text);
           // txtTest.Text = Convert.ToString( x);
            resetFields();
        }

        private void itemidtxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back) || char.IsLetter(e.KeyChar);
        }

        private void Quntitytxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back) || char.IsLetter(e.KeyChar);
        }

        private void Discounttxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back) || char.IsLetter(e.KeyChar);
        }

        private void Telephonetxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back) || char.IsLetter(e.KeyChar);
        }

        private void Nametxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back) || char.IsNumber(e.KeyChar);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            new Sales_Bill().Show();
        }
    }
}
