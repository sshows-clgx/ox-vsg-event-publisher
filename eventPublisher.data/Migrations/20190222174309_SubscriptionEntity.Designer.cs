﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using eventPublisher.data;

namespace eventPublisher.data.Migrations
{
    [DbContext(typeof(EventPublisherContext))]
    [Migration("20190222174309_SubscriptionEntity")]
    partial class SubscriptionEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("eventPublisher.data.entities.ApplicationEntity", b =>
                {
                    b.Property<long>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("application_id");

                    b.Property<DateTime>("InsertedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("inserted_utc")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.HasKey("ApplicationId")
                        .HasName("pk_applications");

                    b.ToTable("applications");
                });

            modelBuilder.Entity("eventPublisher.data.entities.ApplicationEventEntity", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("event_id");

                    b.Property<long>("ApplicationId")
                        .HasColumnName("application_id");

                    b.Property<string>("FailedCommandCallbackUrl")
                        .HasColumnName("failed_command_callback_url");

                    b.Property<DateTime>("InsertedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("inserted_utc")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<int>("TopicId")
                        .HasColumnName("topic_id");

                    b.HasKey("EventId")
                        .HasName("pk_application_events");

                    b.HasIndex("ApplicationId")
                        .HasName("ix_application_events_application_id");

                    b.HasIndex("TopicId")
                        .HasName("ix_application_events_topic_id");

                    b.ToTable("application_events");
                });

            modelBuilder.Entity("eventPublisher.data.entities.SubscriptionEntity", b =>
                {
                    b.Property<int>("EventId")
                        .HasColumnName("event_id");

                    b.Property<long>("ApplicationId")
                        .HasColumnName("application_id");

                    b.Property<string>("CallbackUrl")
                        .HasColumnName("callback_url");

                    b.Property<DateTime>("InsertedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("inserted_utc")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.HasKey("EventId", "ApplicationId")
                        .HasName("pk_subscriptions");

                    b.HasIndex("ApplicationId")
                        .HasName("ix_subscriptions_application_id");

                    b.ToTable("subscriptions");
                });

            modelBuilder.Entity("eventPublisher.data.entities.TopicEntity", b =>
                {
                    b.Property<int>("TopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("topic_id");

                    b.Property<DateTime>("InsertedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("inserted_utc")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.HasKey("TopicId")
                        .HasName("pk_topics");

                    b.ToTable("topics");
                });

            modelBuilder.Entity("eventPublisher.data.entities.ApplicationEventEntity", b =>
                {
                    b.HasOne("eventPublisher.data.entities.ApplicationEntity", "ApplicationNav")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .HasConstraintName("fk_application_events_applications_application_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eventPublisher.data.entities.TopicEntity", "TopicNav")
                        .WithMany("ApplicationEvents")
                        .HasForeignKey("TopicId")
                        .HasConstraintName("fk_application_events_topics_topic_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eventPublisher.data.entities.SubscriptionEntity", b =>
                {
                    b.HasOne("eventPublisher.data.entities.ApplicationEntity", "ApplicationNav")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .HasConstraintName("fk_subscriptions_applications_application_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eventPublisher.data.entities.ApplicationEventEntity", "ApplicationEventNav")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .HasConstraintName("fk_subscriptions_application_events_event_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
