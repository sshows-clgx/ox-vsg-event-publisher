using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace eventPublisher.data.Migrations
{
    public partial class AddTopics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "topic_id",
                table: "application_events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    topic_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: true),
                    inserted_utc = table.Column<DateTime>(nullable: false, defaultValueSql: "now() at time zone 'utc'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topics", x => x.topic_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "topics");

            migrationBuilder.DropColumn(
                name: "topic_id",
                table: "application_events");
        }
    }
}
