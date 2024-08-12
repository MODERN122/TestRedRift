﻿// <auto-generated />
using System;
using DiceGameApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiceGameApp.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DiceGameApp.Shared.GameSession", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("boolean");

                    b.Property<string>("Player1Id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Player2Id")
                        .HasColumnType("text");

                    b.Property<int>("TurnNumber")
                        .HasColumnType("integer");

                    b.Property<string>("WinnerId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Player1Id");

                    b.HasIndex("Player2Id");

                    b.ToTable("GameSessions");
                });

            modelBuilder.Entity("DiceGameApp.Shared.Player", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("DiceGameApp.Shared.Turn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("DiceRoll1")
                        .HasColumnType("integer");

                    b.Property<int?>("DiceRoll2")
                        .HasColumnType("integer");

                    b.Property<int?>("DiceRoll3")
                        .HasColumnType("integer");

                    b.Property<string>("GameSessionId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<string>("PlayerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("SpecialDiceRoll")
                        .HasColumnType("integer");

                    b.Property<int?>("TurnNumber")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TurnTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("GameSessionId");

                    b.ToTable("Turns");
                });

            modelBuilder.Entity("DiceGameApp.Shared.GameSession", b =>
                {
                    b.HasOne("DiceGameApp.Shared.Player", "Player1")
                        .WithMany()
                        .HasForeignKey("Player1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DiceGameApp.Shared.Player", "Player2")
                        .WithMany()
                        .HasForeignKey("Player2Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Player1");

                    b.Navigation("Player2");
                });

            modelBuilder.Entity("DiceGameApp.Shared.Turn", b =>
                {
                    b.HasOne("DiceGameApp.Shared.GameSession", "GameSession")
                        .WithMany("Turns")
                        .HasForeignKey("GameSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameSession");
                });

            modelBuilder.Entity("DiceGameApp.Shared.GameSession", b =>
                {
                    b.Navigation("Turns");
                });
#pragma warning restore 612, 618
        }
    }
}
