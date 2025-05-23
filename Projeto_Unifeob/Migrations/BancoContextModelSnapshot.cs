﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Projeto_Unifeob.Data;

#nullable disable

namespace Projeto_Unifeob.Migrations
{
    [DbContext(typeof(BancoContext))]
    partial class BancoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Projeto_Unifeob.Models.Beneficio_Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("nome_beneficio")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("percentual")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.ToTable("Beneficio");
                });

            modelBuilder.Entity("Projeto_Unifeob.Models.ContribuinteBeneficio_Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BeneficioId")
                        .HasColumnType("int");

                    b.Property<int>("ContribuinteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BeneficioId");

                    b.HasIndex("ContribuinteId");

                    b.ToTable("ContribuinteBeneficio");
                });

            modelBuilder.Entity("Projeto_Unifeob.Models.Contribuinte_Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<DateTime>("DataAbertura")
                        .HasColumnType("date");

                    b.Property<string>("RazaoSocial")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RegimeTributacao")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Contribuinte");
                });

            modelBuilder.Entity("Projeto_Unifeob.Models.ContribuinteBeneficio_Model", b =>
                {
                    b.HasOne("Projeto_Unifeob.Models.Beneficio_Model", "Beneficio")
                        .WithMany("ContribuinteBeneficios")
                        .HasForeignKey("BeneficioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projeto_Unifeob.Models.Contribuinte_Model", "Contribuinte")
                        .WithMany("ContribuinteBeneficios")
                        .HasForeignKey("ContribuinteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Beneficio");

                    b.Navigation("Contribuinte");
                });

            modelBuilder.Entity("Projeto_Unifeob.Models.Beneficio_Model", b =>
                {
                    b.Navigation("ContribuinteBeneficios");
                });

            modelBuilder.Entity("Projeto_Unifeob.Models.Contribuinte_Model", b =>
                {
                    b.Navigation("ContribuinteBeneficios");
                });
#pragma warning restore 612, 618
        }
    }
}
