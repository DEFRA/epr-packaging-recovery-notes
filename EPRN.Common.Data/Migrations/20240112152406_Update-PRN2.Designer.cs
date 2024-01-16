﻿// <auto-generated />
using System;
using EPRN.Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EPRN.Waste.API.Migrations
{
    [DbContext(typeof(EPRNContext))]
    [Migration("20240112152406_Update-PRN2")]
    partial class UpdatePRN2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EPRN.Common.Data.DataModels.PackagingRecoveryNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CompletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SentTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SiteId")
                        .HasColumnType("int");

                    b.Property<double?>("Tonnes")
                        .HasColumnType("float");

                    b.Property<int?>("WasteSubTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("WasteTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WasteTypeId");

                    b.ToTable("PRN");
                });

            modelBuilder.Entity("EPRN.Common.Data.DataModels.WasteJourney", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Adjustment")
                        .HasColumnType("float");

                    b.Property<bool?>("BaledWithWire")
                        .HasColumnType("bit");

                    b.Property<double?>("BaledWithWireDeductionPercentage")
                        .HasColumnType("float");

                    b.Property<bool?>("Completed")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DoneWaste")
                        .HasColumnType("int");

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

                    b.Property<int?>("SiteId")
                        .HasColumnType("int");

                    b.Property<double?>("Tonnes")
                        .HasColumnType("float");

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

            modelBuilder.Entity("EPRN.Common.Data.DataModels.WasteSubType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Adjustment")
                        .HasColumnType("float");

                    b.Property<bool>("AdjustmentPercentageRequired")
                        .HasColumnType("bit");

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

            modelBuilder.Entity("EPRN.Common.Data.DataModels.WasteType", b =>
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

            modelBuilder.Entity("EPRN.Common.Data.DataModels.PackagingRecoveryNote", b =>
                {
                    b.HasOne("EPRN.Common.Data.DataModels.WasteType", "WasteType")
                        .WithMany()
                        .HasForeignKey("WasteTypeId");

                    b.Navigation("WasteType");
                });

            modelBuilder.Entity("EPRN.Common.Data.DataModels.WasteJourney", b =>
                {
                    b.HasOne("EPRN.Common.Data.DataModels.WasteSubType", "WasteSubType")
                        .WithMany("WasteJourneys")
                        .HasForeignKey("WasteSubTypeId");

                    b.HasOne("EPRN.Common.Data.DataModels.WasteType", "WasteType")
                        .WithMany("Journeys")
                        .HasForeignKey("WasteTypeId");

                    b.Navigation("WasteSubType");

                    b.Navigation("WasteType");
                });

            modelBuilder.Entity("EPRN.Common.Data.DataModels.WasteSubType", b =>
                {
                    b.HasOne("EPRN.Common.Data.DataModels.WasteType", "WasteType")
                        .WithMany("SubTypes")
                        .HasForeignKey("WasteTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WasteType");
                });

            modelBuilder.Entity("EPRN.Common.Data.DataModels.WasteSubType", b =>
                {
                    b.Navigation("WasteJourneys");
                });

            modelBuilder.Entity("EPRN.Common.Data.DataModels.WasteType", b =>
                {
                    b.Navigation("Journeys");

                    b.Navigation("SubTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
