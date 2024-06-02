using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ROC.WebApi.Migrations.PostgreSQL.Identity;

/// <inheritdoc />
public partial class AddIdentitySchema : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            "identity");

        migrationBuilder.CreateTable(
            "Roles",
            schema: "identity",
            columns: table => new
            {
                Id = table.Column<string>("text", nullable: false),
                Description = table.Column<string>("text", nullable: true),
                TenantId = table.Column<string>("character varying(64)", maxLength: 64, nullable: false),
                Name = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>("text", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_Roles", x => x.Id); });

        migrationBuilder.CreateTable(
            "Users",
            schema: "identity",
            columns: table => new
            {
                Id = table.Column<string>("text", nullable: false),
                FirstName = table.Column<string>("text", nullable: true),
                LastName = table.Column<string>("text", nullable: true),
                ImageUrl = table.Column<string>("text", nullable: true),
                IsActive = table.Column<bool>("boolean", nullable: false),
                RefreshToken = table.Column<string>("text", nullable: true),
                RefreshTokenExpiryTime = table.Column<DateTime>("timestamp with time zone", nullable: false),
                ObjectId = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                TenantId = table.Column<string>("character varying(64)", maxLength: 64, nullable: false),
                UserName = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>("boolean", nullable: false),
                PasswordHash = table.Column<string>("text", nullable: true),
                SecurityStamp = table.Column<string>("text", nullable: true),
                ConcurrencyStamp = table.Column<string>("text", nullable: true),
                PhoneNumber = table.Column<string>("text", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>("boolean", nullable: false),
                TwoFactorEnabled = table.Column<bool>("boolean", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>("timestamp with time zone", nullable: true),
                LockoutEnabled = table.Column<bool>("boolean", nullable: false),
                AccessFailedCount = table.Column<int>("integer", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

        migrationBuilder.CreateTable(
            "RoleClaims",
            schema: "identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                CreatedBy = table.Column<string>("text", nullable: true),
                CreatedOn = table.Column<DateTimeOffset>("timestamp with time zone", nullable: false),
                TenantId = table.Column<string>("character varying(64)", maxLength: 64, nullable: false),
                RoleId = table.Column<string>("text", nullable: false),
                ClaimType = table.Column<string>("text", nullable: true),
                ClaimValue = table.Column<string>("text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleClaims", x => x.Id);
                table.ForeignKey(
                    "FK_RoleClaims_Roles_RoleId",
                    x => x.RoleId,
                    principalSchema: "identity",
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserClaims",
            schema: "identity",
            columns: table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<string>("text", nullable: false),
                ClaimType = table.Column<string>("text", nullable: true),
                ClaimValue = table.Column<string>("text", nullable: true),
                TenantId = table.Column<string>("character varying(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserClaims", x => x.Id);
                table.ForeignKey(
                    "FK_UserClaims_Users_UserId",
                    x => x.UserId,
                    principalSchema: "identity",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserLogins",
            schema: "identity",
            columns: table => new
            {
                LoginProvider = table.Column<string>("text", nullable: false),
                ProviderKey = table.Column<string>("text", nullable: false),
                ProviderDisplayName = table.Column<string>("text", nullable: true),
                UserId = table.Column<string>("text", nullable: false),
                TenantId = table.Column<string>("character varying(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    "FK_UserLogins_Users_UserId",
                    x => x.UserId,
                    principalSchema: "identity",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserRoles",
            schema: "identity",
            columns: table => new
            {
                UserId = table.Column<string>("text", nullable: false),
                RoleId = table.Column<string>("text", nullable: false),
                TenantId = table.Column<string>("character varying(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    "FK_UserRoles_Roles_RoleId",
                    x => x.RoleId,
                    principalSchema: "identity",
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_UserRoles_Users_UserId",
                    x => x.UserId,
                    principalSchema: "identity",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserTokens",
            schema: "identity",
            columns: table => new
            {
                UserId = table.Column<string>("text", nullable: false),
                LoginProvider = table.Column<string>("text", nullable: false),
                Name = table.Column<string>("text", nullable: false),
                Value = table.Column<string>("text", nullable: true),
                TenantId = table.Column<string>("character varying(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    "FK_UserTokens_Users_UserId",
                    x => x.UserId,
                    principalSchema: "identity",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_RoleClaims_RoleId",
            schema: "identity",
            table: "RoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            "RoleNameIndex",
            schema: "identity",
            table: "Roles",
            columns: new[] { "NormalizedName", "TenantId" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_UserClaims_UserId",
            schema: "identity",
            table: "UserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            "IX_UserLogins_UserId",
            schema: "identity",
            table: "UserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            "IX_UserRoles_RoleId",
            schema: "identity",
            table: "UserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            "EmailIndex",
            schema: "identity",
            table: "Users",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            "UserNameIndex",
            schema: "identity",
            table: "Users",
            column: "NormalizedUserName",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "RoleClaims",
            "identity");

        migrationBuilder.DropTable(
            "UserClaims",
            "identity");

        migrationBuilder.DropTable(
            "UserLogins",
            "identity");

        migrationBuilder.DropTable(
            "UserRoles",
            "identity");

        migrationBuilder.DropTable(
            "UserTokens",
            "identity");

        migrationBuilder.DropTable(
            "Roles",
            "identity");

        migrationBuilder.DropTable(
            "Users",
            "identity");
    }
}