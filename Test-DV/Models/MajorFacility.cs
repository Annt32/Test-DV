using System.ComponentModel.DataAnnotations;

namespace Test_DV.Models
{
    public class MajorFacility
    {
        [Key]
        public Guid Id { get; set; }
        public byte? Status { get; set; }
        public long? CreatedDate { get; set; }
        public long? LastModifiedDate { get; set; }
        public Guid? IdDepartmentFacility { get; set; }
        public Guid? IdMajor { get; set; }

        public DepartmentFacility DepartmentFacility { get; set; }
        public Major Major { get; set; }
        public ICollection<StaffMajorFacility> StaffMajorFacilities { get; set; }
    }
}
