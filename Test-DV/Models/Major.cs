using System.ComponentModel.DataAnnotations;

namespace Test_DV.Models
{
    public class Major
    {
        [Key]
        public Guid Id { get; set; }
        public byte? Status { get; set; }
        public long? CreatedDate { get; set; }
        public long? LastModifiedDate { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public ICollection<MajorFacility> MajorFacilities { get; set; }
    }
}
