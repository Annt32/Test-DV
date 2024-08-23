namespace Test_DV.Models
{
    public class ImportHistory
    {
        public Guid Id { get; set; }
        public DateTime ImportDate { get; set; }
        public string FileName { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
