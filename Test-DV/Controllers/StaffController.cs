using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Test_DV.Models;

namespace Test_DV.Controllers
{
    public class StaffController : Controller
    {
        private readonly AppDbContext _context;
        public StaffController()
        {
            _context = new AppDbContext();
        }

        public IActionResult Index()
        {
            var staffList = _context.Staffs.ToList();
            return View(staffList);
        }

        private bool IsValidEmail(string email, string domain)
        {
            if (string.IsNullOrWhiteSpace(email) || email.Length >= 100 || email.Contains(" ") || HasVietnameseChars(email))
                return false;

            return email.EndsWith(domain);
        }

        private bool HasVietnameseChars(string input)
        {
            string pattern = @"[áàạảãâấầậẩẫăắằặẳẵéèẹẻẽêếềệểễóòọỏõôốồộổỗơớờợởỡúùụủũưứừựửữíìịỉĩýỳỵỷỹđ]";
            return System.Text.RegularExpressions.Regex.IsMatch(input, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        private bool IsValidStaffCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code) && code.Length < 15;
        }

        private bool IsUniqueStaff(Staff staff)
        {
            return !_context.Staffs.Any(s =>
                s.Id != staff.Id &&
                (s.StaffCode == staff.StaffCode || s.AccountFe == staff.AccountFe || s.AccountFpt == staff.AccountFpt));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Staff staff)
        {

            try
            {
                if (!IsValidStaffCode(staff.StaffCode))
                {
                    ModelState.AddModelError("StaffCode", "Mã phải nhỏ hơn 15 ký tự và không được bỏ trống.");
                    return View(staff);
                }
                if (!IsValidEmail(staff.AccountFpt, "@fpt.edu.vn"))
                {
                    ModelState.AddModelError("AccountFpt", "Email FPT phải kết thúc bằng '@fpt.edu.vn', không chứa khoảng trắng và không có tiếng Việt.");
                    return View(staff);
                }
                if (!IsValidEmail(staff.AccountFe, "@fe.edu.vn"))
                {
                    ModelState.AddModelError("AccountFe", "Email FE phải kết thúc bằng '@fe.edu.vn', không chứa khoảng trắng và không có tiếng Việt.");
                    return View(staff);
                }
                if (!IsUniqueStaff(staff))
                {
                    ModelState.AddModelError("", "Mã, email FPT hoặc email FE đã tồn tại.");
                    return View(staff);
                }
                

                staff.Id = Guid.NewGuid();
                staff.Status = 1;
                staff.CreatedDate = DateTime.Now.Ticks;
                staff.LastModifiedDate = DateTime.Now.Ticks;

                _context.Staffs.Add(staff);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi, {ex}");
                return View(staff);
            }

        }


        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = _context.Staffs.Find(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            try
            {
                if (!IsValidStaffCode(staff.StaffCode))
                {
                    ModelState.AddModelError("StaffCode", "Mã phải nhỏ hơn 15 ký tự và không được bỏ trống.");
                    return View(staff);
                }
                if (!IsValidEmail(staff.AccountFpt, "@fpt.edu.vn"))
                {
                    ModelState.AddModelError("AccountFpt", "Email FPT phải kết thúc bằng '@fpt.edu.vn', không chứa khoảng trắng và không có tiếng Việt.");
                    return View(staff);
                }
                if (!IsValidEmail(staff.AccountFe, "@fe.edu.vn"))
                {
                    ModelState.AddModelError("AccountFe", "Email FE phải kết thúc bằng '@fe.edu.vn', không chứa khoảng trắng và không có tiếng Việt.");
                    return View(staff);
                }
                if (!IsUniqueStaff(staff))
                {
                    ModelState.AddModelError("", "Mã, email FPT hoặc email FE đã tồn tại.");
                    return View(staff);
                }

                try
                {
                    _context.Update(staff);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Staffs.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi, {ex}");
                return View(staff);
            }
                       
        }


        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = _context.Staffs
                .FirstOrDefault(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var staff = _context.Staffs.Find(id);
            _context.Staffs.Remove(staff);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult ToggleStatus(Guid id)
        {
            var staff = _context.Staffs.Find(id);
            if (staff == null)
            {
                return NotFound();
            }

            staff.Status = staff.Status == 1 ? (byte)0 : (byte)1;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ManageSpecialization(Guid staffId)
        {
            var staff = _context.Staffs
                .Include(s => s.StaffMajorFacilities)
                .ThenInclude(smf => smf.MajorFacility)
                .ThenInclude(mf => mf.DepartmentFacility)
                .ThenInclude(df => df.Facility)
                .Include(s => s.StaffMajorFacilities)
                .ThenInclude(smf => smf.MajorFacility)
                .ThenInclude(mf => mf.DepartmentFacility)
                .ThenInclude(df => df.Department)
                .Include(s => s.StaffMajorFacilities)
                .ThenInclude(smf => smf.MajorFacility)
                .ThenInclude(mf => mf.Major)
                .FirstOrDefault(s => s.Id == staffId);

            if (staff == null)
            {
                return NotFound();
            }

            var viewModel = new StaffSpecializationViewModel
            {
                Staff = staff
            };

            return View(viewModel);
        }


        public IActionResult AddSpecialization(Guid staffId)
        {
            // Lấy danh sách các cơ sở mà nhân viên đã có bộ môn và chuyên ngành
            var existingFacilities = _context.StaffMajorFacilities
                .Where(smf => smf.IdStaff == staffId)
                .Select(smf => smf.MajorFacility.DepartmentFacility.IdFacility)
                .ToList();

            // Lọc ra các cơ sở chưa có trong danh sách của nhân viên
            var availableFacilities = _context.Facilities
                .Where(f => !existingFacilities.Contains(f.Id))
                .Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList();

            var viewModel = new AddSpecializationViewModel
            {
                StaffId = staffId,
                Facilities = availableFacilities,
                Departments = new List<SelectListItem>(), // Để trống lúc đầu
                Majors = new List<SelectListItem>() // Để trống lúc đầu
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult AddSpecialization(AddSpecializationViewModel viewModel)
        {

            // Bảng DepartmentFacilities
            var departmentFacility = new DepartmentFacility
            {
                Id = Guid.NewGuid(),
                IdDepartment = viewModel.SelectedDepartmentId,
                IdFacility = viewModel.SelectedFacilityId,
                Status = 1,
                CreatedDate = DateTime.Now.Ticks,
                LastModifiedDate = DateTime.Now.Ticks
            };
            _context.DepartmentFacilities.Add(departmentFacility);
            _context.SaveChanges();

            // Bảng MajorFacilities
            var majorFacility = new MajorFacility
            {
                Id = Guid.NewGuid(),
                IdDepartmentFacility = departmentFacility.Id,
                IdMajor = viewModel.SelectedMajorId,
                Status = 1,
                CreatedDate = DateTime.Now.Ticks,
                LastModifiedDate = DateTime.Now.Ticks
            };
            _context.MajorFacilities.Add(majorFacility);
            _context.SaveChanges();

            // Bảng StaffMajorFacilities
            var staffMajorFacility = new StaffMajorFacility
            {
                Id = Guid.NewGuid(),
                IdStaff = viewModel.StaffId,
                IdMajorFacility = majorFacility.Id,
                Status = 1,
                CreatedDate = DateTime.Now.Ticks,
                LastModifiedDate = DateTime.Now.Ticks
            };
            _context.StaffMajorFacilities.Add(staffMajorFacility);
            _context.SaveChanges();

            return RedirectToAction("ManageSpecialization", new { staffId = viewModel.StaffId });
        }


        public JsonResult GetDepartmentsByFacility(Guid facilityId)
        {
            var departments = from d in _context.Departments
                              join df in _context.DepartmentFacilities on d.Id equals df.IdDepartment
                              join f in _context.Facilities on df.IdFacility equals f.Id
                              where f.Id == facilityId
                              select new
                              {
                                  d.Id,
                                  d.Name 
                              };

            return Json(departments.ToList());
        }



        public JsonResult GetMajorsByDepartment(Guid departmentId)
        {
            var majors = from m in _context.Majors
                         join mf in _context.MajorFacilities on m.Id equals mf.IdMajor
                         join df in _context.DepartmentFacilities on mf.IdDepartmentFacility equals df.Id
                         where df.IdDepartment == departmentId
                         select new
                         {
                             m.Id,
                             m.Name,
                         };

            return Json(majors.ToList());
        }

        [HttpPost]
        public IActionResult DeleteSpecialization(Guid id)
        {
            // Tìm bản ghi trong bảng StaffMajorFacilities
            var staffMajorFacility = _context.StaffMajorFacilities.FirstOrDefault(smf => smf.Id == id);

            if (staffMajorFacility == null)
            {
                return NotFound();
            }

            _context.StaffMajorFacilities.Remove(staffMajorFacility);

            // Tìm bản ghi trong bảng MajorFacilities
            var majorFacility = _context.MajorFacilities.FirstOrDefault(mf => mf.Id == staffMajorFacility.IdMajorFacility);

            if (majorFacility != null)
            {
                // Kiểm tra nếu có bất kỳ bản ghi nào khác trong StaffMajorFacilities đang tham chiếu đến MajorFacility này
                var relatedStaffMajorFacilities = _context.StaffMajorFacilities
                    .Where(smf => smf.IdMajorFacility == majorFacility.Id)
                    .ToList();

                // Nếu không còn bản ghi nào khác tham chiếu, xóa MajorFacility và liên kết với DepartmentFacility
                if (!relatedStaffMajorFacilities.Any())
                {
                    var departmentFacility = _context.DepartmentFacilities
                        .FirstOrDefault(df => df.Id == majorFacility.IdDepartmentFacility);

                    if (departmentFacility != null)
                    {
                        _context.DepartmentFacilities.Remove(departmentFacility);
                    }

                    _context.MajorFacilities.Remove(majorFacility);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("ManageSpecialization", new { staffId = staffMajorFacility.IdStaff });
        }


        public IActionResult DownloadTemplate()
        {
            // Thiết lập LicenseContext
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // Tạo một worksheet
                var worksheet = package.Workbook.Worksheets.Add("Template");

                // Tạo tiêu đề cột
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Mã Nhân Viên";
                worksheet.Cells[1, 3].Value = "Họ và Tên";
                worksheet.Cells[1, 4].Value = "Email FPT";
                worksheet.Cells[1, 5].Value = "Email FE";
                worksheet.Cells[1, 6].Value = "Bộ Môn - Chuyên Ngành";

                // Định dạng tiêu đề cột
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                }

                var staffs = _context.Staffs
                     .Include(s => s.StaffMajorFacilities)
                     .ThenInclude(smf => smf.MajorFacility)
                     .ThenInclude(mf => mf.DepartmentFacility)
                     .ThenInclude(df => df.Facility)
                     .Include(s => s.StaffMajorFacilities)
                     .ThenInclude(smf => smf.MajorFacility)
                     .ThenInclude(mf => mf.DepartmentFacility)
                     .ThenInclude(df => df.Department)
                     .Include(s => s.StaffMajorFacilities)
                     .ThenInclude(smf => smf.MajorFacility)
                     .ThenInclude(mf => mf.Major)
                     .ToList();

                int row = 2;
                int stt = 1;
                foreach (var staff in staffs)
                {
                    var specializations = staff.StaffMajorFacilities
                        .Select(smf =>
                            $"{smf.MajorFacility?.DepartmentFacility?.Facility?.Name ?? "N/A"} - " +
                            $"{smf.MajorFacility?.DepartmentFacility?.Department?.Name ?? "N/A"} - " +
                            $"{smf.MajorFacility?.Major?.Name ?? "N/A"}")
                        .ToList();

                    if (specializations.Any())
                    {
                        foreach (var specialization in specializations)
                        {
                            worksheet.Cells[row, 1].Value = stt++; // STT
                            worksheet.Cells[row, 2].Value = staff.StaffCode; // Mã Nhân Viên
                            worksheet.Cells[row, 3].Value = staff.Name; // Họ và Tên
                            worksheet.Cells[row, 4].Value = staff.AccountFpt; // Email FPT
                            worksheet.Cells[row, 5].Value = staff.AccountFe; // Email FE
                            worksheet.Cells[row, 6].Value = specialization; // Bộ Môn - Chuyên Ngành
                            row++;
                        }
                    }
                    else
                    {
                        worksheet.Cells[row, 1].Value = stt++; // STT
                        worksheet.Cells[row, 2].Value = staff.StaffCode; // Mã Nhân Viên
                        worksheet.Cells[row, 3].Value = staff.Name; // Họ và Tên
                        worksheet.Cells[row, 4].Value = staff.AccountFpt; // Email FPT
                        worksheet.Cells[row, 5].Value = staff.AccountFe; // Email FE
                        worksheet.Cells[row, 6].Value = "Chưa có"; // Bộ Môn - Chuyên Ngành
                        row++;
                    }
                }

                // Tạo file Excel
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // Trả về file Tải xuống
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Template.xlsx");
            }
        }




        [HttpPost]
        public IActionResult ImportStaff(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "File không hợp lệ.");
                return RedirectToAction("Index");
            }

            var importHistory = new ImportHistory
            {
                Id = Guid.NewGuid(),
                ImportDate = DateTime.Now,
                FileName = file.FileName,
                Status = "Đang xử lý",
                Message = ""
            };
            _context.ImportHistories.Add(importHistory);
            _context.SaveChanges();

            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            try
                            {
                                var staffCode = worksheet.Cells[row, 2].Text;
                                var name = worksheet.Cells[row, 3].Text;
                                var emailFpt = worksheet.Cells[row, 4].Text;
                                var emailFe = worksheet.Cells[row, 5].Text;
                                var specialization = worksheet.Cells[row, 6].Text;

                                staffCode = staffCode.Trim().ToLower();
                                emailFpt = emailFpt.Trim().ToLower();
                                emailFe = emailFe.Trim().ToLower();

                                // Kiểm tra mã nhân viên có trong email FPT và FE không
                                if (!emailFpt.Contains(staffCode) || !emailFe.Contains(staffCode))
                                {
                                    throw new Exception($"Dòng {row}: Email không chứa mã nhân viên.");
                                }


                                var staff = new Staff
                                {
                                    Id = Guid.NewGuid(),
                                    StaffCode = staffCode,
                                    Name = name,
                                    AccountFpt = emailFpt,
                                    AccountFe = emailFe,
                                    Status = 1, 
                                    CreatedDate = DateTime.Now.Ticks,
                                    LastModifiedDate = DateTime.Now.Ticks
                                };
                                _context.Staffs.Add(staff);

                                // Tách thông tin Bộ Môn - Chuyên Ngành - Cơ Sở
                                var parts = specialization.Split('-');
                                if (parts.Length != 3)
                                {
                                    throw new Exception($"Dòng {row}: Dữ liệu chuyên ngành không hợp lệ.");
                                }

                                var facilityName = parts[2].Trim().ToLower();
                                var departmentName = parts[1].Trim().ToLower();
                                var majorName = parts[0].Trim().ToLower();

                                var facility = _context.Facilities
                                    .FirstOrDefault(f => f.Name.ToLower() == facilityName);
                                var department = _context.Departments
                                    .FirstOrDefault(d => d.Name.ToLower() == departmentName);
                                var major = _context.Majors
                                    .FirstOrDefault(m => m.Name.ToLower() == majorName);

                                if (facility == null || department == null || major == null)
                                {
                                    throw new Exception($"Dòng {row}: Không tìm thấy cơ sở, bộ môn, hoặc chuyên ngành.");
                                }


                                // Lưu thông tin chuyên ngành của nhân viên
                                var departmentFacility = _context.DepartmentFacilities.FirstOrDefault(df =>
                                    df.IdDepartment == department.Id && df.IdFacility == facility.Id);
                                if (departmentFacility == null)
                                {
                                    departmentFacility = new DepartmentFacility
                                    {
                                        Id = Guid.NewGuid(),
                                        IdDepartment = department.Id,
                                        IdFacility = facility.Id,
                                        Status = 1,
                                        CreatedDate = DateTime.Now.Ticks,
                                        LastModifiedDate = DateTime.Now.Ticks
                                    };
                                    _context.DepartmentFacilities.Add(departmentFacility);
                                    _context.SaveChanges();
                                }

                                var majorFacility = _context.MajorFacilities.FirstOrDefault(mf =>
                                    mf.IdDepartmentFacility == departmentFacility.Id && mf.IdMajor == major.Id);
                                if (majorFacility == null)
                                {
                                    majorFacility = new MajorFacility
                                    {
                                        Id = Guid.NewGuid(),
                                        IdDepartmentFacility = departmentFacility.Id,
                                        IdMajor = major.Id,
                                        Status = 1,
                                        CreatedDate = DateTime.Now.Ticks,
                                        LastModifiedDate = DateTime.Now.Ticks
                                    };
                                    _context.MajorFacilities.Add(majorFacility);
                                    _context.SaveChanges();
                                }

                                var staffMajorFacility = new StaffMajorFacility
                                {
                                    Id = Guid.NewGuid(),
                                    IdStaff = staff.Id,
                                    IdMajorFacility = majorFacility.Id,
                                    Status = 1,
                                    CreatedDate = DateTime.Now.Ticks,
                                    LastModifiedDate = DateTime.Now.Ticks
                                };
                                _context.StaffMajorFacilities.Add(staffMajorFacility);

                                importHistory.Message += $"Dòng {row}: Import thành công.\n";
                            }
                            catch (Exception ex)
                            {
                                importHistory.Message += $"Dòng {row}: Lỗi - {ex.Message}\n";
                            }
                        }
                        _context.SaveChanges();
                    }
                }
                importHistory.Status = "Hoàn thành";
            }
            catch (Exception ex)
            {
                importHistory.Status = "Lỗi";
                importHistory.Message += $"Lỗi hệ thống: {ex.Message}";
            }
            finally
            {
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult ImportHistory()
        {
            var histories = _context.ImportHistories
                .OrderByDescending(h => h.ImportDate)
                .ToList();

            return View(histories);
        }

    }
}
