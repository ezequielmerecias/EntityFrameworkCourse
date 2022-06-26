using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WizLib_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addRawCategoryToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO tbl_category VALUES('Cat 1')");
            migrationBuilder.Sql("INSERT INTO tbl_category VALUES('Cat 2')");
            migrationBuilder.Sql("INSERT INTO tbl_category VALUES('Cat 3')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
