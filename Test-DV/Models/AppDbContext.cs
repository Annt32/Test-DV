using Microsoft.EntityFrameworkCore;

namespace Test_DV.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentFacility> DepartmentFacilities { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<MajorFacility> MajorFacilities { get; set; }
        public DbSet<StaffMajorFacility> StaffMajorFacilities { get; set; }
        public DbSet<ImportHistory> ImportHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-PMB8531\\SQLEXPRESS;Initial Catalog=exam_distribution_test_2;Integrated Security=True;Trust Server Certificate=True");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffMajorFacility>()
                    .HasKey(smf => smf.Id);

            modelBuilder.Entity<MajorFacility>()
                .HasKey(mf => mf.Id);

            modelBuilder.Entity<DepartmentFacility>()
                .HasKey(df => df.Id);

            // Thiết lập mối quan hệ
            modelBuilder.Entity<StaffMajorFacility>()
                .HasOne(smf => smf.MajorFacility)
                .WithMany(mf => mf.StaffMajorFacilities)
                .HasForeignKey(smf => smf.IdMajorFacility);

            modelBuilder.Entity<StaffMajorFacility>()
                .HasOne(smf => smf.Staff)
                .WithMany(s => s.StaffMajorFacilities)
                .HasForeignKey(smf => smf.IdStaff);

            modelBuilder.Entity<MajorFacility>()
                .HasOne(mf => mf.DepartmentFacility)
                .WithMany(df => df.MajorFacilities)
                .HasForeignKey(mf => mf.IdDepartmentFacility);

            modelBuilder.Entity<MajorFacility>()
                .HasOne(mf => mf.Major)
                .WithMany(m => m.MajorFacilities)
                .HasForeignKey(mf => mf.IdMajor);

            modelBuilder.Entity<DepartmentFacility>()
                .HasOne(df => df.Department)
                .WithMany(d => d.DepartmentFacilities)
                .HasForeignKey(df => df.IdDepartment);

            modelBuilder.Entity<DepartmentFacility>()
                .HasOne(df => df.Facility)
                .WithMany(f => f.DepartmentFacilities)
                .HasForeignKey(df => df.IdFacility);

            modelBuilder.Entity<DepartmentFacility>()
                .HasOne(df => df.Staff)
                .WithMany(s => s.DepartmentFacilities)
                .HasForeignKey(df => df.IdStaff);


            base.OnModelCreating(modelBuilder);
        }
    }
}
