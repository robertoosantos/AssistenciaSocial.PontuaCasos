﻿// <auto-generated />
using System;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    [DbContext(typeof(PontuaCasosContext))]
    [Migration("20221014180112_ProntuarioOpcional")]
    partial class ProntuarioOpcional
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Caso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("CriadoPorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("EmAtualizacao")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModificadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModificadoPorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("OrganizacaoId")
                        .HasColumnType("int");

                    b.Property<string>("Prontuario")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponsavelFamiliar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ValidoAte")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidoAte");

                    b.Property<DateTime>("ValidoDe")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidoDe");

                    b.HasKey("Id");

                    b.HasIndex("CriadoPorId");

                    b.HasIndex("ModificadoPorId");

                    b.HasIndex("OrganizacaoId");

                    b.ToTable("Casos", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb.UseHistoryTable("CasosHistorico");
                            ttb
                                .HasPeriodStart("ValidoDe")
                                .HasColumnName("ValidoDe");
                            ttb
                                .HasPeriodEnd("ValidoAte")
                                .HasColumnName("ValidoAte");
                        }
                    ));
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.IndividuoEmViolacao", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CasoId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValidoAte")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidoAte");

                    b.Property<DateTime>("ValidoDe")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidoDe");

                    b.HasKey("Id");

                    b.HasIndex("CasoId");

                    b.HasIndex("ItemId");

                    b.ToTable("IndividuosEmViolacoes", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb.UseHistoryTable("IndividuosEmViolacoesHistorico");
                            ttb
                                .HasPeriodStart("ValidoDe")
                                .HasColumnName("ValidoDe");
                            ttb
                                .HasPeriodEnd("ValidoAte")
                                .HasColumnName("ValidoAte");
                        }
                    ));
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<int?>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("CriadoPorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("ECategoria")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModificadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModificadoPorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OrganizacaoId")
                        .HasColumnType("int");

                    b.Property<int>("Pontos")
                        .HasColumnType("int");

                    b.Property<int?>("RelacionadoAoId")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("UnicaPorAtendido")
                        .HasColumnType("bit");

                    b.Property<bool>("UnicaPorFamilia")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("CriadoPorId");

                    b.HasIndex("ModificadoPorId");

                    b.HasIndex("OrganizacaoId");

                    b.HasIndex("RelacionadoAoId");

                    b.ToTable("Itens");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.ItemFamiliar", b =>
                {
                    b.Property<int>("CasoId")
                        .HasColumnType("int");

                    b.Property<int>("ItemFamiliarId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValidoAte")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidoAte");

                    b.Property<DateTime>("ValidoDe")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidoDe");

                    b.HasKey("CasoId", "ItemFamiliarId");

                    b.HasIndex("ItemFamiliarId");

                    b.ToTable("ItensFamiliares", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb.UseHistoryTable("ItensFamiliaresHistorico");
                            ttb
                                .HasPeriodStart("ValidoDe")
                                .HasColumnName("ValidoDe");
                            ttb
                                .HasPeriodEnd("ValidoAte")
                                .HasColumnName("ValidoAte");
                        }
                    ));
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("CriadoPorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ModificadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModificadoPorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CriadoPorId");

                    b.HasIndex("ModificadoPorId");

                    b.ToTable("Organizacoes");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.SaudeIndividuo", b =>
                {
                    b.Property<string>("IndividuoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ItemSaudeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValidoAte")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidoAte");

                    b.Property<DateTime>("ValidoDe")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidoDe");

                    b.HasKey("IndividuoId", "ItemSaudeId");

                    b.HasIndex("ItemSaudeId");

                    b.ToTable("SaudeIndividuos", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb.UseHistoryTable("SaudeIndividuosHistorico");
                            ttb
                                .HasPeriodStart("ValidoDe")
                                .HasColumnName("ValidoDe");
                            ttb
                                .HasPeriodEnd("ValidoAte")
                                .HasColumnName("ValidoAte");
                        }
                    ));
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", b =>
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

                    b.Property<int?>("OrganizacaoId")
                        .HasColumnType("int");

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

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("OrganizacaoId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.ViolenciaSofrida", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IndividuoEmViolacaoId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("SituacaoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValidoAte")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidoAte");

                    b.Property<DateTime>("ValidoDe")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidoDe");

                    b.Property<int>("ViolenciaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SituacaoId");

                    b.HasIndex("ViolenciaId");

                    b.HasIndex("IndividuoEmViolacaoId", "ViolenciaId")
                        .IsUnique();

                    b.ToTable("ViolenciasSofridas", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb.UseHistoryTable("ViolenciasSofridasHistorico");
                            ttb
                                .HasPeriodStart("ValidoDe")
                                .HasColumnName("ValidoDe");
                            ttb
                                .HasPeriodEnd("ValidoAte")
                                .HasColumnName("ValidoAte");
                        }
                    ));
                });

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

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
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("OrganizacaoUsuario", b =>
                {
                    b.Property<string>("MembrosId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OrganizacoesId")
                        .HasColumnType("int");

                    b.HasKey("MembrosId", "OrganizacoesId");

                    b.HasIndex("OrganizacoesId");

                    b.ToTable("MembrosOrganizacao", (string)null);
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Caso", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "CriadoPor")
                        .WithMany()
                        .HasForeignKey("CriadoPorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "ModificadoPor")
                        .WithMany()
                        .HasForeignKey("ModificadoPorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", "Organizacao")
                        .WithMany()
                        .HasForeignKey("OrganizacaoId");

                    b.Navigation("CriadoPor");

                    b.Navigation("ModificadoPor");

                    b.Navigation("Organizacao");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.IndividuoEmViolacao", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Caso", "Caso")
                        .WithMany("Individuos")
                        .HasForeignKey("CasoId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Caso");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", "Categoria")
                        .WithMany("Itens")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "CriadoPor")
                        .WithMany()
                        .HasForeignKey("CriadoPorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "ModificadoPor")
                        .WithMany()
                        .HasForeignKey("ModificadoPorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", "Organizacao")
                        .WithMany("Itens")
                        .HasForeignKey("OrganizacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", "RelacionadoA")
                        .WithMany()
                        .HasForeignKey("RelacionadoAoId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Categoria");

                    b.Navigation("CriadoPor");

                    b.Navigation("ModificadoPor");

                    b.Navigation("Organizacao");

                    b.Navigation("RelacionadoA");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.ItemFamiliar", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Caso", null)
                        .WithMany()
                        .HasForeignKey("CasoId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired()
                        .HasConstraintName("FK_ItensFamiliares_Casos_CasoId");

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemFamiliarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ItensFamiliares_Itens_ItemFamiliarId");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "CriadoPor")
                        .WithMany()
                        .HasForeignKey("CriadoPorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", "ModificadoPor")
                        .WithMany()
                        .HasForeignKey("ModificadoPorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CriadoPor");

                    b.Navigation("ModificadoPor");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.SaudeIndividuo", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.IndividuoEmViolacao", null)
                        .WithMany()
                        .HasForeignKey("IndividuoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_SaudeIndividuos_Individuos_IndividuoId");

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemSaudeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_SaudeIndividuos_Itens_ItemSaudeId");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", null)
                        .WithMany("Administradores")
                        .HasForeignKey("OrganizacaoId");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.ViolenciaSofrida", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.IndividuoEmViolacao", "IndividuoEmViolacao")
                        .WithMany("ViolenciasSofridas")
                        .HasForeignKey("IndividuoEmViolacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", "Situacao")
                        .WithMany()
                        .HasForeignKey("SituacaoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", "Violencia")
                        .WithMany()
                        .HasForeignKey("ViolenciaId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("IndividuoEmViolacao");

                    b.Navigation("Situacao");

                    b.Navigation("Violencia");
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
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", null)
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

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrganizacaoUsuario", b =>
                {
                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Usuario", null)
                        .WithMany()
                        .HasForeignKey("MembrosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", null)
                        .WithMany()
                        .HasForeignKey("OrganizacoesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Caso", b =>
                {
                    b.Navigation("Individuos");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.IndividuoEmViolacao", b =>
                {
                    b.Navigation("ViolenciasSofridas");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Item", b =>
                {
                    b.Navigation("Itens");
                });

            modelBuilder.Entity("AssistenciaSocial.PontuaCasos.WebApp.Models.Organizacao", b =>
                {
                    b.Navigation("Administradores");

                    b.Navigation("Itens");
                });
#pragma warning restore 612, 618
        }
    }
}
