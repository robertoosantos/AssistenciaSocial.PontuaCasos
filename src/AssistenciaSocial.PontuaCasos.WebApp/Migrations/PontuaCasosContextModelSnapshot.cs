﻿// <auto-generated />
using System;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    [DbContext(typeof(PontuaCasosContext))]
    partial class PontuaCasosContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Ativo")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT");

                    b.Property<int>("CriadoPorId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModificadoEm")
                        .HasColumnType("TEXT");

                    b.Property<int>("ModificadoPorId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Multiplo")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrganizacaoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Pontos")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CriadoPorId");

                    b.HasIndex("ItemId");

                    b.HasIndex("ModificadoPorId");

                    b.HasIndex("OrganizacaoId");

                    b.ToTable("Itens");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT");

                    b.Property<int>("CriadoPorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModificadoEm")
                        .HasColumnType("TEXT");

                    b.Property<int>("ModificadoPorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CriadoPorId");

                    b.HasIndex("ModificadoPorId");

                    b.ToTable("Organizacoes");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT");

                    b.Property<int>("CriadoPorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModificadoEm")
                        .HasColumnType("TEXT");

                    b.Property<int>("ModificadoPorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("OrganizacaoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CriadoPorId");

                    b.HasIndex("ModificadoPorId");

                    b.HasIndex("OrganizacaoId");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("MembrosOrganizacao", b =>
                {
                    b.Property<int>("OrganizacaoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("OrganizacaoId", "UsuarioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("MembrosOrganizacao");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "CriadoPor")
                        .WithMany()
                        .HasForeignKey("CriadoPorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", null)
                        .WithMany("Itens")
                        .HasForeignKey("ItemId");

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "ModificadoPor")
                        .WithMany()
                        .HasForeignKey("ModificadoPorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", "Organizacao")
                        .WithMany()
                        .HasForeignKey("OrganizacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CriadoPor");

                    b.Navigation("ModificadoPor");

                    b.Navigation("Organizacao");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "CriadoPor")
                        .WithMany()
                        .HasForeignKey("CriadoPorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "ModificadoPor")
                        .WithMany()
                        .HasForeignKey("ModificadoPorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CriadoPor");

                    b.Navigation("ModificadoPor");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "CriadoPor")
                        .WithMany()
                        .HasForeignKey("CriadoPorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "ModificadoPor")
                        .WithMany()
                        .HasForeignKey("ModificadoPorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", null)
                        .WithMany("Administradores")
                        .HasForeignKey("OrganizacaoId");

                    b.Navigation("CriadoPor");

                    b.Navigation("ModificadoPor");
                });

            modelBuilder.Entity("MembrosOrganizacao", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", null)
                        .WithMany()
                        .HasForeignKey("OrganizacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_MembrosOrganizacao_Organizacoes_OrganizacaoId");

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_MembrosOrganizacao_Usuarios_UsuarioId");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", b =>
                {
                    b.Navigation("Itens");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", b =>
                {
                    b.Navigation("Administradores");
                });
#pragma warning restore 612, 618
        }
    }
}