using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang.Data
{
    public class HangSanXuat
    {
        public int Id { get; set; }
        public string TenHangSanXuat { get; set; }

        // Navigation
        public ICollection<SanPham> SanPhams { get; set; }
    }
}
