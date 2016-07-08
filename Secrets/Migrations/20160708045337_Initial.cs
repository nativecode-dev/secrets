namespace Secrets.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accessors",
                columns:
                    table =>
                    new
                        {
                            Key = table.Column<Guid>(nullable: false),
                            DateCreated = table.Column<DateTimeOffset>(nullable: false),
                            DateModified = table.Column<DateTimeOffset>(nullable: false),
                            Email = table.Column<string>(maxLength: 256, nullable: false),
                            Login = table.Column<string>(maxLength: 256, nullable: false)
                        },
                constraints: table => { table.PrimaryKey("PK_Accessors", x => x.Key); });

            migrationBuilder.CreateTable(
                name: "Secrets",
                columns:
                    table =>
                    new
                        {
                            Key = table.Column<Guid>(nullable: false),
                            ApiKey = table.Column<string>(maxLength: 256, nullable: true),
                            DateCreated = table.Column<DateTimeOffset>(nullable: false),
                            DateModified = table.Column<DateTimeOffset>(nullable: false),
                            Login = table.Column<string>(maxLength: 256, nullable: true),
                            MaxUse = table.Column<int>(nullable: true),
                            MaxUseCounter = table.Column<int>(nullable: true),
                            Password = table.Column<string>(maxLength: 128, nullable: true),
                            Url = table.Column<string>(maxLength: 1024, nullable: false),
                            UrlPattern = table.Column<string>(maxLength: 2048, nullable: true)
                        },
                constraints: table => { table.PrimaryKey("PK_Secrets", x => x.Key); });

            migrationBuilder.CreateTable(
                name: "SecretAccessor",
                columns:
                    table =>
                    new
                        {
                            Key = table.Column<Guid>(nullable: false),
                            AccessorKey = table.Column<Guid>(nullable: false),
                            DateCreated = table.Column<DateTimeOffset>(nullable: false),
                            DateModified = table.Column<DateTimeOffset>(nullable: false),
                            SecretKey = table.Column<Guid>(nullable: false)
                        },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_SecretAccessor", x => x.Key);
                        table.ForeignKey(
                            name: "FK_SecretAccessor_Accessors_AccessorKey",
                            column: x => x.AccessorKey,
                            principalTable: "Accessors",
                            principalColumn: "Key",
                            onDelete: ReferentialAction.Cascade);
                        table.ForeignKey(
                            name: "FK_SecretAccessor_Secrets_SecretKey",
                            column: x => x.SecretKey,
                            principalTable: "Secrets",
                            principalColumn: "Key",
                            onDelete: ReferentialAction.Cascade);
                    });

            migrationBuilder.CreateIndex(name: "IX_Accessors_Email", table: "Accessors", column: "Email", unique: true);

            migrationBuilder.CreateIndex(name: "IX_Accessors_Login", table: "Accessors", column: "Login", unique: true);

            migrationBuilder.CreateIndex(name: "IX_SecretAccessor_AccessorKey", table: "SecretAccessor", column: "AccessorKey");

            migrationBuilder.CreateIndex(name: "IX_SecretAccessor_SecretKey", table: "SecretAccessor", column: "SecretKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "SecretAccessor");

            migrationBuilder.DropTable(name: "Accessors");

            migrationBuilder.DropTable(name: "Secrets");
        }
    }
}
