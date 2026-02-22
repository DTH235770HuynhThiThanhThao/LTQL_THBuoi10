using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang.Data
{
    public class SanPham
    {
        public int Id { get; set; }

        public int HangSanXuatId { get; set; }
       
        public int LoaiSanPhamId { get; set; }
      


        public string TenSanPham { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }

        public string? HinhAnh { get; set; }
        public string? MoTa { get; set; }

        // Navigation
        public LoaiSanPham LoaiSanPham { get; set; }
        public HangSanXuat HangSanXuat { get; set; }
    }


    public class DanhSachSanPham
    {
        public int Id { get; set; }

        public int HangSanXuatId { get; set; }
        public string TenHangSanXuat { get; set; }  //Thêm
        public int LoaiSanPhamId { get; set; }
        public string TenLoai { get; set; }   //Thêm


        public string TenSanPham { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }

        public string? HinhAnh { get; set; }
        public string? MoTa { get; set; }

        // Navigation
        public LoaiSanPham LoaiSanPham { get; set; }
        public HangSanXuat HangSanXuat { get; set; }
    }
}
