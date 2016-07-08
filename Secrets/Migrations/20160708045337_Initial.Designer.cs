using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Secrets;

namespace Secrets.Migrations
{
    [DbContext(typeof(SecretContext))]
    [Migration("20160708045337_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Secrets.Entities.Accessor", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<DateTimeOffset>("DateModified");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Key");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Accessors");
                });

            modelBuilder.Entity("Secrets.Entities.Secret", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApiKey")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<DateTimeOffset>("DateModified");

                    b.Property<string>("Login")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("MaxUse");

                    b.Property<int?>("MaxUseCounter");

                    b.Property<string>("Password")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<string>("UrlPattern")
                        .HasAnnotation("MaxLength", 2048);

                    b.HasKey("Key");

                    b.ToTable("Secrets");
                });

            modelBuilder.Entity("Secrets.Entities.SecretAccessor", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AccessorKey")
                        .IsRequired();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<DateTimeOffset>("DateModified");

                    b.Property<Guid>("SecretKey");

                    b.HasKey("Key");

                    b.HasIndex("AccessorKey");

                    b.HasIndex("SecretKey");

                    b.ToTable("SecretAccessor");
                });

            modelBuilder.Entity("Secrets.Entities.SecretAccessor", b =>
                {
                    b.HasOne("Secrets.Entities.Accessor", "Accessor")
                        .WithMany("Secrets")
                        .HasForeignKey("AccessorKey")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Secrets.Entities.Secret", "Secret")
                        .WithMany("Accessors")
                        .HasForeignKey("SecretKey")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
