using System;
using System.Linq;
using System.Threading.Tasks;
using aznews.Areas.Admin.Models;
using aznews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // 🟢 Index - Có tìm kiếm
        public async Task<IActionResult> Index(string? searchString)
        {
            var query = _context.KhoaViens.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToLower();
                query = query.Where(k =>
                    (k.TenKhoaVien != null && k.TenKhoaVien.ToLower().Contains(searchString)) ||
                    (k.Email != null && k.Email.ToLower().Contains(searchString)) ||
                    (k.DiaChi != null && k.DiaChi.ToLower().Contains(searchString)));
            }

            var list = await query.OrderBy(k => k.MaKhoaVien).ToListAsync();
            ViewData["SearchString"] = searchString;
            return View(list);
        }



        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhoaVien model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _context.KhoaViens.AnyAsync(k => k.TenKhoaVien == model.TenKhoaVien))
            {
                ModelState.AddModelError("", "Tên khoa viện đã tồn tại.");
                return View(model);
            }

            _context.KhoaViens.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var kv = await _context.KhoaViens.FirstOrDefaultAsync(k => k.MaKhoaVien == id);
            if (kv == null) return NotFound();
            return View(kv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhoaVien model)
        {
            if (id != model.MaKhoaVien) return NotFound();
            if (!ModelState.IsValid) return View(model);

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Có lỗi khi cập nhật dữ liệu.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var kv = await _context.KhoaViens.FindAsync(id);
            if (kv == null) return NotFound();

            _context.KhoaViens.Remove(kv);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa khoa viện thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}
