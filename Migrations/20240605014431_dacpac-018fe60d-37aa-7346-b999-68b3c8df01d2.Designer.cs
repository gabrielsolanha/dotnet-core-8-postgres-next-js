﻿// <auto-generated />
using System;
using AplicacaoWeb.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AplicacaoWeb.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240605014431_dacpac-018fe60d-37aa-7346-b999-68b3c8df01d2")]
    partial class dacpac018fe60d37aa7346b99968b3c8df01d2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.Filme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("Ativo");

                    b.Property<string>("Plate")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Titulo");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserResponsibleId")
                        .HasColumnType("integer")
                        .HasColumnName("User");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Plate")
                        .IsUnique();

                    b.HasIndex("UserResponsibleId");

                    b.ToTable("Filmes", (string)null);
                });

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.ImageUrlAndName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ArquiveName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FilmeId")
                        .HasColumnType("integer")
                        .HasColumnName("FilmId");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("Ativo");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FilmeId");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("ImageUrlAndNames", (string)null);
                });

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.Screen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("Ativo");

                    b.Property<string>("ScreenName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Name");

                    b.Property<string>("ScreenType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ScreenUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ScreenUrl")
                        .IsUnique();

                    b.ToTable("Screen", (string)null);
                });

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CallMeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("Ativo");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Password");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("UserName");

                    b.HasKey("Id");

                    b.HasIndex("Telefone")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.UserScreen", b =>
                {
                    b.Property<int>("ScreenId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("ScreenId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserScreen", (string)null);
                });

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.Filme", b =>
                {
                    b.HasOne("AplicacaoWeb.Models.Entities.Category", "Category")
                        .WithMany("Filmes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AplicacaoWeb.Models.Entities.User", "UserResponsible")
                        .WithMany("Filmes")
                        .HasForeignKey("UserResponsibleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("UserResponsible");
                });

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.ImageUrlAndName", b =>
                {
                    b.HasOne("AplicacaoWeb.Models.Entities.Filme", "Filme")
                        .WithMany("Images")
                        .HasForeignKey("FilmeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Filme");
                });

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.UserScreen", b =>
                {
                    b.HasOne("AplicacaoWeb.Models.Entities.Screen", "Screens")
                        .WithMany()
                        .HasForeignKey("ScreenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AplicacaoWeb.Models.Entities.User", "Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Screens");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.Category", b =>
                {
                    b.Navigation("Filmes");
                });

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.Filme", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("AplicacaoWeb.Models.Entities.User", b =>
                {
                    b.Navigation("Filmes");
                });
#pragma warning restore 612, 618
        }
    }
}
