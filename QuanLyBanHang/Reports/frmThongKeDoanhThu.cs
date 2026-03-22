using DocumentFormat.OpenXml.InkML;
using Microsoft.Reporting.WinForms;
using QuanLyBanHang.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuanLyBanHang.Reports.QLBHDataSet;

using System.ComponentModel.Design;
using System.IO;
//using Microsoft.Reporting.WinForms;

namespace QuanLyBanHang.Reports
{
    public partial class frmThongKeDoanhThu : Form
    {
        QLBHDbContext context = new QLBHDbContext();
        QLBHDataSet.DanhSachHoaDonDataTable danhSachHoaDonDataTable = new QLBHDataSet.DanhSachHoaDonDataTable();
        string reportsFolder = Application.StartupPath.Replace("bin\\Debug\\net5.0-windows", "Reports");
        public frmThongKeDoanhThu()
        {
            InitializeComponent();
        }

        private void frmThongKeDoanhThu_Load(object sender, EventArgs e)
        {
            try
            {
                // 2. Lấy dữ liệu từ Database bằng Entity Framework
                // Sử dụng .Select để ánh xạ sang lớp DanhSachHoaDon và tính tổng tiền
                var danhSachHoaDon = context.HoaDons.Select(r => new DanhSachHoaDon
                {
                    Id = r.Id,
                    NhanVienId = r.NhanVienId,
                    HoVaTenNhanVien = r.NhanVien.HoVaTen, // Lưu ý: kiểm tra tên thuộc tính HoVaTen ở lớp NhanVien
                    KhachHangId = r.KhachHangId,
                    HoVaTenKhachHang = r.KhachHang.HoVaTen, // Lưu ý: kiểm tra tên thuộc tính HoVaTen ở lớp KhachHang
                    NgayLap = r.NgayLap,
                    GhiChuHoaDon = r.GhiChuHoaDon,
                    // Tính tổng tiền từ danh sách chi tiết hóa đơn: Sum(Số lượng * Đơn giá)
                    TongTienHoaDon = r.HoaDonChiTiets.Sum(ct => ct.SoLuongBan * ct.DonGiaBan)
                }).ToList();

                // 3. Xóa dữ liệu cũ trong DataTable của Dataset (tránh cộng dồn khi load lại)
                // Lưu ý: Đảm bảo bạn đã khai báo biến danhSachHoaDonDataTable ở cấp class
                danhSachHoaDonDataTable.Clear();

                // 4. Duyệt danh sách vừa lấy được để nạp vào DataTable của Dataset
                foreach (var row in danhSachHoaDon)
                {
                    danhSachHoaDonDataTable.AddDanhSachHoaDonRow(
                        row.Id,
                        row.NhanVienId,
                        row.HoVaTenNhanVien,
                        row.KhachHangId,
                        row.HoVaTenKhachHang,
                        row.NgayLap,
                        row.GhiChuHoaDon,
                        row.TongTienHoaDon ?? 0 // Nếu null thì để là 0
                    );
                }

                // 5. Thiết lập nguồn dữ liệu cho Report
                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "DanhSachHoaDon"; // Tên này phải khớp chính xác với tên Dataset trong file .rdlc
                reportDataSource.Value = danhSachHoaDonDataTable;

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);

                // 6. Gán file báo cáo (Kiểm tra lại đường dẫn namespace của bạn)
                reportViewer1.LocalReport.ReportEmbeddedResource = "QuanLyBanHang.Reports.rptThongKeDoanhThu.rdlc";

                // 7. Truyền tham số cho tiêu đề phụ (Parameter)
                ReportParameter reportParameter = new ReportParameter("MoTaKetQuaHienThi", "(Tất cả hóa đơn)");
                reportViewer1.LocalReport.SetParameters(reportParameter);

                // 8. Cấu hình hiển thị và Refresh
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLocKetQua_Click(object sender, EventArgs e)
        {
            var danhSachHoaDon = context.HoaDons.Select(r => new DanhSachHoaDon
            {
                Id = r.Id,
                NhanVienId = r.NhanVienId,
                HoVaTenNhanVien = r.NhanVien.HoVaTen,
                KhachHangId = r.KhachHangId,
                HoVaTenKhachHang = r.KhachHang.HoVaTen,
                NgayLap = r.NgayLap,
                GhiChuHoaDon = r.GhiChuHoaDon,
                TongTienHoaDon = r.HoaDonChiTiets.Sum(r => r.SoLuongBan * r.DonGiaBan)
            });

            danhSachHoaDon = danhSachHoaDon.Where(r => r.NgayLap >= dtpTuNgay.Value && r.NgayLap <= dtpDenNgay.Value);
            danhSachHoaDonDataTable.Clear();
            foreach (var row in danhSachHoaDon)
            {
                danhSachHoaDonDataTable.AddDanhSachHoaDonRow(row.Id,
                    row.NhanVienId,
                    row.HoVaTenNhanVien,
                    row.KhachHangId,
                    row.HoVaTenKhachHang,
                    row.NgayLap,
                    row.GhiChuHoaDon,
                    row.TongTienHoaDon ?? 0);
            }

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DanhSachHoaDon";
            reportDataSource.Value = danhSachHoaDonDataTable;

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            reportViewer1.LocalReport.ReportPath = Path.Combine(reportsFolder, "rptThongKeDoanhThu.rdlc");

            ReportParameter reportParameter = new ReportParameter("MoTaKetQuaHienThi", "Từ ngày " + dtpTuNgay.Text + " - Đến ngày: " + dtpDenNgay.Text);
            reportViewer1.LocalReport.SetParameters(reportParameter);

            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;

            reportViewer1.RefreshReport();
        }

        private void btnHienTatCa_Click(object sender, EventArgs e)
        {
            frmThongKeDoanhThu_Load(sender, e);
        }
    }
}
