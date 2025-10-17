using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using aznews.Areas.Admin.Models;
using aznews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore; // ⚡ thêm dòng này để dùng AnyAsync, ToListAsync...

namespace aznews.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhoaVienController : Controller
    {
        private readonly DataContext _context;

        public KhoaVienController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tblist = _context.KhoaViens.OrderBy(m => m.MaKhoaVien).ToList();
            return View(tblist);
        }

        // POST: Admin/KhoaVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhoaVien model)
        {
            // Kiểm tra tính hợp lệ của dữ liệu nhập
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model);
            }

            // ⚠️ Đảm bảo dùng đúng DbSet tên là KhoaViens (giống trong DataContext)
            if (await _context.KhoaViens.AnyAsync(k => k.TenKhoaVien == model.TenKhoaVien))
            {
                ModelState.AddModelError("", "Tên khoa viện đã tồn tại.");
                return View(model);
            }

            // Thêm mới vào database
            _context.KhoaViens.Add(model);
            await _context.SaveChangesAsync();

            // Quay lại danh sách
            return RedirectToAction(nameof(Index));
        }
    }
}
