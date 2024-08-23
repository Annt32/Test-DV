namespace Test_DV.Models
{
    public class StaffSpecializationViewModel
    {
        public Staff Staff { get; set; }
        public IEnumerable<Facility> Facilities { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<Major> Majors { get; set; }
    }

}
