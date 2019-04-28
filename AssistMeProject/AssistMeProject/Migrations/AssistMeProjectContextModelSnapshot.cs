﻿// <auto-generated />
using System;
using AssistMeProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AssistMeProject.Migrations
{
    [DbContext(typeof(AssistMeProjectContext))]
    partial class AssistMeProjectContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AssistMeProject.Models.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(30000);

                    b.Property<int>("QuestionID");

                    b.HasKey("Id");

                    b.HasIndex("QuestionID");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("AssistMeProject.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnswerId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(30000);

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("AssistMeProject.Models.Label", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("NumberOfTimes");

                    b.Property<string>("Tag");

                    b.HasKey("Id");

                    b.ToTable("Label");
                });

            modelBuilder.Entity("AssistMeProject.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AskAgain");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(30000);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("AssistMeProject.Models.QuestionLabel", b =>
                {
                    b.Property<int>("QuestionId");

                    b.Property<int>("LabelId");

                    b.HasKey("QuestionId", "LabelId");

                    b.HasIndex("LabelId");

                    b.ToTable("QuestionLabels");
                });

            modelBuilder.Entity("AssistMeProject.Models.Studio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Unit");

                    b.HasKey("Id");

                    b.ToTable("Studio");
                });

            modelBuilder.Entity("AssistMeProject.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CITY");

                    b.Property<string>("COUNTRY");

                    b.Property<string>("DESCRIPTION");

                    b.Property<string>("EMAIL");

                    b.Property<string>("GOOGLE_KEY");

                    b.Property<int>("INTERESTING_VOTES_RECEIVED");

                    b.Property<string>("INTERESTS_OR_KNOWLEDGE");

                    b.Property<int>("LEVEL");

                    b.Property<string>("PASSWORD");

                    b.Property<string>("PHOTO");

                    b.Property<int>("POSITIVE_VOTES_RECEIVED");

                    b.Property<int>("QUESTIONS_ANSWERED");

                    b.Property<int>("QUESTIONS_ASKED");

                    b.Property<string>("USERNAME");

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("AssistMeProject.Models.Answer", b =>
                {
                    b.HasOne("AssistMeProject.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AssistMeProject.Models.Comment", b =>
                {
                    b.HasOne("AssistMeProject.Models.Answer", "Answer")
                        .WithMany("Comments")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AssistMeProject.Models.QuestionLabel", b =>
                {
                    b.HasOne("AssistMeProject.Models.Label", "Label")
                        .WithMany("QuestionLabels")
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AssistMeProject.Models.Question", "Question")
                        .WithMany("QuestionLabels")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
