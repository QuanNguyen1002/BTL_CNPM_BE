using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BTL.Entities
{
    public class User : FullAuditedEntity<long>
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public UserType UserType { get; set; }
    }
    public enum UserType
    {
        HocSinh = 0,
        NhanVienBanThoiGian = 1,
        SinhVien = 2,
        NhanVienToanThoiGian = 3,
    }
}
