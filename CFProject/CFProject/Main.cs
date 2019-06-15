﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFProject
{
    public partial class Main : Form
    {
        private TaiKhoan tk;
        public Main()
        {
            InitializeComponent();
        }

        public Main(TaiKhoan t)
        {
            InitializeComponent();
            this.tk = t;
            lblNameAcc.Text = t.TenDangNhap;
        }

        #region MenuStrip
        private void thôngTinToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Quản lí quán cafe v1.0\n1612203 - 1612209\nKhoa CNTT - ĐH KHTN - ĐHQGTPHCM", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void quảnLíToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control frmControl = new Control();
            this.Hide();
            frmControl.ShowDialog();
            this.Show();
        }
        #endregion

        #region Table

        int tableIndex = 1;
        public void LoadDataTable()
        {
            int xLocation = 5;
            int yLocation = 30;
            List<BanAn> lt;
            using (var db = new QLCafeEntities())
            {
                lt = db.BanAns.ToList();
            }

            foreach (var table in lt)
            {
                Button btn = new Button();
                btn.Name = string.Format("Button{0}", table.MaBan);
                btn.Text = table.TenBan;
                btn.Location = new System.Drawing.Point(xLocation, yLocation);
                btn.Size = new System.Drawing.Size(100, 100);
                if (table.TinhTrang=="Trống")
                {
                    btn.BackColor = Color.Brown;
                }
                else
                {
                    btn.BackColor = Color.Yellow;
                }
                pnTable.Controls.Add(btn);
                btn.Click += Click_Event;
                xLocation += 120;
                if (xLocation >= 400)
                {
                    xLocation = 5;
                    yLocation += 120;
                }
            }

        }

        private void Click_Event(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            //MessageBox.Show(btn.Name.ToString());
            var name = btn.Name;
            int len = name.Length;
            var idx = name.IndexOf('n');
            tableIndex = int.Parse(name.Substring(idx + 1, len - 1 - idx));
            //MessageBox.Show(tableIndex.ToString());

        }
        #endregion

        #region Checkout
        private void Main_Load(object sender, EventArgs e)
        {
            LoadDataTable();

            using (var db = new QLCafeEntities())
            {
                var lc = db.NhomSanPhams.Where(c => c.isDeleted == 0).ToList();
                var lcData = lc.Select(c => c.TenNhom).ToList();
                cbCategory.DataSource = lcData;
                //var lp = db.SanPhams.Where(p => p.isDeleted == 0).Select(p => p.TenSanPham).ToList();
                //cbProduct.DataSource = lp;
                int cid = lc[0].MaNhom;
                var lp = db.SanPhams.Where(p => p.isDeleted == 0 && p.MaNhom == cid).Select(p => p.TenSanPham).ToList();
                cbProduct.DataSource = lp;
            }

            using (var db = new QLCafeEntities())
            {
                var lt = db.ChiTietBanAns.Where(t => t.MaBan == tableIndex).Select(t=>new {t.MaSanPham,t.SanPham.TenSanPham,t.SoLuong,t.DonGia}).ToList();
                grvListTable.DataSource = lt;

            }
            grvListTable.Columns[0].HeaderText = "Tên món";
            grvListTable.Columns[1].HeaderText = "Số lượng";
            grvListTable.Columns[2].HeaderText = "Đơn giá";
            grvListTable.Columns[3].HeaderText = "Thành tiền";
            grvListTable.Columns[0].Width = 200;
            grvListTable.Columns[1].Width = 100;
            grvListTable.Columns[2].Width = 100;
            grvListTable.Columns[3].Width = 120;
            grvListTable.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9.75F, FontStyle.Bold);
            grvListTable.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            grvListTable.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grvListTable.EnableHeadersVisualStyles = false;

        }

        private void cbCatogory_TextChanged(object sender, EventArgs e)
        {
            using (var db = new QLCafeEntities())
            {
                var lc = db.NhomSanPhams.Where(c => c.isDeleted == 0).ToList();
                int selIndex = cbCategory.SelectedIndex;
                int cid = lc[selIndex].MaNhom;
                var lp = db.SanPhams.Where(p => p.isDeleted == 0 && p.MaNhom == cid).Select(p => p.TenSanPham).ToList();
                cbProduct.DataSource = lp;
            }
        }

        #endregion
    }
}
