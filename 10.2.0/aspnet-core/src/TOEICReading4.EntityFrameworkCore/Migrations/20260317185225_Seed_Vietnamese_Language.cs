using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TOEICReading4.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Vietnamese_Language : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
IF NOT EXISTS (
    SELECT 1
    FROM AbpLanguages
    WHERE TenantId IS NULL AND Name = 'vi'
)
BEGIN
    INSERT INTO AbpLanguages (Name, DisplayName, Icon, IsDisabled, TenantId, CreationTime, IsDeleted)
    VALUES ('vi', N'Tiếng Việt', 'famfamfam-flags vn', 0, NULL, GETUTCDATE(), 0)
END
"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
DELETE FROM AbpLanguages
WHERE TenantId IS NULL AND Name = 'vi'
"
            );
        }
    }
}
