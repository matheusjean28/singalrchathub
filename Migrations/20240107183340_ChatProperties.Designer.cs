﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserContext;

#nullable disable

namespace ChatSignalR.Migrations
{
    [DbContext(typeof(UserDbContext))]
    [Migration("20240107183340_ChatProperties")]
    partial class ChatProperties
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.14");

            modelBuilder.Entity("ChatModel.Chat", b =>
                {
                    b.Property<string>("ChatID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChatName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OnlineUser")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ChatID");

                    b.HasIndex("UserId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("UserModel.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChatID")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentChatId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentConnectionId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Gener")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Pass")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChatID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatModel.Chat", b =>
                {
                    b.HasOne("UserModel.User", "Owner")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("UserModel.User", b =>
                {
                    b.HasOne("ChatModel.Chat", null)
                        .WithMany("Users")
                        .HasForeignKey("ChatID");
                });

            modelBuilder.Entity("ChatModel.Chat", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}