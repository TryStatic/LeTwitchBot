﻿// <auto-generated />
using System;
using LeTwitchBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LeTwitchBot.Migrations
{
    [DbContext(typeof(Storage))]
    partial class StorageModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("LeTwitchBot.Data.Models.ChannelVisitor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Currency")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<int>("TwitchID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TwitchUsername")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Visitors");
                });

            modelBuilder.Entity("LeTwitchBot.Data.Models.Visit", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("VisitDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("VisitorID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("VisitorID");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("LeTwitchBot.Data.Models.Visit", b =>
                {
                    b.HasOne("LeTwitchBot.Data.Models.ChannelVisitor", "Visitor")
                        .WithMany("Visits")
                        .HasForeignKey("VisitorID");

                    b.Navigation("Visitor");
                });

            modelBuilder.Entity("LeTwitchBot.Data.Models.ChannelVisitor", b =>
                {
                    b.Navigation("Visits");
                });
#pragma warning restore 612, 618
        }
    }
}
