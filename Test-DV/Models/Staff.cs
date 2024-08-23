using System.ComponentModel.DataAnnotations;

namespace Test_DV.Models
{
    public class Staff
    {
        [Key]
        public Guid Id { get; set; }
        public byte? Status { get; set; }
        public long? CreatedDate { get; set; }
        public long? LastModifiedDate { get; set; }
        public string AccountFe { get; set; }
        public string AccountFpt { get; set; }
        public string Name { get; set; }
        public string StaffCode { get; set; }
        public ICollection<StaffMajorFacility> StaffMajorFacilities { get; set; }
        public ICollection<DepartmentFacility> DepartmentFacilities { get; set; }
    }
}
