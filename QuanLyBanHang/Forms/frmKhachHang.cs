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

namespace QuanLyBanHang.Forms
{
    public partial class frmKhachHang : Form
    {
        QLBHDbContext context = new QLBHDbContext();
        bool xuLyThem = false;
        int id;
        public frmKhachHang()
        {
            InitializeComponent();
        }

        private void BatTatChucNang(bool giaTri)
        {
            btnLuu.Enabled = giaTri;
            btnHuyBo.Enabled = giaTri;
            txtHoVaTen.Enabled = giaTri;
            txtDienThoai.Enabled = giaTri;
            txtDiaChi.Enabled = giaTri;

            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
            btnTimKiem.Enabled = !giaTri;
            btnNhap.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            BatTatChucNang(false);
            List<KhachHang> kh = new List<KhachHang>();
            kh = context.KhachHangs.ToList();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = kh;

            txtHoVaTen.DataBindings.Clear();
            txtHoVaTen.DataBindings.Add("Text", bindingSource, "HoVaTen", false, DataSourceUpdateMode.Never);
            txtDienThoai.DataBindings.Clear();
            txtDienThoai.DataBindings.Add("Text", bindingSource, "DienThoai", false, DataSourceUpdateMode.Never);
            txtDiaChi.DataBindings.Clear();
            txtDiaChi.DataBindings.Add("Text", bindingSource, "DiaChi", false, DataSourceUpdateMode.Never);
            dataGridView.DataSource = bindingSource;

            // --- THÊM DÒNG NÀY ĐỂ ẨN CỘT ---
            if (dataGridView.Columns["HoaDon"] != null)
            {
                dataGridView.Columns["HoaDon"].Visible = false;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            xuLyThem = true;
            BatTatChucNang(true);
            txtHoVaTen.Clear();
            txtDienThoai.Clear();
            txtDiaChi.Clear();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            xuLyThem = false;
            BatTatChucNang(true);
            id = Convert.ToInt32(dataGridView.CurrentRow.Cells["ID"].Value.ToString());
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoVaTen.Text))
                MessageBox.Show("Vui lòng nhập họ và tên khách hàng?", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {

                if (xuLyThem)
                {
                    KhachHang kh = new KhachHang();
                    kh.HoVaTen = txtHoVaTen.Text;
                    kh.DienThoai = txtDienThoai.Text;
                    kh.DiaChi = txtDiaChi.Text;
                    context.KhachHangs.Add(kh);

                    context.SaveChanges();
                }
                else
                {
                    KhachHang kh = context.KhachHangs.Find(id);
                    if (kh != null)
                    {
                        kh.HoVaTen = txtHoVaTen.Text;
                        kh.DienThoai = txtDienThoai.Text;
                        kh.DiaChi = txtDiaChi.Text;
                        context.KhachHangs.Update(kh);
                        context.SaveChanges();
                    }

                }
                frmKhachHang_Load(sender, e);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận xóa khách hàng " + txtHoVaTen.Text + "?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                id = Convert.ToInt32(dataGridView.CurrentRow.Cells["ID"].Value.ToString());
                KhachHang kh = context.KhachHangs.Find(id);
                if (kh != null)
                {
                    context.KhachHangs.Remove(kh);
                }
                context.SaveChanges();
                frmKhachHang_Load(sender, e);
            }

        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            frmKhachHang_Load(sender, e);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // BƯỚC 1: Nếu ô nhập tên đang bị khóa, mở ra để người dùng nhập
            if (txtHoVaTen.Enabled == false)
            {
                // Mở các ô nhập liệu
                txtHoVaTen.Enabled = true;
                txtDienThoai.Enabled = true;

                // Xóa trắng để nhập mới
                txtHoVaTen.Clear();
                txtDienThoai.Clear();
                txtDiaChi.Clear();

                // Khóa các nút không cần thiết, chỉ để lại nút Tìm và Hủy
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false; // Không cần lưu khi đang tìm
                btnHuyBo.Enabled = true;

                // Đưa con trỏ chuột vào ô Tên
                txtHoVaTen.Focus();
            }
            // BƯỚC 2: Nếu ô tên đã mở rồi -> Thực hiện tìm kiếm
            else
            {
                string tuKhoa = txtHoVaTen.Text.Trim(); // Lấy từ khóa người dùng nhập

                // Lọc dữ liệu: Tìm theo Tên HOẶC Số điện thoại
                var ketQua = context.KhachHangs
                    .Where(k => k.HoVaTen.Contains(tuKhoa) || k.DienThoai.Contains(tuKhoa))
                    .ToList();

                // Đổ dữ liệu mới tìm được vào bảng
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = ketQua;
                dataGridView.DataSource = bindingSource;


                // Thông báo nếu không tìm thấy
                if (ketQua.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy khách hàng nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "CSV file (*.csv)|*.csv";
            saveFile.FileName = "KhachHang.csv";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                var danhSach = context.KhachHangs.ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("HoVaTen,DienThoai,DiaChi");

                foreach (var kh in danhSach)
                {
                    sb.AppendLine($"{kh.HoVaTen},{kh.DienThoai},{kh.DiaChi}");
                }

                System.IO.File.WriteAllText(saveFile.FileName, sb.ToString(), Encoding.UTF8);

                MessageBox.Show("Xuất dữ liệu thành công!", "Thông báo");
            }
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "CSV file (*.csv)|*.csv";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var lines = System.IO.File.ReadAllLines(openFile.FileName, Encoding.UTF8);

                // Bỏ dòng tiêu đề (dòng đầu)
                for (int i = 1; i < lines.Length; i++)
                {
                    var data = lines[i].Split(',');

                    if (data.Length >= 3)
                    {
                        KhachHang kh = new KhachHang();
                        kh.HoVaTen = data[0];
                        kh.DienThoai = data[1];
                        kh.DiaChi = data[2];

                        context.KhachHangs.Add(kh);
                    }
                }

                context.SaveChanges();
                frmKhachHang_Load(sender, e);

                MessageBox.Show("Nhập dữ liệu thành công!", "Thông báo");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
