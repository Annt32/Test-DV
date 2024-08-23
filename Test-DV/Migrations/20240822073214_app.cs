using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test_DV.Migrations
{
    public partial class app : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: true),
                    LastModifiedDate = table.Column<long>(type: "bigint", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: true),
                    LastModifiedDate = table.Column<long>(type: "bigint", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: true),
                    LastModifiedDate = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: true),
                    LastModifiedDate = table.Column<long>(type: "bigint", nullable: true),
                    AccountFe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountFpt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentFacilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: true),
                    LastModifiedDate = table.Column<long>(type: "bigint", nullable: true),
                    IdDepartment = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdFacility = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentFacilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentFacilities_Departments_IdDepartment",
                        column: x => x.IdDepartment,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentFacilities_Facilities_IdFacility",
                        column: x => x.IdFacility,
                        principalTable: "Facilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentFacilities_Staffs_IdStaff",
                        column: x => x.IdStaff,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MajorFacilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: true),
                    LastModifiedDate = table.Column<long>(type: "bigint", nullable: true),
                    IdDepartmentFacility = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdMajor = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MajorFacilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MajorFacilities_DepartmentFacilities_IdDepartmentFacility",
                        column: x => x.IdDepartmentFacility,
                        principalTable: "DepartmentFacilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MajorFacilities_Majors_IdMajor",
                        column: x => x.IdMajor,
                        principalTable: "Majors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StaffMajorFacilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: true),
                    LastModifiedDate = table.Column<long>(type: "bigint", nullable: true),
                    IdMajorFacility = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdStaff = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMajorFacilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffMajorFacilities_MajorFacilities_IdMajorFacility",
                        column: x => x.IdMajorFacility,
                        principalTable: "MajorFacilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffMajorFacilities_Staffs_IdStaff",
                        column: x => x.IdStaff,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentFacilities_IdDepartment",
                table: "DepartmentFacilities",
                column: "IdDepartment");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentFacilities_IdFacility",
                table: "DepartmentFacilities",
                column: "IdFacility");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentFacilities_IdStaff",
                table: "DepartmentFacilities",
                column: "IdStaff");

            migrationBuilder.CreateIndex(
                name: "IX_MajorFacilities_IdDepartmentFacility",
                table: "MajorFacilities",
                column: "IdDepartmentFacility");

            migrationBuilder.CreateIndex(
                name: "IX_MajorFacilities_IdMajor",
                table: "MajorFacilities",
                column: "IdMajor");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMajorFacilities_IdMajorFacility",
                table: "StaffMajorFacilities",
                column: "IdMajorFacility");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMajorFacilities_IdStaff",
                table: "StaffMajorFacilities",
                column: "IdStaff");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaffMajorFacilities");

            migrationBuilder.DropTable(
                name: "MajorFacilities");

            migrationBuilder.DropTable(
                name: "DepartmentFacilities");

            migrationBuilder.DropTable(
                name: "Majors");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "Staffs");
        }
    }
}
