﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WasteManagement.API.Data;

#nullable disable

namespace Waste.API.Migrations
{
    [DbContext(typeof(WasteContext))]
    partial class WasteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Waste.API.Models.WasteJourney", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Adjustment")
                        .HasColumnType("float");

                    b.Property<bool?>("BaledWithWire")
                        .HasColumnType("bit");

                    b.Property<bool?>("Completed")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("DeductionAmount")
                        .HasColumnType("float");

                    b.Property<string>("DoneWaste")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Month")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Quantity")
                        .HasColumnType("float");

                    b.Property<string>("ReferenceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Total")
                        .HasColumnType("float");

                    b.Property<int?>("WasteSubTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("WasteTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WasteSubTypeId");

                    b.HasIndex("WasteTypeId");

                    b.ToTable("WasteJourney");
                });

            modelBuilder.Entity("Waste.API.Models.WasteSubType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Adjustment")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("WasteTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WasteTypeId");

                    b.ToTable("WasteSubType");
                });

            modelBuilder.Entity("Waste.API.Models.WasteType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("WasteType");
                });

            modelBuilder.Entity("Waste.API.Models.WasteJourney", b =>
                {
                    b.HasOne("Waste.API.Models.WasteSubType", "WasteSubType")
                        .WithMany("WasteJourneys")
                        .HasForeignKey("WasteSubTypeId");

                    b.HasOne("Waste.API.Models.WasteType", "WasteType")
                        .WithMany("Journeys")
                        .HasForeignKey("WasteTypeId");

                    b.Navigation("WasteSubType");

                    b.Navigation("WasteType");
                });

            modelBuilder.Entity("Waste.API.Models.WasteSubType", b =>
                {
                    b.HasOne("Waste.API.Models.WasteType", "WasteType")
                        .WithMany("SubTypes")
                        .HasForeignKey("WasteTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WasteType");
                });

            modelBuilder.Entity("Waste.API.Models.WasteSubType", b =>
                {
                    b.Navigation("WasteJourneys");
                });

            modelBuilder.Entity("Waste.API.Models.WasteType", b =>
                {
                    b.Navigation("Journeys");

                    b.Navigation("SubTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
