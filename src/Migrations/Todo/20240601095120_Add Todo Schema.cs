using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ROC.WebApi.Migrations.PostgreSQL.Todo;

/// <inheritdoc />
public partial class AddTodoSchema : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            "todo");

        migrationBuilder.CreateTable(
            "Todos",
            schema: "todo",
            columns: table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                Title = table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                Note = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                TenantId = table.Column<string>("character varying(64)", maxLength: 64, nullable: false),
                Created = table.Column<DateTimeOffset>("timestamp with time zone", nullable: false),
                CreatedBy = table.Column<Guid>("uuid", nullable: false),
                LastModified = table.Column<DateTimeOffset>("timestamp with time zone", nullable: false),
                LastModifiedBy = table.Column<Guid>("uuid", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_Todos", x => x.Id); });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Todos",
            "todo");
    }
}