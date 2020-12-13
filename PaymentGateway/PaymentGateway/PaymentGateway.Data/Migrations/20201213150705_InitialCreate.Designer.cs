﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaymentGateway.Data;

namespace PaymentGateway.Data.Migrations
{
    [DbContext(typeof(PaymentContext))]
    [Migration("20201213150705_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("PaymentGateway.Data.Entities.Authorization", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Void")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Authorizations");
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.Payment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("AuthorizationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorizationId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.Refund", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("AuthorizationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorizationId");

                    b.ToTable("Refund");
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.Payment", b =>
                {
                    b.HasOne("PaymentGateway.Data.Entities.Authorization", null)
                        .WithMany("Payments")
                        .HasForeignKey("AuthorizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.Refund", b =>
                {
                    b.HasOne("PaymentGateway.Data.Entities.Authorization", null)
                        .WithMany("Refunds")
                        .HasForeignKey("AuthorizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.Authorization", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Refunds");
                });
#pragma warning restore 612, 618
        }
    }
}
