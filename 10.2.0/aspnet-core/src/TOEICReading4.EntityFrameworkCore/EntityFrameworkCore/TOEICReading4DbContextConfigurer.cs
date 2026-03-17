using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace TOEICReading4.EntityFrameworkCore;

public static class TOEICReading4DbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<TOEICReading4DbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<TOEICReading4DbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}
