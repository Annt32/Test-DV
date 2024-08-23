using Microsoft.AspNetCore.Mvc.Rendering;

namespace Test_DV.Models
{
    public class AddSpecializationViewModel
    {
        public Guid StaffId { get; set; }
        public Guid SelectedFacilityId { get; set; }
        public Guid SelectedDepartmentId { get; set; }
        public Guid SelectedMajorId { get; set; }

        public List<SelectListItem> Facilities { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Majors { get; set; }
    }

}
