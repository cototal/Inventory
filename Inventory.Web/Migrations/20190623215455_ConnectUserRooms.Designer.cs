﻿// <auto-generated />
using System;
using Inventory.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inventory.Web.Migrations
{
    [DbContext(typeof(InventoryContext))]
    [Migration("20190623215455_ConnectUserRooms")]
    partial class ConnectUserRooms
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Inventory.Web.Models.Borrower", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateLent");

                    b.Property<int>("ItemId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("Borrowers");
                });

            modelBuilder.Entity("Inventory.Web.Models.Container", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("RoomId");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Containers");
                });

            modelBuilder.Entity("Inventory.Web.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ContainerId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ContainerId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Inventory.Web.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Inventory.Web.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Inventory.Web.Models.Borrower", b =>
                {
                    b.HasOne("Inventory.Web.Models.Item", "Item")
                        .WithMany("Borrowers")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inventory.Web.Models.Container", b =>
                {
                    b.HasOne("Inventory.Web.Models.Room", "Room")
                        .WithMany("Containers")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inventory.Web.Models.Item", b =>
                {
                    b.HasOne("Inventory.Web.Models.Container", "Container")
                        .WithMany("Items")
                        .HasForeignKey("ContainerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inventory.Web.Models.Room", b =>
                {
                    b.HasOne("Inventory.Web.Models.User", "User")
                        .WithMany("Rooms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
