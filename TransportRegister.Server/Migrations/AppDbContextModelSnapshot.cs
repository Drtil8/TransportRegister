﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TransportRegister.Server.Data;

#nullable disable

namespace TransportRegister.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TransportRegister.Server.Models.DriversLicense", b =>
                {
                    b.Property<int>("DriversLicenseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DriversLicenseId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("IssuedOn")
                        .HasColumnType("date");

                    b.Property<int>("VehicleType")
                        .HasColumnType("int");

                    b.HasKey("DriversLicenseId");

                    b.HasIndex("DriverId");

                    b.ToTable("DriversLicenses");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Fine", b =>
                {
                    b.Property<int>("FineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FineId"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateOnly>("DueDate")
                        .HasColumnType("date");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("OffenceId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("PaidOn")
                        .HasColumnType("date");

                    b.HasKey("FineId");

                    b.HasIndex("OffenceId")
                        .IsUnique();

                    b.ToTable("Fines");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.LicensePlateHistory", b =>
                {
                    b.Property<int>("LicensePlateHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LicensePlateHistoryId"));

                    b.Property<DateTime>("ChangedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LicensePlate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("LicensePlateHistoryId");

                    b.HasIndex("VehicleId");

                    b.ToTable("LicensePlates");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Offence", b =>
                {
                    b.Property<int>("OffenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OffenceId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsValid")
                        .HasColumnType("bit");

                    b.Property<int>("OffenceTypeId")
                        .HasColumnType("int");

                    b.Property<string>("OfficerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OfficialId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PenaltyPoints")
                        .HasColumnType("int");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReportedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("OffenceId");

                    b.HasIndex("OffenceTypeId");

                    b.HasIndex("OfficerId");

                    b.HasIndex("OfficialId");

                    b.HasIndex("PersonId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Offences");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.OffencePhoto", b =>
                {
                    b.Property<int>("OffencePhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OffencePhotoId"));

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("OffenceId")
                        .HasColumnType("int");

                    b.HasKey("OffencePhotoId");

                    b.HasIndex("OffenceId");

                    b.ToTable("OffencePhotos");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.OffenceType", b =>
                {
                    b.Property<int>("OffenceTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OffenceTypeId"));

                    b.Property<double>("FineAmount")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PenaltyPoints")
                        .HasColumnType("int");

                    b.HasKey("OffenceTypeId");

                    b.ToTable("OffenceTypes");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonId"));

                    b.Property<string>("BirthNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfficialId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PersonType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Sex_Male")
                        .HasColumnType("bit");

                    b.HasKey("PersonId");

                    b.HasIndex("OfficialId");

                    b.ToTable("Persons", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Theft", b =>
                {
                    b.Property<int>("TheftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TheftId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FoundOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("OfficialId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ReportedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReportingOfficerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ReportingPersonId")
                        .HasColumnType("int");

                    b.Property<string>("ResolvingOfficerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ReturnedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StolenOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("TheftId");

                    b.HasIndex("OfficialId");

                    b.HasIndex("ReportingOfficerId");

                    b.HasIndex("ReportingPersonId");

                    b.HasIndex("ResolvingOfficerId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Thefts");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsValid")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users", (string)null);

                    b.HasDiscriminator<string>("UserType").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Vehicle", b =>
                {
                    b.Property<int>("VehicleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VehicleId"));

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("EngineVolume_CM3")
                        .HasColumnType("float");

                    b.Property<double>("Height_CM")
                        .HasColumnType("float");

                    b.Property<double>("Horsepower_KW")
                        .HasColumnType("float");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<double>("Length_CM")
                        .HasColumnType("float");

                    b.Property<double>("LoadCapacity_KG")
                        .HasColumnType("float");

                    b.Property<int>("ManufacturedYear")
                        .HasColumnType("int");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfficialId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("VIN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VehicleType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<double>("Width_CM")
                        .HasColumnType("float");

                    b.HasKey("VehicleId");

                    b.HasIndex("OfficialId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Vehicles", (string)null);

                    b.HasDiscriminator<string>("VehicleType").HasValue("VehicleBase");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Driver", b =>
                {
                    b.HasBaseType("TransportRegister.Server.Models.Person");

                    b.Property<int>("BadPoints")
                        .HasColumnType("int");

                    b.Property<string>("DriversLicenseNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DrivingSuspendedUntil")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasSuspendedLicense")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastCrimeCommited")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastPointsDeduction")
                        .HasColumnType("datetime2");

                    b.ToTable("Drivers", (string)null);
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Officer", b =>
                {
                    b.HasBaseType("TransportRegister.Server.Models.User");

                    b.Property<int>("PersonalId")
                        .HasColumnType("int");

                    b.Property<string>("Rank")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Officer");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Official", b =>
                {
                    b.HasBaseType("TransportRegister.Server.Models.User");

                    b.HasDiscriminator().HasValue("Official");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Bus", b =>
                {
                    b.HasBaseType("TransportRegister.Server.Models.Vehicle");

                    b.Property<int>("SeatCapacity")
                        .HasColumnType("int");

                    b.Property<int>("StandingCapacity")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Bus");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Car", b =>
                {
                    b.HasBaseType("TransportRegister.Server.Models.Vehicle");

                    b.Property<int>("NumberOfDoors")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Car");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Motorcycle", b =>
                {
                    b.HasBaseType("TransportRegister.Server.Models.Vehicle");

                    b.Property<string>("Constraints")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Motorcycle");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Truck", b =>
                {
                    b.HasBaseType("TransportRegister.Server.Models.Vehicle");

                    b.HasDiscriminator().HasValue("Truck");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransportRegister.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TransportRegister.Server.Models.DriversLicense", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.Driver", "IssuedFor")
                        .WithMany("Licenses")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IssuedFor");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Fine", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.Offence", "Offence")
                        .WithOne("Fine")
                        .HasForeignKey("TransportRegister.Server.Models.Fine", "OffenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Offence");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.LicensePlateHistory", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.Vehicle", "Vehicle")
                        .WithMany("LicensePlates")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Offence", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.OffenceType", "OffenceType")
                        .WithMany()
                        .HasForeignKey("OffenceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransportRegister.Server.Models.Officer", "ReportedByOfficer")
                        .WithMany("ReportedOffences")
                        .HasForeignKey("OfficerId");

                    b.HasOne("TransportRegister.Server.Models.Official", "ProcessedByOfficial")
                        .WithMany("ProcessedOffences")
                        .HasForeignKey("OfficialId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("TransportRegister.Server.Models.Person", "CommitedBy")
                        .WithMany("CommitedOffences")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransportRegister.Server.Models.Vehicle", "OffenceOnVehicle")
                        .WithMany("Offences")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.OwnsOne("TransportRegister.Server.Models.Address", "Address", b1 =>
                        {
                            b1.Property<int>("OffenceId")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("HouseNumber")
                                .HasColumnType("int");

                            b1.Property<int>("PostalCode")
                                .HasColumnType("int");

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("OffenceId");

                            b1.ToTable("Offences");

                            b1.WithOwner()
                                .HasForeignKey("OffenceId");
                        });

                    b.Navigation("Address");

                    b.Navigation("CommitedBy");

                    b.Navigation("OffenceOnVehicle");

                    b.Navigation("OffenceType");

                    b.Navigation("ProcessedByOfficial");

                    b.Navigation("ReportedByOfficer");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.OffencePhoto", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.Offence", "Offence")
                        .WithMany("Photos")
                        .HasForeignKey("OffenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Offence");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Person", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.Official", "AddedByOfficial")
                        .WithMany("AddedPersons")
                        .HasForeignKey("OfficialId");

                    b.OwnsOne("TransportRegister.Server.Models.Address", "Address", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("HouseNumber")
                                .HasColumnType("int");

                            b1.Property<int>("PostalCode")
                                .HasColumnType("int");

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("PersonId");

                            b1.ToTable("Persons");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.Navigation("AddedByOfficial");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Theft", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.Official", "ProcessedByOfficial")
                        .WithMany("ProcessedThefts")
                        .HasForeignKey("OfficialId");

                    b.HasOne("TransportRegister.Server.Models.Officer", "ReportedByOfficer")
                        .WithMany("ReportedThefts")
                        .HasForeignKey("ReportingOfficerId");

                    b.HasOne("TransportRegister.Server.Models.Person", "ReportedByPerson")
                        .WithMany("ReportedThefts")
                        .HasForeignKey("ReportingPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransportRegister.Server.Models.Officer", "ResolvedByOfficer")
                        .WithMany("ResolvedThefts")
                        .HasForeignKey("ResolvingOfficerId");

                    b.HasOne("TransportRegister.Server.Models.Vehicle", "StolenVehicle")
                        .WithMany("Thefts")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.OwnsOne("TransportRegister.Server.Models.Address", "Address", b1 =>
                        {
                            b1.Property<int>("TheftId")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("HouseNumber")
                                .HasColumnType("int");

                            b1.Property<int>("PostalCode")
                                .HasColumnType("int");

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("TheftId");

                            b1.ToTable("Thefts");

                            b1.WithOwner()
                                .HasForeignKey("TheftId");
                        });

                    b.Navigation("Address");

                    b.Navigation("ProcessedByOfficial");

                    b.Navigation("ReportedByOfficer");

                    b.Navigation("ReportedByPerson");

                    b.Navigation("ResolvedByOfficer");

                    b.Navigation("StolenVehicle");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Vehicle", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.Official", "AddedByOfficial")
                        .WithMany("AddedVehicles")
                        .HasForeignKey("OfficialId");

                    b.HasOne("TransportRegister.Server.Models.Person", "Owner")
                        .WithMany("Vehicles")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AddedByOfficial");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Driver", b =>
                {
                    b.HasOne("TransportRegister.Server.Models.Person", null)
                        .WithOne()
                        .HasForeignKey("TransportRegister.Server.Models.Driver", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Offence", b =>
                {
                    b.Navigation("Fine");

                    b.Navigation("Photos");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Person", b =>
                {
                    b.Navigation("CommitedOffences");

                    b.Navigation("ReportedThefts");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Vehicle", b =>
                {
                    b.Navigation("LicensePlates");

                    b.Navigation("Offences");

                    b.Navigation("Thefts");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Driver", b =>
                {
                    b.Navigation("Licenses");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Officer", b =>
                {
                    b.Navigation("ReportedOffences");

                    b.Navigation("ReportedThefts");

                    b.Navigation("ResolvedThefts");
                });

            modelBuilder.Entity("TransportRegister.Server.Models.Official", b =>
                {
                    b.Navigation("AddedPersons");

                    b.Navigation("AddedVehicles");

                    b.Navigation("ProcessedOffences");

                    b.Navigation("ProcessedThefts");
                });
#pragma warning restore 612, 618
        }
    }
}
