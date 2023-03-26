using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class hotelsContext : DbContext
    {
        public hotelsContext()
        {
        }

        public hotelsContext(DbContextOptions<hotelsContext> options)
                    : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableServiceProviderCaching(false);
                optionsBuilder.UseSqlServer("Server=95.183.52.33;Database=hotels;User ID=sa;Password=Hotel2023+;",
                //optionsBuilder.UseSqlServer(Program.ServerSettings.DbConnectionString,
                x => x.MigrationsHistoryTable("MigrationsHistory", "dbo"))
                //.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
                //.LogTo(message => Debug.WriteLine(message))
                //.LogTo(Console.WriteLine)
                ;
            }
        }

        public virtual DbSet<AccessRuleList> AccessRuleLists { get; set; }
        public virtual DbSet<AddressList> AddressLists { get; set; }
        public virtual DbSet<BranchList> BranchLists { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<CityList> CityLists { get; set; }
        public virtual DbSet<CountryList> CountryLists { get; set; }
        public virtual DbSet<CurrencyList> CurrencyLists { get; set; }
        public virtual DbSet<DocumentAdviceList> DocumentAdviceLists { get; set; }
        public virtual DbSet<DocumentTypeList> DocumentTypeLists { get; set; }
        public virtual DbSet<ExchangeRateList> ExchangeRateLists { get; set; }
        public virtual DbSet<GuestList> GuestLists { get; set; }
        public virtual DbSet<HotelAccommodationActionList> HotelAccommodationActionLists { get; set; }
        public virtual DbSet<HotelActionTypeList> HotelActionTypeLists { get; set; }
        public virtual DbSet<HotelList> HotelLists { get; set; }
        public virtual DbSet<HotelPropertyFeeList> HotelPropertyFeeLists { get; set; }
        public virtual DbSet<HotelPropertyList> HotelPropertyLists { get; set; }
        public virtual DbSet<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        public virtual DbSet<HotelReservationList> HotelReservationLists { get; set; }
        public virtual DbSet<HotelReservationReviewList> HotelReservationReviewLists { get; set; }
        public virtual DbSet<HotelReservationStatusList> HotelReservationStatusLists { get; set; }
        public virtual DbSet<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
        public virtual DbSet<HotelRoomList> HotelRoomLists { get; set; }
        public virtual DbSet<HotelRoomTypeList> HotelRoomTypeLists { get; set; }
        public virtual DbSet<HotelServiceFeeList> HotelServiceFeeLists { get; set; }
        public virtual DbSet<HotelServiceList> HotelServiceLists { get; set; }
        public virtual DbSet<LanguageList> LanguageLists { get; set; }
        public virtual DbSet<LoginHistoryList> LoginHistoryLists { get; set; }
        public virtual DbSet<MottoList> MottoLists { get; set; }
        public virtual DbSet<ParameterList> ParameterLists { get; set; }
        public virtual DbSet<ReportList> ReportLists { get; set; }
        public virtual DbSet<ReportQueueList> ReportQueueLists { get; set; }
        public virtual DbSet<TemplateList> TemplateLists { get; set; }
        public virtual DbSet<UserList> UserLists { get; set; }
        public virtual DbSet<UserRoleList> UserRoleLists { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Czech_CI_AS");

            modelBuilder.Entity<AccessRuleList>(entity =>
            {
                entity.ToTable("AccessRuleList");

                entity.HasIndex(e => e.TableName, "IX_AccessRuleList")
                    .IsUnique();

                entity.Property(e => e.AccessRole)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Admin,')");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AccessRuleLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccessRuleList_UserList");
            });

            modelBuilder.Entity<AddressList>(entity =>
            {
                entity.ToTable("AddressList");

                entity.HasIndex(e => new { e.CompanyName, e.UserId }, "IX_AddressList")
                    .IsUnique();

                entity.HasIndex(e => new { e.Email, e.UserId }, "IX_AddressList_1");

                entity.Property(e => e.AddressType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BankAccount)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ContactName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Dic)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Ico)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PostCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AddressLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AddressList_UserList");
            });

            modelBuilder.Entity<BranchList>(entity =>
            {
                entity.ToTable("BranchList");

                entity.HasIndex(e => new { e.CompanyName, e.UserId }, "IX_BranchList")
                    .IsUnique();

                entity.Property(e => e.BankAccount)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ContactName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Dic)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Ico)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PostCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BranchLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BranchList_UserList");
            });

            modelBuilder.Entity<Calendar>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Date });

                entity.ToTable("Calendar");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Notes)
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Calendars)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Calendar_UserList");
            });

            modelBuilder.Entity<CityList>(entity =>
            {
                entity.ToTable("CityList");

                entity.HasIndex(e => e.City, "IX_CityList")
                    .IsUnique();

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CityLists)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CityList_CountryList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CityLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_City_UserList");
            });

            modelBuilder.Entity<CountryList>(entity =>
            {
                entity.ToTable("CountryList");

                entity.HasIndex(e => e.SystemName, "IX_Country")
                    .IsUnique();

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CountryLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Country_UserList");
            });

            modelBuilder.Entity<CurrencyList>(entity =>
            {
                entity.ToTable("CurrencyList");

                entity.HasIndex(e => e.Name, "IX_CurrencyList")
                    .IsUnique();

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.ExchangeRate).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CurrencyLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyList_UserList");
            });

            modelBuilder.Entity<DocumentAdviceList>(entity =>
            {
                entity.ToTable("DocumentAdviceList");

                entity.HasIndex(e => new { e.BranchId, e.DocumentTypeId, e.StartDate }, "IX_DocumentAdviceList")
                    .IsUnique();

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Prefix)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.DocumentAdviceLists)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentAdviceList_BranchList");

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.DocumentAdviceLists)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentAdviceList_DocumentTypeList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DocumentAdviceLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentAdvice_UserList");
            });

            modelBuilder.Entity<DocumentTypeList>(entity =>
            {
                entity.ToTable("DocumentTypeList");

                entity.HasIndex(e => e.SystemName, "IX_DocumentTypeList")
                    .IsUnique();

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DocumentTypeLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentTypeList_UserList");
            });

            modelBuilder.Entity<ExchangeRateList>(entity =>
            {
                entity.ToTable("ExchangeRateList");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidFrom).HasColumnType("date");

                entity.Property(e => e.ValidTo).HasColumnType("date");

                entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.ExchangeRateLists)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeRateList_CurrencyList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ExchangeRateLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeRateList_UserList");
            });

            modelBuilder.Entity<GuestList>(entity =>
            {
                entity.ToTable("GuestList");

                entity.HasIndex(e => e.Email, "IX_GuestList");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HotelAccommodationActionList>(entity =>
            {
                entity.ToTable("HotelAccommodationActionList");

                entity.HasIndex(e => new { e.HotelId, e.HotelActionTypeId, e.StartDate, e.EndDate }, "IX_HotelAccomodationActionList")
                    .IsUnique();

                entity.Property(e => e.DescriptionCz)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.DescriptionEn)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.HotelActionType)
                    .WithMany(p => p.HotelAccommodationActionLists)
                    .HasForeignKey(d => d.HotelActionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelAccomodationActionList_HotelActionList");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelAccommodationActionLists)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelAccomodationActionList_HotelList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelAccommodationActionLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelAccomodationActionList_UserList");
            });

            modelBuilder.Entity<HotelActionTypeList>(entity =>
            {
                entity.ToTable("HotelActionTypeList");

                entity.HasIndex(e => e.SystemName, "IX_HotelActionList")
                    .IsUnique();

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelActionTypeLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelActionList_UserList");
            });

            modelBuilder.Entity<HotelList>(entity =>
            {
                entity.ToTable("HotelList");

                entity.HasIndex(e => new { e.Name, e.CityId }, "IX_Hotels")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4096)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.HotelLists)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hotels_CityList");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.HotelLists)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hotels_CountryList");

                entity.HasOne(d => d.DefaultCurrency)
                    .WithMany(p => p.HotelLists)
                    .HasForeignKey(d => d.DefaultCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelList_CurrencyList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hotels_UserList");
            });

            modelBuilder.Entity<HotelPropertyFeeList>(entity =>
            {
                entity.ToTable("HotelPropertyFeeList");

                entity.HasIndex(e => new { e.HotelId, e.PropertyId }, "IX_HotelPropertyFeeList")
                    .IsUnique();

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelPropertyFeeLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelPropertyFeeList_UserList");
            });

            modelBuilder.Entity<HotelPropertyList>(entity =>
            {
                entity.ToTable("HotelPropertyList");

                entity.HasIndex(e => e.SystemName, "IX_PropertyList")
                    .IsUnique();

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelPropertyLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyList_UserList");
            });

            modelBuilder.Entity<HotelReservationDetailList>(entity =>
            {
                entity.ToTable("HotelReservationDetailList");

                entity.HasIndex(e => e.HotelId, "IX_ReservationsDetailList");

                entity.HasIndex(e => new { e.HotelId, e.GuestId }, "IX_ReservationsDetailList_1");

                entity.HasIndex(e => e.GuestId, "IX_ReservationsDetailList_2");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.HotelReservationDetailLists)
                    .HasForeignKey(d => d.GuestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReservationsDetailList_GuestList");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelReservationDetailLists)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReservationsDetailList_HotelList");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.HotelReservationDetailLists)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReservationsDetailList_ReservationList");
            });

            modelBuilder.Entity<HotelReservationList>(entity =>
            {
                entity.ToTable("HotelReservationList");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ReservationNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SearchedText).HasColumnType("text");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Zipcode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.HotelReservationLists)
                    .HasForeignKey(d => d.GuestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservations_GuestList");

                entity.HasOne(d => d.HotelAccommodationAction)
                    .WithMany(p => p.HotelReservationLists)
                    .HasForeignKey(d => d.HotelAccommodationActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservationList_HotelAccomodationActionList");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelReservationLists)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservations_HotelList");
            });

            modelBuilder.Entity<HotelReservationReviewList>(entity =>
            {
                entity.ToTable("HotelReservationReviewList");

                entity.HasIndex(e => e.HotelId, "IX_ReviewList");

                entity.HasIndex(e => e.ReservationId, "IX_ReviewList_1")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.HotelReservationReviewLists)
                    .HasForeignKey(d => d.GuestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReviewList_GuestList");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelReservationReviewLists)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReviewList_HotelList");

                entity.HasOne(d => d.Reservation)
                    .WithOne(p => p.HotelReservationReviewList)
                    .HasForeignKey<HotelReservationReviewList>(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReviewList_HotelReservationList");
            });

            modelBuilder.Entity<HotelReservationStatusList>(entity =>
            {
                entity.ToTable("HotelReservationStatusList");

                entity.HasIndex(e => e.SystemName, "IX_ReservationStatusList")
                    .IsUnique();

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelReservationStatusLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReservationStatusList_UserList");
            });

            modelBuilder.Entity<HotelReservedRoomList>(entity =>
            {
                entity.ToTable("HotelReservedRoomList");

                entity.HasIndex(e => new { e.HotelRoomId, e.ReservationId }, "IX_HotelReservedRoomList")
                    .IsUnique();

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.HotelRoom)
                    .WithMany(p => p.HotelReservedRoomLists)
                    .HasForeignKey(d => d.HotelRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservedRoomList_HotelRoomList");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.HotelReservedRoomLists)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservedRoomList_ReservationList");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.HotelReservedRoomLists)
                    .HasForeignKey(d => d.RoomTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservedRoomList_HotelRoomTypeList");
            });

            modelBuilder.Entity<HotelRoomList>(entity =>
            {
                entity.ToTable("HotelRoomList");

                entity.HasIndex(e => e.HotelId, "IX_HotelRoomList");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelRoomLists)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelRoomList_HotelList");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.HotelRoomLists)
                    .HasForeignKey(d => d.RoomTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelRoomList_HotelRoomTypeList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelRoomLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelRoomList_UserList");
            });

            modelBuilder.Entity<HotelRoomTypeList>(entity =>
            {
                entity.ToTable("HotelRoomTypeList");

                entity.HasIndex(e => e.SystemName, "IX_RoomTypeList")
                    .IsUnique();

                entity.Property(e => e.DescriptionCz)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.DescriptionEn)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<HotelServiceFeeList>(entity =>
            {
                entity.ToTable("HotelServiceFeeList");

                entity.HasIndex(e => new { e.HotelId, e.PropertyId }, "IX_HotelServiceFeeList")
                    .IsUnique();

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelServiceFeeLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelServiceFeeList_UserList");
            });

            modelBuilder.Entity<HotelServiceList>(entity =>
            {
                entity.ToTable("HotelServiceList");

                entity.HasIndex(e => e.SystemName, "IX_ServiceList")
                    .IsUnique();

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelServiceLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceList_UserList");
            });

            modelBuilder.Entity<LanguageList>(entity =>
            {
                entity.ToTable("LanguageList");

                entity.HasIndex(e => e.SystemName, "IX_LanguageList");

                entity.Property(e => e.DescriptionCz)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DescriptionEn)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LanguageLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageList_UserList");
            });

            modelBuilder.Entity<LoginHistoryList>(entity =>
            {
                entity.ToTable("LoginHistoryList");

                entity.Property(e => e.IpAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MottoList>(entity =>
            {
                entity.ToTable("MottoList");

                entity.HasIndex(e => e.SystemName, "IX_MottoList")
                    .IsUnique();

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ParameterList>(entity =>
            {
                entity.ToTable("ParameterList");

                entity.HasIndex(e => new { e.SystemName, e.UserId }, "IX_ParameterList")
                    .IsUnique();

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ParameterLists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ParameterList_UserList");
            });

            modelBuilder.Entity<ReportList>(entity =>
            {
                entity.ToTable("ReportList");

                entity.HasIndex(e => new { e.PageName, e.SystemName }, "IX_ReportList")
                    .IsUnique();

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.File).IsRequired();

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReportPath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReportLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportList_UserList");
            });

            modelBuilder.Entity<ReportQueueList>(entity =>
            {
                entity.ToTable("ReportQueueList");

                entity.HasIndex(e => e.SystemName, "IX_ReportQueue")
                    .IsUnique();

                entity.HasIndex(e => e.TableName, "IX_ReportQueueList");

                entity.HasIndex(e => new { e.TableName, e.Sequence }, "IX_ReportQueueList_1")
                    .IsUnique();

                entity.Property(e => e.Search)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sql).IsRequired();

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReportQueueLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportQueueList_UserList");
            });

            modelBuilder.Entity<TemplateList>(entity =>
            {
                entity.ToTable("TemplateList");

                entity.HasIndex(e => e.Name, "IX_TemplateList")
                    .IsUnique();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TemplateLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateList_UserList");
            });

            modelBuilder.Entity<UserList>(entity =>
            {
                entity.ToTable("UserList");

                entity.HasIndex(e => e.UserName, "IX_UserList")
                    .IsUnique();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ApiToken)
                    .HasMaxLength(4096)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoPath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SurName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserLists)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserList_UserRoleList");
            });

            modelBuilder.Entity<UserRoleList>(entity =>
            {
                entity.ToTable("UserRoleList");

                entity.HasIndex(e => e.SystemName, "IX_UserRoleList")
                    .IsUnique();

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
