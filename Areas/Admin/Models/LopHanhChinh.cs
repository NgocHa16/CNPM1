using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aznews.Areas.Admin.Models
{
    [Table("LopHanhChinh")]
    public class LopHanhChinh
    {
        [Key]
        public int MaLopHC { get; set; }
        public string? TenLopHC { get; set; }
        public string? KhoaHoc { get; set; }
        public int MaNganh { get; set; }
    }
}