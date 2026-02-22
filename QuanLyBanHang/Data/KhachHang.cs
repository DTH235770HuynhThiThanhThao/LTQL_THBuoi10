using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang.Data
{
    [Table("KhachHangs")]
    public class KhachHang
    {
        public int Id { get; set; }
        public string HoVaTen { get; set; }
        public string? DienThoai { get; set; }
        public string? DiaChi { get; set; }

        // Navigation
        public ICollection<HoaDon> HoaDons { get; set; }
    }
}
