using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WizLib_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyBookAndAuthorRelationBookAuthor2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Authors",
            //    columns: table => new
            //    {
            //        Author_Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Authors", x => x.Author_Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BookDetail",
            //    columns: table => new
            //    {
            //        BookDetail_Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        NumberOfChapters = table.Column<int>(type: "int", nullable: false),
            //        NumberOfPages = table.Column<int>(type: "int", nullable: false),
            //        Weight = table.Column<double>(type: "float", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BookDetail", x => x.BookDetail_Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Publishers",
            //    columns: table => new
            //    {
            //        Publisher_Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Publishers", x => x.Publisher_Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tb_Genre",
            //    columns: table => new
            //    {
            //        GenreId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tb_Genre", x => x.GenreId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Books",
            //    columns: table => new
            //    {
            //        Book_Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ISBN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
            //        Price = table.Column<double>(type: "float", nullable: false),
            //        BookDetail_Id = table.Column<int>(type: "int", nullable: false),
            //        Publisher_Id = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Books", x => x.Book_Id);
            //        table.ForeignKey(
            //            name: "FK_Books_BookDetail_BookDetail_Id",
            //            column: x => x.BookDetail_Id,
            //            principalTable: "BookDetail",
            //            principalColumn: "BookDetail_Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Books_Publishers_Publisher_Id",
            //            column: x => x.Publisher_Id,
            //            principalTable: "Publishers",
            //            principalColumn: "Publisher_Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                columns: table => new
                {
                    Book_Id = table.Column<int>(type: "int", nullable: false),
                    Author_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthors", x => new { x.Author_Id, x.Book_Id });
                    table.ForeignKey(
                        name: "FK_BookAuthors_Authors_Author_Id",
                        column: x => x.Author_Id,
                        principalTable: "Authors",
                        principalColumn: "Author_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthors_Books_Book_Id",
                        column: x => x.Book_Id,
                        principalTable: "Books",
                        principalColumn: "Book_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_Book_Id",
                table: "BookAuthors",
                column: "Book_Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Books_BookDetail_Id",
            //    table: "Books",
            //    column: "BookDetail_Id",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Books_Publisher_Id",
            //    table: "Books",
            //    column: "Publisher_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthors");

            //migrationBuilder.DropTable(
            //    name: "tb_Genre");

            //migrationBuilder.DropTable(
            //    name: "Authors");

            //migrationBuilder.DropTable(
            //    name: "Books");

            //migrationBuilder.DropTable(
            //    name: "BookDetail");

            //migrationBuilder.DropTable(
            //    name: "Publishers");
        }
    }
}
