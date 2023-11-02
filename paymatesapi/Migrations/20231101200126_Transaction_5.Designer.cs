﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using paymatesapi.Contexts;

#nullable disable

namespace paymatesapi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231101200126_Transaction_5")]
    partial class Transaction_5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("paymatesapi.Entities.Friend", b =>
                {
                    b.Property<int>("FriendId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FriendOneUid")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FriendTwoUid")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("FriendId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("paymatesapi.Entities.Transaction", b =>
                {
                    b.Property<string>("Uid")
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreditorUid")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("DebtorUid")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("FriendPairFriendId")
                        .HasColumnType("int");

                    b.Property<string>("Icon")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Uid");

                    b.HasIndex("FriendPairFriendId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("paymatesapi.Entities.User", b =>
                {
                    b.Property<string>("Uid")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("varchar(70)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("varchar(70)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("longtext");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RefreshTokenExpiry")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Uid");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("paymatesapi.Entities.Transaction", b =>
                {
                    b.HasOne("paymatesapi.Entities.Friend", "FriendPair")
                        .WithMany("Transactions")
                        .HasForeignKey("FriendPairFriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FriendPair");
                });

            modelBuilder.Entity("paymatesapi.Entities.Friend", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
