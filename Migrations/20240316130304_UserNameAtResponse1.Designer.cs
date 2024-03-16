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
    [Migration("20240316130304_UserNameAtResponse1")]
    partial class UserNameAtResponse1
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

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ChatID");

                    b.HasIndex("OwnerId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("ChatSignalR.Models.PermisionsChat.UserPermissionData", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChatID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PermissionLevel")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChatID");

                    b.ToTable("UserPermission");
                });

            modelBuilder.Entity("ChatSignalR.Models.WrapperChat.WrapperChat", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChatName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("WrapperChat");
                });

            modelBuilder.Entity("UserModel.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChatID")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentChatId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentConnectionId")
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

                    b.Property<int>("UserPermissionLevel")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ChatID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatModel.Chat", b =>
                {
                    b.HasOne("UserModel.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ChatSignalR.Models.PermisionsChat.UserPermissionData", b =>
                {
                    b.HasOne("ChatModel.Chat", null)
                        .WithMany("UserPermissionList")
                        .HasForeignKey("ChatID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ChatSignalR.Models.WrapperChat.WrapperChat", b =>
                {
                    b.HasOne("UserModel.User", null)
                        .WithMany("MyOwnsChatIds")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("UserModel.User", b =>
                {
                    b.HasOne("ChatModel.Chat", null)
                        .WithMany("Users")
                        .HasForeignKey("ChatID");
                });

            modelBuilder.Entity("ChatModel.Chat", b =>
                {
                    b.Navigation("UserPermissionList");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("UserModel.User", b =>
                {
                    b.Navigation("MyOwnsChatIds");
                });
#pragma warning restore 612, 618
        }
    }
}
