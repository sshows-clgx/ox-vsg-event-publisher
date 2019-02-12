using Microsoft.EntityFrameworkCore.Migrations;

namespace eventPublisher.data.Migrations
{
    public partial class EventForeignKeys2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_application_events_topic_id",
                table: "application_events",
                column: "topic_id");

            migrationBuilder.AddForeignKey(
                name: "fk_application_events_topics_topic_id",
                table: "application_events",
                column: "topic_id",
                principalTable: "topics",
                principalColumn: "topic_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_application_events_topics_topic_id",
                table: "application_events");

            migrationBuilder.DropIndex(
                name: "ix_application_events_topic_id",
                table: "application_events");
        }
    }
}
