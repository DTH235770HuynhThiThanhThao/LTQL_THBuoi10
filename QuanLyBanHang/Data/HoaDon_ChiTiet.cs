using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang.Data
{
    public class HoaDon_ChiTiet
    {
        public int Id { get; set; }

        public int HoaDonId { get; set; }
        public int SanPhamId { get; set; }

        public short SoLuongBan { get; set; }
        public decimal DonGiaBan { get; set; }

        // Navigation
        public HoaDon HoaDon { get; set; }
        public SanPham SanPham { get; set; }
    }
}
