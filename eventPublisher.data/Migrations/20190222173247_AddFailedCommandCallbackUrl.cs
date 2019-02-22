using Microsoft.EntityFrameworkCore.Migrations;

namespace eventPublisher.data.Migrations
{
    public partial class AddFailedCommandCallbackUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "failed_command_callback_url",
                table: "application_events",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_application_events_application_id",
                table: "application_events",
                column: "application_id");

            migrationBuilder.AddForeignKey(
                name: "fk_application_events_applications_application_id",
                table: "application_events",
                column: "application_id",
                principalTable: "applications",
                principalColumn: "application_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_application_events_applications_application_id",
                table: "application_events");

            migrationBuilder.DropIndex(
                name: "ix_application_events_application_id",
                table: "application_events");

            migrationBuilder.DropColumn(
                name: "failed_command_callback_url",
                table: "application_events");
        }
    }
}
