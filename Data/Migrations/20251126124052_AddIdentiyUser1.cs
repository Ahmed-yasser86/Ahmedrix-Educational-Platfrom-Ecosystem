using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace OnlineCoursesPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentiyUser1 : Migration
    {
        /// <inheritdoc />
        /// 
        const string ADMIN_USER_GUID = "b4c471d6-db5f-44a9-9359-0d7eca9b2c1d";
        const string ADMIN_ROLE_GUID = "44a12b34-a14c-4987-a1d4-1d16a2f33c8d";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var passwordHash = hasher.HashPassword(null, "Password100!");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT INTO AspNetUsers(" +
                "Id, UserName, NormalizedUserName, Email, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, " +
                "LockoutEnabled, AccessFailedCount, NormalizedEmail, PasswordHash, SecurityStamp, FirstName, LastName, " +
                "Address1, Address2, PostCode)");

            sb.AppendLine("VALUES(");
            sb.AppendLine($"'{ADMIN_USER_GUID}'");                         // Id
            sb.AppendLine(",'admin@techtree.co.uk'");                     // UserName
            sb.AppendLine(",'ADMIN@TECHTREE.CO.UK'");                     // NormalizedUserName
            sb.AppendLine(",'admin@techtree.co.uk'");                     // Email
            sb.AppendLine(", 0");                                         // EmailConfirmed
            sb.AppendLine(", 0");                                         // PhoneNumberConfirmed
            sb.AppendLine(", 0");                                         // TwoFactorEnabled
            sb.AppendLine(", 0");                                         // LockoutEnabled
            sb.AppendLine(", 0");                                         // AccessFailedCount
            sb.AppendLine(",'ADMIN@TECHTREE.CO.UK'");                     // NormalizedEmail
            sb.AppendLine($", '{passwordHash}'");                         // PasswordHash
            sb.AppendLine($", '{Guid.NewGuid()}'");                       // SecurityStamp
            sb.AppendLine(",'Admin'");                                    // FirstName
            sb.AppendLine(",'Admin'");                                    // LastName
            sb.AppendLine(",'Default Address 1'");                        // Address1
            sb.AppendLine(",'Default Address 2'");                        // Address2
            sb.AppendLine(",'00000'");                                    // PostCode
            sb.AppendLine(");");

            migrationBuilder.Sql(sb.ToString());

            // Create Admin role
            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('{ADMIN_ROLE_GUID}','Admin','ADMIN')");

            // Assign Admin role to admin user
            migrationBuilder.Sql($"INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('{ADMIN_USER_GUID}','{ADMIN_ROLE_GUID}')");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId = '{ADMIN_USER_GUID}' AND RoleId = '{ADMIN_ROLE_GUID}'");

            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id = '{ADMIN_USER_GUID}'");

            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{ADMIN_ROLE_GUID}'");
        }
    }
}
