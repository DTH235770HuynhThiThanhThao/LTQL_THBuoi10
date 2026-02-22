using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyBanHang.Data;


namespace QuanLyBanHang.Data
{
    public class LoaiSanPham
    {
        public int Id { get; set; }
        public string TenLoai { get; set; }

        // Navigation
        public ICollection<SanPham> SanPhams { get; set; }
    }
}
