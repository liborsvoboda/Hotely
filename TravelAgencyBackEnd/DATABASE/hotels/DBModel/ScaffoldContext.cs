using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class ScaffoldContext : DbContext
    {
        public ScaffoldContext()
        {
        }

        public ScaffoldContext(DbContextOptions<ScaffoldContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessRoleList> AccessRoleLists { get; set; }
        public virtual DbSet<AddressList> AddressLists { get; set; }
        public virtual DbSet<AdminLoginHistoryList> AdminLoginHistoryLists { get; set; }
        public virtual DbSet<BranchList> BranchLists { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<CityList> CityLists { get; set; }
        public virtual DbSet<CountryList> CountryLists { get; set; }
        public virtual DbSet<CurrencyList> CurrencyLists { get; set; }
        public virtual DbSet<DocumentAdviceList> DocumentAdviceLists { get; set; }
        public virtual DbSet<DocumentTypeList> DocumentTypeLists { get; set; }
        public virtual DbSet<ExchangeRateList> ExchangeRateLists { get; set; }
        public virtual DbSet<GuestList> GuestLists { get; set; }
        public virtual DbSet<GuestLoginHistoryList> GuestLoginHistoryLists { get; set; }
        public virtual DbSet<HotelAccommodationActionList> HotelAccommodationActionLists { get; set; }
        public virtual DbSet<HotelActionTypeList> HotelActionTypeLists { get; set; }
        public virtual DbSet<HotelApprovalList> HotelApprovalLists { get; set; }
        public virtual DbSet<HotelImagesList> HotelImagesLists { get; set; }
        public virtual DbSet<HotelList> HotelLists { get; set; }
        public virtual DbSet<HotelPropertyAndServiceList> HotelPropertyAndServiceLists { get; set; }
        public virtual DbSet<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        public virtual DbSet<HotelReservationList> HotelReservationLists { get; set; }
        public virtual DbSet<HotelReservationReviewList> HotelReservationReviewLists { get; set; }
        public virtual DbSet<HotelReservationStatusList> HotelReservationStatusLists { get; set; }
        public virtual DbSet<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
        public virtual DbSet<HotelRoomList> HotelRoomLists { get; set; }
        public virtual DbSet<HotelRoomTypeList> HotelRoomTypeLists { get; set; }
        public virtual DbSet<IgnoredExceptionList> IgnoredExceptionLists { get; set; }
        public virtual DbSet<InterestAreaCityList> InterestAreaCityLists { get; set; }
        public virtual DbSet<InterestAreaList> InterestAreaLists { get; set; }
        public virtual DbSet<LanguageList> LanguageLists { get; set; }
        public virtual DbSet<MottoList> MottoLists { get; set; }
        public virtual DbSet<ParameterList> ParameterLists { get; set; }
        public virtual DbSet<PropertyGroupList> PropertyGroupLists { get; set; }
        public virtual DbSet<PropertyOrServiceTypeList> PropertyOrServiceTypeLists { get; set; }
        public virtual DbSet<PropertyOrServiceUnitList> PropertyOrServiceUnitLists { get; set; }
        public virtual DbSet<ReportList> ReportLists { get; set; }
        public virtual DbSet<ReportQueueList> ReportQueueLists { get; set; }
        public virtual DbSet<SystemFailList> SystemFailLists { get; set; }
        public virtual DbSet<TemplateList> TemplateLists { get; set; }
        public virtual DbSet<UserList> UserLists { get; set; }
        public virtual DbSet<UserRoleList> UserRoleLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Czech_CI_AS");

            modelBuilder.Entity<AccessRoleList>(entity =>
            {
                entity.Property(e => e.AccessRole).HasDefaultValueSql("('Admin,')");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AccessRoleLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccessRuleList_UserList");
            });

            modelBuilder.Entity<AddressList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AddressLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AddressList_UserList");
            });

            modelBuilder.Entity<AdminLoginHistoryList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<BranchList>(entity =>
            {
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

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Calendars)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Calendar_UserList");
            });

            modelBuilder.Entity<CityList>(entity =>
            {
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
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CurrencyLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyList_UserList");
            });

            modelBuilder.Entity<DocumentAdviceList>(entity =>
            {
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
                entity.Property(e => e.SystemName).HasDefaultValueSql("('MustProgramming')");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DocumentTypeLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentTypeList_UserList");
            });

            modelBuilder.Entity<ExchangeRateList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

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
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<GuestLoginHistoryList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<HotelAccommodationActionList>(entity =>
            {
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
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelActionTypeLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelActionList_UserList");
            });

            modelBuilder.Entity<HotelApprovalList>(entity =>
            {
                entity.ToView("HotelApprovalList");
            });

            modelBuilder.Entity<HotelImagesList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelImagesLists)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelImagesList_HotelList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelImagesLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelImagesList_UserList");
            });

            modelBuilder.Entity<HotelList>(entity =>
            {
                entity.Property(e => e.Advertised).HasDefaultValueSql("((1))");

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

            modelBuilder.Entity<HotelPropertyAndServiceList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelPropertyAndServiceLists)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_HotelPropertyAndServiceList_HotelList");

                entity.HasOne(d => d.PropertyOrService)
                    .WithMany(p => p.HotelPropertyAndServiceLists)
                    .HasForeignKey(d => d.PropertyOrServiceId)
                    .HasConstraintName("FK_HotelPropertyAndServiceList_PropertyOrServiceList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelPropertyAndServiceLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelPropertyAndServiceList_UserList");
            });

            modelBuilder.Entity<HotelReservationDetailList>(entity =>
            {
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
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

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
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelReservationStatusLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReservationStatusList_UserList");
            });

            modelBuilder.Entity<HotelReservedRoomList>(entity =>
            {
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
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<IgnoredExceptionList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.IgnoredExceptionLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IgnoredExceptionList_UserList");
            });

            modelBuilder.Entity<InterestAreaCityList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.InterestAreaCityLists)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_InterestAreaCityList_CityList");

                entity.HasOne(d => d.Iac)
                    .WithMany(p => p.InterestAreaCityLists)
                    .HasForeignKey(d => d.Iacid)
                    .HasConstraintName("FK_InterestAreaCityList_InterestAreaList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.InterestAreaCityLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterestAreaCityList_UserList");
            });

            modelBuilder.Entity<InterestAreaList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.InterestAreaLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterestAreaList_UserList");
            });

            modelBuilder.Entity<LanguageList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LanguageLists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_LanguageList_UserList");
            });

            modelBuilder.Entity<MottoList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MottoLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MottoList_UserList");
            });

            modelBuilder.Entity<ParameterList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ParameterLists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ParameterList_UserList");
            });

            modelBuilder.Entity<PropertyGroupList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PropertyGroupLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyGroupList_UserList");
            });

            modelBuilder.Entity<PropertyOrServiceTypeList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.PropertyGroup)
                    .WithMany(p => p.PropertyOrServiceTypeLists)
                    .HasForeignKey(d => d.PropertyGroupId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_PropertyOrServiceTypeList_PropertyGroupList");

                entity.HasOne(d => d.PropertyOrServiceUnitType)
                    .WithMany(p => p.PropertyOrServiceTypeLists)
                    .HasForeignKey(d => d.PropertyOrServiceUnitTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyOrServiceList_PropertyOrServiceUnitTypeList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PropertyOrServiceTypeLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyOrServiceList_UserList");
            });

            modelBuilder.Entity<PropertyOrServiceUnitList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PropertyOrServiceUnitLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyOrServiceTypeList_UserList");
            });

            modelBuilder.Entity<ReportList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReportLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportList_UserList");
            });

            modelBuilder.Entity<ReportQueueList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReportQueueLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportQueueList_UserList");
            });

            modelBuilder.Entity<SystemFailList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemFailLists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SystemFailList_UserList");
            });

            modelBuilder.Entity<TemplateList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TemplateLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateList_UserList");
            });

            modelBuilder.Entity<UserList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserLists)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserList_UserRoleList");
            });

            modelBuilder.Entity<UserRoleList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
