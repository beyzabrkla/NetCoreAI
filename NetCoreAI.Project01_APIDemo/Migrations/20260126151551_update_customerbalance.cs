using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCoreAI.Project01_APIDemo.Migrations
{
    /// <inheritdoc />
    public partial class update_customerbalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CustomerBalance",
                table: "Customers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CustomerBalance",
                table: "Customers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
