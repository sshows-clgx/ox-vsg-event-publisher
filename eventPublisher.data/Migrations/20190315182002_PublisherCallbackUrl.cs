using Microsoft.EntityFrameworkCore.Migrations;

namespace eventPublisher.data.Migrations
{
    public partial class PublisherCallbackUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "failed_command_callback_url",
                table: "application_events",
                newName: "publisher_callback_url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "publisher_callback_url",
                table: "application_events",
                newName: "failed_command_callback_url");
        }
    }
}
