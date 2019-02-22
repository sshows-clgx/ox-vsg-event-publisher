using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eventPublisher.data.Migrations
{
    public partial class SubscriptionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    event_id = table.Column<int>(nullable: false),
                    application_id = table.Column<long>(nullable: false),
                    callback_url = table.Column<string>(nullable: true),
                    inserted_utc = table.Column<DateTime>(nullable: false, defaultValueSql: "now() at time zone 'utc'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscriptions", x => new { x.event_id, x.application_id });
                    table.ForeignKey(
                        name: "fk_subscriptions_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "applications",
                        principalColumn: "application_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_subscriptions_application_events_event_id",
                        column: x => x.event_id,
                        principalTable: "application_events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_subscriptions_application_id",
                table: "subscriptions",
                column: "application_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "subscriptions");
        }
    }
}
