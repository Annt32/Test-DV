using System.ComponentModel.DataAnnotations;

namespace Test_DV.Models
{
    public class StaffMajorFacility
    {
        [Key]
        public Guid Id { get; set; }
        public byte? Status { get; set; }
        public long? CreatedDate { get; set; }
        public long? LastModifiedDate { get; set; }
        public Guid? IdMajorFacility { get; set; }
        public Guid? IdStaff { get; set; }

        public MajorFacility MajorFacility { get; set; }
        public Staff Staff { get; set; }
    }
}
