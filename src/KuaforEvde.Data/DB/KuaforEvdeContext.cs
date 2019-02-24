using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KuaforEvde.Data.DB
{
    public partial class KuaforEvdeContext : DbContext
    {
        public KuaforEvdeContext()
        {
        }

        //public KuaforEvdeContext(DbContextOptions<KuaforEvdeContext> options)
        //    : base(options)
        //{
        //}

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<ContactPerson> ContactPerson { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<HairDresser> HairDresser { get; set; }
        public virtual DbSet<HairDresserArea> HairDresserArea { get; set; }
        public virtual DbSet<HairDresserImage> HairDresserImage { get; set; }
        public virtual DbSet<HairDresserService> HairDresserService { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<Token> Token { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=1234567Ev;database=kuafor_evde");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account", "kuafor_evde");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Gender).HasColumnType("tinyint(4)");

                entity.Property(e => e.HasValidEmail).HasColumnType("tinyint(1)");

                entity.Property(e => e.HasValidPhone).HasColumnType("tinyint(1)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("tinyint(4)");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city", "kuafor_evde");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.DisplayOrder).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("tinyint(4)");
            });

            modelBuilder.Entity<ContactPerson>(entity =>
            {
                entity.ToTable("contact_person", "kuafor_evde");

                entity.HasIndex(e => e.HairDresserId)
                    .HasName("FK_HairDresser_ContactPerson_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.HairDresserId).HasColumnType("int(11)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(145)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(145)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("tinyint(4)");

                entity.HasOne(d => d.HairDresser)
                    .WithMany(p => p.ContactPerson)
                    .HasForeignKey(d => d.HairDresserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HairDresser_ContactPerson");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("district", "kuafor_evde");

                entity.HasIndex(e => e.CityId)
                    .HasName("FK_City_District_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CityId).HasColumnType("int(11)");

                entity.Property(e => e.DisplayOrder).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("tinyint(4)");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.District)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_City_District");
            });

            modelBuilder.Entity<HairDresser>(entity =>
            {
                entity.ToTable("hair_dresser", "kuafor_evde");

                entity.HasIndex(e => e.AccountId)
                    .HasName("FK_Account_HairDresser_idx");

                entity.HasIndex(e => e.CityId)
                    .HasName("FK_City_HairDresser_idx");

                entity.HasIndex(e => e.DisrtictId)
                    .HasName("FK_Disrtict_HairDresser_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AccountId).HasColumnType("int(11)");

                entity.Property(e => e.AddressText)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.CityId).HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.DisrtictId).HasColumnType("int(11)");

                entity.Property(e => e.Latitude)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("tinyint(4)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.HairDresser)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_HairDresser");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.HairDresser)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_City_HairDresser");

                entity.HasOne(d => d.Disrtict)
                    .WithMany(p => p.HairDresser)
                    .HasForeignKey(d => d.DisrtictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Disrtict_HairDresser");
            });

            modelBuilder.Entity<HairDresserArea>(entity =>
            {
                entity.ToTable("hair_dresser_area", "kuafor_evde");

                entity.HasIndex(e => e.DistrictId)
                    .HasName("FK_HairDresser_Disritct_idx");

                entity.HasIndex(e => e.HairDresserId)
                    .HasName("FK_HairDresser_Area_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.DistrictId).HasColumnType("int(11)");

                entity.Property(e => e.HairDresserId).HasColumnType("int(11)");

                entity.Property(e => e.Status).HasColumnType("tinyint(4)");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HairDresserArea)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HairDresser_Disritct");

                entity.HasOne(d => d.HairDresser)
                    .WithMany(p => p.HairDresserArea)
                    .HasForeignKey(d => d.HairDresserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HairDresser_Area");
            });

            modelBuilder.Entity<HairDresserImage>(entity =>
            {
                entity.ToTable("hair_dresser_image", "kuafor_evde");

                entity.HasIndex(e => e.HairDresserId)
                    .HasName("FK_HairDresser_Image_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AltText)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayOrder).HasColumnType("int(11)");

                entity.Property(e => e.HairDresserId).HasColumnType("int(11)");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.IsMain).HasColumnType("tinyint(4)");

                entity.Property(e => e.Status).HasColumnType("int(11)");

                entity.HasOne(d => d.HairDresser)
                    .WithMany(p => p.HairDresserImage)
                    .HasForeignKey(d => d.HairDresserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HairDresser_Image");
            });

            modelBuilder.Entity<HairDresserService>(entity =>
            {
                entity.ToTable("hair_dresser_service", "kuafor_evde");

                entity.HasIndex(e => e.HairDresserId)
                    .HasName("FK_HairDresser_Service_idx");

                entity.HasIndex(e => e.ServiceId)
                    .HasName("FK_Service_HairDresserService_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Gender).HasColumnType("tinyint(4)");

                entity.Property(e => e.HairDresserId).HasColumnType("int(11)");

                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");

                entity.Property(e => e.ServiceId).HasColumnType("int(11)");

                entity.Property(e => e.Status).HasColumnType("tinyint(4)");

                entity.HasOne(d => d.HairDresser)
                    .WithMany(p => p.HairDresserService)
                    .HasForeignKey(d => d.HairDresserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HairDresser_Service");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.HairDresserService)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Service_HairDresserService");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("service", "kuafor_evde");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ForFemale).HasColumnType("tinyint(1)");

                entity.Property(e => e.ForMale).HasColumnType("tinyint(1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("tinyint(4)");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("token", "kuafor_evde");

                entity.HasIndex(e => e.AccountId)
                    .HasName("FK_Account_Token_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AccountId).HasColumnType("int(11)");

                entity.Property(e => e.TokenKey)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Token)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Token");
            });
        }
    }
}
