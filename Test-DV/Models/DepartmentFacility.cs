using System.ComponentModel.DataAnnotations;

namespace Test_DV.Models
{
    public class DepartmentFacility
    {
        [Key]
        public Guid Id { get; set; }
        public byte? Status { get; set; }
        public long? CreatedDate { get; set; }
        public long? LastModifiedDate { get; set; }
        public Guid? IdDepartment { get; set; }
        public Guid? IdFacility { get; set; }
        public Guid? IdStaff { get; set; }

        public Department Department { get; set; }
        public Facility Facility { get; set; }
        public Staff Staff { get; set; }
        public ICollection<MajorFacility> MajorFacilities { get; set; }
    }
}
