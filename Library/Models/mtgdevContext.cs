using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace mtg_lib.Library.Models
{
    public partial class mtgdevContext : DbContext
    {
        public mtgdevContext()
        {
        }

        public mtgdevContext(DbContextOptions<mtgdevContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artist> Artists { get; set; } = null!;
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Card> Cards { get; set; } = null!;
        public virtual DbSet<CardColor> CardColors { get; set; } = null!;
        public virtual DbSet<CardType> CardTypes { get; set; } = null!;
        public virtual DbSet<Color> Colors { get; set; } = null!;
        public virtual DbSet<Format> Formats { get; set; } = null!;
        public virtual DbSet<Rarity> Rarities { get; set; } = null!;
        public virtual DbSet<Set> Sets { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;
        public virtual DbSet<UserCoin> UserCoins { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=mtg-dev;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("artists");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("cards");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArtistId).HasColumnName("artist_id");

                entity.Property(e => e.ConvertedManaCost)
                    .HasMaxLength(255)
                    .HasColumnName("converted_mana_cost");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.Flavor).HasColumnName("flavor");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.Layout)
                    .HasMaxLength(255)
                    .HasColumnName("layout");

                entity.Property(e => e.ManaCost)
                    .HasMaxLength(255)
                    .HasColumnName("mana_cost");

                entity.Property(e => e.MtgId)
                    .HasMaxLength(255)
                    .HasColumnName("mtg_id");

                entity.Property(e => e.MultiverseId).HasColumnName("multiverse_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Number)
                    .HasMaxLength(255)
                    .HasColumnName("number");

                entity.Property(e => e.OriginalImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("original_image_url");

                entity.Property(e => e.OriginalText).HasColumnName("original_text");

                entity.Property(e => e.OriginalType)
                    .HasMaxLength(255)
                    .HasColumnName("original_type");

                entity.Property(e => e.Power)
                    .HasMaxLength(255)
                    .HasColumnName("power");

                entity.Property(e => e.RarityCode)
                    .HasMaxLength(255)
                    .HasColumnName("rarity_code");

                entity.Property(e => e.SetCode)
                    .HasMaxLength(255)
                    .HasColumnName("set_code");

                entity.Property(e => e.Text).HasColumnName("text");

                entity.Property(e => e.Toughness)
                    .HasMaxLength(255)
                    .HasColumnName("toughness");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("type");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("updated_at");

                entity.Property(e => e.Variations)
                    .HasColumnType("json")
                    .HasColumnName("variations");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.ArtistId)
                    .HasConstraintName("cards_artist_id_foreign");

                entity.HasOne(d => d.RarityCodeNavigation)
                    .WithMany(p => p.Cards)
                    .HasPrincipalKey(p => p.Code)
                    .HasForeignKey(d => d.RarityCode)
                    .HasConstraintName("cards_rarity_code_foreign");

                entity.HasOne(d => d.SetCodeNavigation)
                    .WithMany(p => p.Cards)
                    .HasPrincipalKey(p => p.Code)
                    .HasForeignKey(d => d.SetCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cards_set_code_foreign");
            });

            modelBuilder.Entity<CardColor>(entity =>
            {
                entity.HasKey(e => new { e.CardId, e.ColorId })
                    .HasName("card_colors_pkey");

                entity.ToTable("card_colors");

                entity.Property(e => e.CardId).HasColumnName("card_id");

                entity.Property(e => e.ColorId).HasColumnName("color_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.CardColors)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("card_colors_card_id_foreign");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.CardColors)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("card_colors_color_id_foreign");
            });

            modelBuilder.Entity<CardType>(entity =>
            {
                entity.HasKey(e => new { e.CardId, e.TypeId })
                    .HasName("card_types_pkey");

                entity.ToTable("card_types");

                entity.Property(e => e.CardId).HasColumnName("card_id");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.CardTypes)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("card_types_card_id_foreign");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.CardTypes)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("card_types_type_id_foreign");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("colors");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<Format>(entity =>
            {
                entity.ToTable("formats");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<Rarity>(entity =>
            {
                entity.ToTable("rarities");

                entity.HasIndex(e => e.Code, "rarities_code_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<Set>(entity =>
            {
                entity.ToTable("sets");

                entity.HasIndex(e => e.Code, "sets_code_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Type1)
                    .HasMaxLength(255)
                    .HasColumnName("type");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<UserCoin>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("usercoins_pk");

                entity.HasIndex(e => e.UserId, "usercoins_userid_uindex")
                    .IsUnique();

                entity.Property(e => e.ClaimedTimeStamp)
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserCoin)
                    .HasForeignKey<UserCoin>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("usercoins_aspnetusers_id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
