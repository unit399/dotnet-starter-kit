using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ROC.WebApi.Migrations.PostgreSQL.Catalog;

/// <inheritdoc />
public partial class AddCatalogSchema : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            "catalog");

        migrationBuilder.CreateTable(
            "Products",
            schema: "catalog",
            columns: table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                Name = table.Column<string>("character varying(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                Price = table.Column<decimal>("numeric", nullable: false),
                TenantId = table.Column<string>("character varying(64)", maxLength: 64, nullable: false),
                Created = table.Column<DateTimeOffset>("timestamp with time zone", nullable: false),
                CreatedBy = table.Column<Guid>("uuid", nullable: false),
                LastModified = table.Column<DateTimeOffset>("timestamp with time zone", nullable: false),
                LastModifiedBy = table.Column<Guid>("uuid", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_Products", x => x.Id); });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Products",
            "catalog");
    }
}