using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using System.ComponentModel.Design;
using System.IO;
using Microsoft.Reporting.WinForms;

using QuanLyBanHang.Data;

namespace QuanLyBanHang.Reports
{
    public partial class frmThongKeSanPham : Form
    {
        QLBHDbContext context = new QLBHDbContext();
        QLBHDataSet.DanhSachSanPhamDataTable danhSachSanPhamDataTable = new QLBHDataSet.DanhSachSanPhamDataTable();
        string reportsFolder = Application.StartupPath.Replace("bin\\Debug\\net5.0-windows", "Reports");
        public frmThongKeSanPham()
        {
            InitializeComponent();
        }

        public void LayLoaiSanPhamVaoComboBox()
        {
            cboLoaiSanPham.DataSource = context.LoaiSanPhams.ToList();
            cboLoaiSanPham.ValueMember = "ID";
            cboLoaiSanPham.DisplayMember = "TenLoai";

        }

        public void LayHangSanXuatVaoComboBox()
        {
            cboHangSanXuat.DataSource = context.HangSanXuats.ToList();
            cboHangSanXuat.ValueMember = "ID";
            cboHangSanXuat.DisplayMember = "TenHangSanXuat";

        }


        private void frmThongKeSanPham_Load(object sender, EventArgs e)
        {
            LayLoaiSanPhamVaoComboBox();
            LayHangSanXuatVaoComboBox(); 

            


            var danhSachSanPham = context.SanPhams.Select(r => new DanhSachSanPham
            {
                Id = r.Id,
                HangSanXuatId = r.HangSanXuatId,
                TenHangSanXuat = r.HangSanXuat.TenHangSanXuat,
                LoaiSanPhamId = r.LoaiSanPhamId,
                TenLoai = r.LoaiSanPham.TenLoai,
                TenSanPham = r.TenSanPham,
                DonGia = r.DonGia,
                SoLuong = r.SoLuong,
                HinhAnh = r.HinhAnh,
                MoTa = r.MoTa
            }).ToList();

            danhSachSanPhamDataTable.Clear();
            foreach (var row in danhSachSanPham)
            {
                danhSachSanPhamDataTable.AddDanhSachSanPhamRow(row.Id,
                 row.HangSanXuatId,
                 row.TenHangSanXuat,
                 row.LoaiSanPhamId,
                 row.TenLoai,
                 row.TenSanPham,
                 row.DonGia,
                 row.SoLuong,
                 row.HinhAnh,
                 row.MoTa);
            }

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DanhSachSanPham";
            reportDataSource.Value = danhSachSanPhamDataTable;

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            //reportViewer.LocalReport.ReportPath = Path.Combine(reportsFolder, "rptThongKeSanPham.rdlc");

            reportViewer1.LocalReport.ReportEmbeddedResource = "QuanLyBanHang.Reports.rptThongKeSanPham.rdlc";

            ReportParameter reportParameter = new ReportParameter("MoTaKetQuaHienThi", "(Tất cả sản phẩm)");
            //reportViewer1.LocalReport.SetParameters(reportParameter);

            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;

            reportViewer1.RefreshReport();

        }

        private void btnLocKetQua_Click(object sender, EventArgs e)
        {
            if (cboHangSanXuat.Text == "" && cboLoaiSanPham.Text == "")
            {
                // Nếu cả 2 ComboBox đều bỏ trống thì hiển thị tất cả 
                frmThongKeSanPham_Load(sender, e);
            }
            else
            {
                var danhSachSanPham = context.SanPhams.Select(r => new DanhSachSanPham
                {
                    Id = r.Id,
                    HangSanXuatId = r.HangSanXuatId,
                    TenHangSanXuat = r.HangSanXuat.TenHangSanXuat,
                    LoaiSanPhamId = r.LoaiSanPhamId,
                    TenLoai = r.LoaiSanPham.TenLoai,
                    TenSanPham = r.TenSanPham,
                    DonGia = r.DonGia,
                    SoLuong = r.SoLuong,
                    HinhAnh = r.HinhAnh,
                    MoTa = r.MoTa
                });

                string hangSanXuat = null;
                string loaiSanPham = null;

                if (cboHangSanXuat.Text != "")
                {
                    int hangSanXuatID = Convert.ToInt32(cboHangSanXuat.SelectedValue.ToString());
                    hangSanXuat = "Hãng sản xuất: " + cboHangSanXuat.Text;
                    danhSachSanPham = danhSachSanPham.Where(r => r.HangSanXuatId == hangSanXuatID);
                }
                if (cboLoaiSanPham.Text != "")
                {
                    int loaiSanPhamID = Convert.ToInt32(cboLoaiSanPham.SelectedValue.ToString());
                    loaiSanPham += "Phân loại: " + cboLoaiSanPham.Text;
                    danhSachSanPham = danhSachSanPham.Where(r => r.LoaiSanPhamId == loaiSanPhamID);
                }

                danhSachSanPhamDataTable.Clear();
                foreach (var row in danhSachSanPham)
                {
                    danhSachSanPhamDataTable.AddDanhSachSanPhamRow(row.Id,
                        row.HangSanXuatId,
                        row.TenHangSanXuat,
                        row.LoaiSanPhamId,
                        row.TenLoai,
                        row.TenSanPham,
                        row.DonGia,
                        row.SoLuong,
                        row.HinhAnh,
                        row.MoTa);
                }

                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "DanhSachSanPham";
                reportDataSource.Value = danhSachSanPhamDataTable;

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                // reportViewer.LocalReport.ReportPath = Path.Combine(reportsFolder, "rptThongKeSanPham.rdlc");
                reportViewer1.LocalReport.ReportEmbeddedResource = "QuanLyBanHang.Reports.rptThongKeSanPham.rdlc";
                ReportParameter reportParameter = new ReportParameter("MoTaKetQuaHienThi", "(" + hangSanXuat + " - " + loaiSanPham + ")");
                //reportViewer1.LocalReport.SetParameters(reportParameter);
                reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
                reportViewer1.RefreshReport();
            }

        }
        }
}
