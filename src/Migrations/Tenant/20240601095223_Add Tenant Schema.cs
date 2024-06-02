using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ROC.WebApi.Migrations.PostgreSQL.Tenant;

/// <inheritdoc />
public partial class AddTenantSchema : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            "tenant");

        migrationBuilder.CreateTable(
            "Tenants",
            schema: "tenant",
            columns: table => new
            {
                Id = table.Column<string>("character varying(64)", maxLength: 64, nullable: false),
                Identifier = table.Column<string>("text", nullable: false),
                Name = table.Column<string>("text", nullable: false),
                ConnectionString = table.Column<string>("text", nullable: false),
                AdminEmail = table.Column<string>("text", nullable: false),
                IsActive = table.Column<bool>("boolean", nullable: false),
                ValidUpto = table.Column<DateTime>("timestamp without time zone", nullable: false),
                Issuer = table.Column<string>("text", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_Tenants", x => x.Id); });

        migrationBuilder.CreateIndex(
            "IX_Tenants_Identifier",
            schema: "tenant",
            table: "Tenants",
            column: "Identifier",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Tenants",
            "tenant");
    }
}