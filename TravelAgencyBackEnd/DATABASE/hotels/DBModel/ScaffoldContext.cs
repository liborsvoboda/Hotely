using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UbytkacBackend.DBModel
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

        public virtual DbSet<BasicAttachmentList> BasicAttachmentLists { get; set; }
        public virtual DbSet<BasicCalendarList> BasicCalendarLists { get; set; }
        public virtual DbSet<BasicCurrencyList> BasicCurrencyLists { get; set; }
        public virtual DbSet<BasicImageGalleryList> BasicImageGalleryLists { get; set; }
        public virtual DbSet<BasicItemList> BasicItemLists { get; set; }
        public virtual DbSet<BasicUnitList> BasicUnitLists { get; set; }
        public virtual DbSet<BasicVatList> BasicVatLists { get; set; }
        public virtual DbSet<BasicViewAttachmentList> BasicViewAttachmentLists { get; set; }
        public virtual DbSet<BusinessAddressList> BusinessAddressLists { get; set; }
        public virtual DbSet<BusinessBranchList> BusinessBranchLists { get; set; }
        public virtual DbSet<BusinessCreditNoteList> BusinessCreditNoteLists { get; set; }
        public virtual DbSet<BusinessCreditNoteSupportList> BusinessCreditNoteSupportLists { get; set; }
        public virtual DbSet<BusinessExchangeRateList> BusinessExchangeRateLists { get; set; }
        public virtual DbSet<BusinessIncomingInvoiceList> BusinessIncomingInvoiceLists { get; set; }
        public virtual DbSet<BusinessIncomingInvoiceSupportList> BusinessIncomingInvoiceSupportLists { get; set; }
        public virtual DbSet<BusinessIncomingOrderList> BusinessIncomingOrderLists { get; set; }
        public virtual DbSet<BusinessIncomingOrderSupportList> BusinessIncomingOrderSupportLists { get; set; }
        public virtual DbSet<BusinessMaturityList> BusinessMaturityLists { get; set; }
        public virtual DbSet<BusinessNotesList> BusinessNotesLists { get; set; }
        public virtual DbSet<BusinessOfferList> BusinessOfferLists { get; set; }
        public virtual DbSet<BusinessOfferSupportList> BusinessOfferSupportLists { get; set; }
        public virtual DbSet<BusinessOutgoingInvoiceList> BusinessOutgoingInvoiceLists { get; set; }
        public virtual DbSet<BusinessOutgoingInvoiceSupportList> BusinessOutgoingInvoiceSupportLists { get; set; }
        public virtual DbSet<BusinessOutgoingOrderList> BusinessOutgoingOrderLists { get; set; }
        public virtual DbSet<BusinessOutgoingOrderSupportList> BusinessOutgoingOrderSupportLists { get; set; }
        public virtual DbSet<BusinessPaymentMethodList> BusinessPaymentMethodLists { get; set; }
        public virtual DbSet<BusinessPaymentStatusList> BusinessPaymentStatusLists { get; set; }
        public virtual DbSet<BusinessReceiptList> BusinessReceiptLists { get; set; }
        public virtual DbSet<BusinessReceiptSupportList> BusinessReceiptSupportLists { get; set; }
        public virtual DbSet<BusinessWarehouseList> BusinessWarehouseLists { get; set; }
        public virtual DbSet<CityList> CityLists { get; set; }
        public virtual DbSet<CodeLibraryList> CodeLibraryLists { get; set; }
        public virtual DbSet<CountryAreaCityList> CountryAreaCityLists { get; set; }
        public virtual DbSet<CountryAreaList> CountryAreaLists { get; set; }
        public virtual DbSet<CountryList> CountryLists { get; set; }
        public virtual DbSet<CreditPackageList> CreditPackageLists { get; set; }
        public virtual DbSet<DocSrvDocTemplateList> DocSrvDocTemplateLists { get; set; }
        public virtual DbSet<DocumentationCodeLibraryList> DocumentationCodeLibraryLists { get; set; }
        public virtual DbSet<DocumentationGroupList> DocumentationGroupLists { get; set; }
        public virtual DbSet<DocumentationList> DocumentationLists { get; set; }
        public virtual DbSet<GetTopFiveFavoriteList> GetTopFiveFavoriteLists { get; set; }
        public virtual DbSet<GuestAdvertiserNoteList> GuestAdvertiserNoteLists { get; set; }
        public virtual DbSet<GuestFavoriteList> GuestFavoriteLists { get; set; }
        public virtual DbSet<GuestList> GuestLists { get; set; }
        public virtual DbSet<GuestLoginHistoryList> GuestLoginHistoryLists { get; set; }
        public virtual DbSet<GuestSettingList> GuestSettingLists { get; set; }
        public virtual DbSet<HolidayTipsList> HolidayTipsLists { get; set; }
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
        public virtual DbSet<InterestAreaCityList> InterestAreaCityLists { get; set; }
        public virtual DbSet<InterestAreaList> InterestAreaLists { get; set; }
        public virtual DbSet<OftenQuestionList> OftenQuestionLists { get; set; }
        public virtual DbSet<PrivacyPolicyList> PrivacyPolicyLists { get; set; }
        public virtual DbSet<PropertyGroupList> PropertyGroupLists { get; set; }
        public virtual DbSet<PropertyOrServiceTypeList> PropertyOrServiceTypeLists { get; set; }
        public virtual DbSet<PropertyOrServiceUnitList> PropertyOrServiceUnitLists { get; set; }
        public virtual DbSet<RegistrationInfoList> RegistrationInfoLists { get; set; }
        public virtual DbSet<ServerBrowsablePathList> ServerBrowsablePathLists { get; set; }
        public virtual DbSet<ServerCorsDefAllowedOriginList> ServerCorsDefAllowedOriginLists { get; set; }
        public virtual DbSet<ServerHealthCheckTaskList> ServerHealthCheckTaskLists { get; set; }
        public virtual DbSet<ServerLiveDataMonitorList> ServerLiveDataMonitorLists { get; set; }
        public virtual DbSet<ServerModuleAndServiceList> ServerModuleAndServiceLists { get; set; }
        public virtual DbSet<ServerSettingList> ServerSettingLists { get; set; }
        public virtual DbSet<ServerToolPanelDefinitionList> ServerToolPanelDefinitionLists { get; set; }
        public virtual DbSet<ServerToolTypeList> ServerToolTypeLists { get; set; }
        public virtual DbSet<SolutionEmailTemplateList> SolutionEmailTemplateLists { get; set; }
        public virtual DbSet<SolutionEmailerHistoryList> SolutionEmailerHistoryLists { get; set; }
        public virtual DbSet<SolutionFailList> SolutionFailLists { get; set; }
        public virtual DbSet<SolutionLanguageList> SolutionLanguageLists { get; set; }
        public virtual DbSet<SolutionMessageModuleList> SolutionMessageModuleLists { get; set; }
        public virtual DbSet<SolutionMessageTypeList> SolutionMessageTypeLists { get; set; }
        public virtual DbSet<SolutionMixedEnumList> SolutionMixedEnumLists { get; set; }
        public virtual DbSet<SolutionMottoList> SolutionMottoLists { get; set; }
        public virtual DbSet<SolutionOperationList> SolutionOperationLists { get; set; }
        public virtual DbSet<SolutionSchedulerList> SolutionSchedulerLists { get; set; }
        public virtual DbSet<SolutionSchedulerProcessList> SolutionSchedulerProcessLists { get; set; }
        public virtual DbSet<SolutionUserList> SolutionUserLists { get; set; }
        public virtual DbSet<SolutionUserRoleList> SolutionUserRoleLists { get; set; }
        public virtual DbSet<SystemCustomPageList> SystemCustomPageLists { get; set; }
        public virtual DbSet<SystemDocumentAdviceList> SystemDocumentAdviceLists { get; set; }
        public virtual DbSet<SystemDocumentTypeList> SystemDocumentTypeLists { get; set; }
        public virtual DbSet<SystemGroupMenuList> SystemGroupMenuLists { get; set; }
        public virtual DbSet<SystemIgnoredExceptionList> SystemIgnoredExceptionLists { get; set; }
        public virtual DbSet<SystemLoginHistoryList> SystemLoginHistoryLists { get; set; }
        public virtual DbSet<SystemMenuList> SystemMenuLists { get; set; }
        public virtual DbSet<SystemModuleList> SystemModuleLists { get; set; }
        public virtual DbSet<SystemParameterList> SystemParameterLists { get; set; }
        public virtual DbSet<SystemReportList> SystemReportLists { get; set; }
        public virtual DbSet<SystemReportQueueList> SystemReportQueueLists { get; set; }
        public virtual DbSet<SystemSvgIconList> SystemSvgIconLists { get; set; }
        public virtual DbSet<SystemTranslationList> SystemTranslationLists { get; set; }
        public virtual DbSet<TemplateList> TemplateLists { get; set; }
        public virtual DbSet<TermsList> TermsLists { get; set; }
        public virtual DbSet<UbytkacInfoList> UbytkacInfoLists { get; set; }
        public virtual DbSet<WebMottoList> WebMottoLists { get; set; }
        public virtual DbSet<WebSettingList> WebSettingLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Czech_CI_AS");

            modelBuilder.Entity<BasicAttachmentList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BasicAttachmentLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AttachmentList_UserList");
            });

            modelBuilder.Entity<BasicCalendarList>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Date })
                    .HasName("PK_Calendar");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BasicCalendarLists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Calendar_UserList");
            });

            modelBuilder.Entity<BasicCurrencyList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.ExchangeRate).HasDefaultValueSql("((1))");

                entity.Property(e => e.Fixed).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BasicCurrencyLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyList_UserList");
            });

            modelBuilder.Entity<BasicImageGalleryList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BasicImageGalleryLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImageGalleryList_UserList");
            });

            modelBuilder.Entity<BasicItemList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.BasicItemLists)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemList_CurrencyList");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.BasicItemLists)
                    .HasPrincipalKey(p => p.Name)
                    .HasForeignKey(d => d.Unit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemList_UnitList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BasicItemLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemList_UserList");

                entity.HasOne(d => d.Vat)
                    .WithMany(p => p.BasicItemLists)
                    .HasForeignKey(d => d.VatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemList_VatList");
            });

            modelBuilder.Entity<BasicUnitList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BasicUnitLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitList_UserList");
            });

            modelBuilder.Entity<BasicVatList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BasicVatLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VatList_UserList");
            });

            modelBuilder.Entity<BasicViewAttachmentList>(entity =>
            {
                entity.ToView("BasicViewAttachmentList");
            });

            modelBuilder.Entity<BusinessAddressList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessAddressLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AddressList_UserList");
            });

            modelBuilder.Entity<BusinessBranchList>(entity =>
            {
                entity.HasComment("");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessBranchLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BranchList_UserList");
            });

            modelBuilder.Entity<BusinessCreditNoteList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.InvoiceNumberNavigation)
                    .WithMany(p => p.BusinessCreditNoteLists)
                    .HasPrincipalKey(p => p.DocumentNumber)
                    .HasForeignKey(d => d.InvoiceNumber)
                    .HasConstraintName("FK_CreditNoteList_OutgoingInvoiceList");

                entity.HasOne(d => d.TotalCurrency)
                    .WithMany(p => p.BusinessCreditNoteLists)
                    .HasForeignKey(d => d.TotalCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreditNoteList_CurrencyList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessCreditNoteLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreditNoteList_UserList");
            });

            modelBuilder.Entity<BusinessCreditNoteSupportList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DocumentNumberNavigation)
                    .WithMany(p => p.BusinessCreditNoteSupportLists)
                    .HasPrincipalKey(p => p.DocumentNumber)
                    .HasForeignKey(d => d.DocumentNumber)
                    .HasConstraintName("FK_CreditNoteItemList_CreditNoteList");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.BusinessCreditNoteSupportLists)
                    .HasPrincipalKey(p => p.Name)
                    .HasForeignKey(d => d.Unit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreditNoteItemList_UnitList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessCreditNoteSupportLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreditNoteItemList_UserList");
            });

            modelBuilder.Entity<BusinessExchangeRateList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.BusinessExchangeRateLists)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeRateList_CurrencyList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessExchangeRateLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseList_UserList");
            });

            modelBuilder.Entity<BusinessIncomingInvoiceList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Maturity)
                    .WithMany(p => p.BusinessIncomingInvoiceLists)
                    .HasForeignKey(d => d.MaturityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomingInvoiceList_MaturityList");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.BusinessIncomingInvoiceLists)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomingInvoiceList_PaymentMethodList");

                entity.HasOne(d => d.PaymentStatus)
                    .WithMany(p => p.BusinessIncomingInvoiceLists)
                    .HasForeignKey(d => d.PaymentStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomingInvoiceList_PaymentStatusList");

                entity.HasOne(d => d.TotalCurrency)
                    .WithMany(p => p.BusinessIncomingInvoiceLists)
                    .HasForeignKey(d => d.TotalCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomingInvoiceList_CurrencyList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessIncomingInvoiceLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomingInvoiceList_UserList");
            });

            modelBuilder.Entity<BusinessIncomingInvoiceSupportList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DocumentNumberNavigation)
                    .WithMany(p => p.BusinessIncomingInvoiceSupportLists)
                    .HasPrincipalKey(p => p.DocumentNumber)
                    .HasForeignKey(d => d.DocumentNumber)
                    .HasConstraintName("FK_IncomingInvoiceItemList_IncomingInvoiceList");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.BusinessIncomingInvoiceSupportLists)
                    .HasPrincipalKey(p => p.Name)
                    .HasForeignKey(d => d.Unit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomingInvoiceItemList_UnitList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessIncomingInvoiceSupportLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomingInvoiceItemList_UserList");
            });

            modelBuilder.Entity<BusinessIncomingOrderList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.TotalCurrency)
                    .WithMany(p => p.BusinessIncomingOrderLists)
                    .HasForeignKey(d => d.TotalCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomingOrderList_CurrencyList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessIncomingOrderLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomingOrderList_UserList");
            });

            modelBuilder.Entity<BusinessIncomingOrderSupportList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DocumentNumberNavigation)
                    .WithMany(p => p.BusinessIncomingOrderSupportLists)
                    .HasPrincipalKey(p => p.DocumentNumber)
                    .HasForeignKey(d => d.DocumentNumber)
                    .HasConstraintName("FK_IncomingOrderItemList_IncomingOrderList");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.BusinessIncomingOrderSupportLists)
                    .HasPrincipalKey(p => p.Name)
                    .HasForeignKey(d => d.Unit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomingOrderItemList_UnitList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessIncomingOrderSupportLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomingOrderItemList_UserList");
            });

            modelBuilder.Entity<BusinessMaturityList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessMaturityLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaturityList_UserList");
            });

            modelBuilder.Entity<BusinessNotesList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessNotesLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NotesList_UserList");
            });

            modelBuilder.Entity<BusinessOfferList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.TotalCurrency)
                    .WithMany(p => p.BusinessOfferLists)
                    .HasForeignKey(d => d.TotalCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OfferList_CurrencyList1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessOfferLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OfferList_UserList");
            });

            modelBuilder.Entity<BusinessOfferSupportList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DocumentNumberNavigation)
                    .WithMany(p => p.BusinessOfferSupportLists)
                    .HasPrincipalKey(p => p.DocumentNumber)
                    .HasForeignKey(d => d.DocumentNumber)
                    .HasConstraintName("FK_OfferItemList_OfferList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessOfferSupportLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OfferItemList_UserList");
            });

            modelBuilder.Entity<BusinessOutgoingInvoiceList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CreditNote)
                    .WithMany(p => p.BusinessOutgoingInvoiceLists)
                    .HasForeignKey(d => d.CreditNoteId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_OutgoingInvoiceList_CreditNoteList");

                entity.HasOne(d => d.Maturity)
                    .WithMany(p => p.BusinessOutgoingInvoiceLists)
                    .HasForeignKey(d => d.MaturityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutgoingInvoiceList_MaturityList");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.BusinessOutgoingInvoiceLists)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutgoingInvoiceList_PaymentMethodList");

                entity.HasOne(d => d.PaymentStatus)
                    .WithMany(p => p.BusinessOutgoingInvoiceLists)
                    .HasForeignKey(d => d.PaymentStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutgoingInvoiceList_PaymentStatusList");

                entity.HasOne(d => d.Receipt)
                    .WithMany(p => p.BusinessOutgoingInvoiceLists)
                    .HasForeignKey(d => d.ReceiptId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_OutgoingInvoiceList_ReceiptList");

                entity.HasOne(d => d.TotalCurrency)
                    .WithMany(p => p.BusinessOutgoingInvoiceLists)
                    .HasForeignKey(d => d.TotalCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutgoingInvoiceList_CurrencyList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessOutgoingInvoiceLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutgoingInvoiceList_UserList");
            });

            modelBuilder.Entity<BusinessOutgoingInvoiceSupportList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DocumentNumberNavigation)
                    .WithMany(p => p.BusinessOutgoingInvoiceSupportLists)
                    .HasPrincipalKey(p => p.DocumentNumber)
                    .HasForeignKey(d => d.DocumentNumber)
                    .HasConstraintName("FK_OutgoingInvoiceItemList_OutgoingInvoiceList");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.BusinessOutgoingInvoiceSupportLists)
                    .HasPrincipalKey(p => p.Name)
                    .HasForeignKey(d => d.Unit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutgoingInvoiceItemList_UnitList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessOutgoingInvoiceSupportLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutgoingInvoiceItemList_UserList");
            });

            modelBuilder.Entity<BusinessOutgoingOrderList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.TotalCurrency)
                    .WithMany(p => p.BusinessOutgoingOrderLists)
                    .HasForeignKey(d => d.TotalCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutgoingOrderList_CurrencyList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessOutgoingOrderLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutgoingOrderList_UserList");
            });

            modelBuilder.Entity<BusinessOutgoingOrderSupportList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DocumentNumberNavigation)
                    .WithMany(p => p.BusinessOutgoingOrderSupportLists)
                    .HasPrincipalKey(p => p.DocumentNumber)
                    .HasForeignKey(d => d.DocumentNumber)
                    .HasConstraintName("FK_OutgoingOrderItemList_OutgoingOrderList");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.BusinessOutgoingOrderSupportLists)
                    .HasPrincipalKey(p => p.Name)
                    .HasForeignKey(d => d.Unit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutgoingOrderItemList_UnitList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessOutgoingOrderSupportLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutgoingOrderItemList_UserList");
            });

            modelBuilder.Entity<BusinessPaymentMethodList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessPaymentMethodLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentMethodList_UserList");
            });

            modelBuilder.Entity<BusinessPaymentStatusList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessPaymentStatusLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentStatusList_UserList");
            });

            modelBuilder.Entity<BusinessReceiptList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.InvoiceNumberNavigation)
                    .WithMany(p => p.BusinessReceiptLists)
                    .HasPrincipalKey(p => p.DocumentNumber)
                    .HasForeignKey(d => d.InvoiceNumber)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ReceiptList_OutgoingInvoiceList");

                entity.HasOne(d => d.TotalCurrency)
                    .WithMany(p => p.BusinessReceiptLists)
                    .HasForeignKey(d => d.TotalCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReceiptList_CurrencyList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessReceiptLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReceiptList_UserList");
            });

            modelBuilder.Entity<BusinessReceiptSupportList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DocumentNumberNavigation)
                    .WithMany(p => p.BusinessReceiptSupportLists)
                    .HasPrincipalKey(p => p.DocumentNumber)
                    .HasForeignKey(d => d.DocumentNumber)
                    .HasConstraintName("FK_ReceiptItemList_ReceiptList");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.BusinessReceiptSupportLists)
                    .HasPrincipalKey(p => p.Name)
                    .HasForeignKey(d => d.Unit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReceiptItemList_UnitList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessReceiptSupportLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReceiptItemList_UserList");
            });

            modelBuilder.Entity<BusinessWarehouseList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastStockTaking).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessWarehouseLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WarehouseList_UserList");
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

            modelBuilder.Entity<CodeLibraryList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CodeLibraryLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CodeLibraryList_UserList");
            });

            modelBuilder.Entity<CountryAreaCityList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CountryAreaCityLists)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_CountryAreaCityList_CityList");

                entity.HasOne(d => d.Icac)
                    .WithMany(p => p.CountryAreaCityLists)
                    .HasForeignKey(d => d.Icacid)
                    .HasConstraintName("FK_CountryAreaCityList_CountryAreaList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CountryAreaCityLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountryAreaCityList_UserList");
            });

            modelBuilder.Entity<CountryAreaList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CountryAreaLists)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountryAreaList_CountryList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CountryAreaLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountryAreaList_UserList");
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

            modelBuilder.Entity<CreditPackageList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.CreditPackageLists)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreditPackageList_CurrencyList1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CreditPackageLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreditPackageList_UserList");
            });

            modelBuilder.Entity<DocSrvDocTemplateList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.DocSrvDocTemplateLists)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocSrvDocTemplateList_DocSrvDocumentationGroupList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DocSrvDocTemplateLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocSrvDocTemplateList_UserList");
            });

            modelBuilder.Entity<DocumentationCodeLibraryList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DocumentationCodeLibraryLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentationCodeLibraryList_UserList");
            });

            modelBuilder.Entity<DocumentationGroupList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DocumentationGroupLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentationGroupList_UserList");
            });

            modelBuilder.Entity<DocumentationList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DocumentationGroup)
                    .WithMany(p => p.DocumentationLists)
                    .HasForeignKey(d => d.DocumentationGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentationList_DocumentationGroupList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DocumentationLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentationList_UserList");
            });

            modelBuilder.Entity<GetTopFiveFavoriteList>(entity =>
            {
                entity.ToView("GetTopFiveFavoriteList");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<GuestAdvertiserNoteList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.GuestAdvertiserNoteLists)
                    .HasForeignKey(d => d.GuestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GuestAdvertiserNoteList_GuestList");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.GuestAdvertiserNoteLists)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GuestAdvertiserNoteList_HotelList");
            });

            modelBuilder.Entity<GuestFavoriteList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.GuestFavoriteLists)
                    .HasForeignKey(d => d.GuestId)
                    .HasConstraintName("FK_GuestFavoriteList_GuestList");
            });

            modelBuilder.Entity<GuestList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GuestLists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_GuestList_UserList");
            });

            modelBuilder.Entity<GuestLoginHistoryList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<GuestSettingList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.GuestSettingLists)
                    .HasForeignKey(d => d.GuestId)
                    .HasConstraintName("FK_GuestSettingList_GuestList");
            });

            modelBuilder.Entity<HolidayTipsList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HolidayTipsLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HolidayTipsList_UserList");
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
                    .HasConstraintName("FK_HotelList_BasicCurrencyList");

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
                entity.Property(e => e.CurrencyId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.HotelReservationDetailLists)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservationDetailList_CurrencyList1");

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

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.HotelReservationDetailLists)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservationDetailList_HotelReservationStatusList");
            });

            modelBuilder.Entity<HotelReservationList>(entity =>
            {
                entity.Property(e => e.CurrencyId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.HotelReservationLists)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservationList_CurrencyList1");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.HotelReservationLists)
                    .HasForeignKey(d => d.GuestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservations_GuestList");

                entity.HasOne(d => d.HotelAccommodationAction)
                    .WithMany(p => p.HotelReservationLists)
                    .HasForeignKey(d => d.HotelAccommodationActionId)
                    .HasConstraintName("FK_HotelReservationList_HotelAccomodationActionList");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelReservationLists)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservations_HotelList");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.HotelReservationLists)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservationList_HotelReservationStatusList");
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

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelReservedRoomLists)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservedRoomList_HotelList");

                entity.HasOne(d => d.HotelRoom)
                    .WithMany(p => p.HotelReservedRoomLists)
                    .HasForeignKey(d => d.HotelRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservedRoomList_HotelRoomList");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.HotelReservedRoomLists)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK_HotelReservedRoomList_ReservationList");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.HotelReservedRoomLists)
                    .HasForeignKey(d => d.RoomTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservedRoomList_HotelRoomTypeList");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.HotelReservedRoomLists)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelReservedRoomList_HotelReservationStatusList");
            });

            modelBuilder.Entity<HotelRoomList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelRoomLists)
                    .HasForeignKey(d => d.HotelId)
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

            modelBuilder.Entity<OftenQuestionList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OftenQuestionLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OftenQuestionList_UserList");
            });

            modelBuilder.Entity<PrivacyPolicyList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PrivacyPolicyLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrivacyPolicyList_UserList");
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

            modelBuilder.Entity<RegistrationInfoList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RegistrationInfoLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegistrationInfoList_UserList");
            });

            modelBuilder.Entity<ServerBrowsablePathList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServerBrowsablePathLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServerBrowsablePathList_UserList");
            });

            modelBuilder.Entity<ServerCorsDefAllowedOriginList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServerCorsDefAllowedOriginLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServerCorsDefAllowedOriginList_UserList");
            });

            modelBuilder.Entity<ServerHealthCheckTaskList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServerHealthCheckTaskLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HealthCheckTaskList_UserList");
            });

            modelBuilder.Entity<ServerLiveDataMonitorList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServerLiveDataMonitorLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServerLiveDataMonitorList_UserList");
            });

            modelBuilder.Entity<ServerModuleAndServiceList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServerModuleAndServiceLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServerModuleAndServiceList_UserList");
            });

            modelBuilder.Entity<ServerSettingList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Type).HasDefaultValueSql("('bit')");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServerSettingLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServerSettingList_SolutionUserList");
            });

            modelBuilder.Entity<ServerToolPanelDefinitionList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ToolType)
                    .WithMany(p => p.ServerToolPanelDefinitionLists)
                    .HasForeignKey(d => d.ToolTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServerToolPanelDefinitionList_ToolTypeList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServerToolPanelDefinitionLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServerToolPanelDefinitionList_UserList");
            });

            modelBuilder.Entity<ServerToolTypeList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServerToolTypeLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServerToolTypeList_UserList");
            });

            modelBuilder.Entity<SolutionEmailTemplateList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.SystemLanguage)
                    .WithMany(p => p.SolutionEmailTemplateLists)
                    .HasForeignKey(d => d.SystemLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmailTemplateList_SystemLanguageList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SolutionEmailTemplateLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmailTemplateList_UserList");
            });

            modelBuilder.Entity<SolutionEmailerHistoryList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SolutionFailList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SolutionFailLists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SolutionFailList_UserList");
            });

            modelBuilder.Entity<SolutionLanguageList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SolutionLanguageLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServerLanguageList_UserList");
            });

            modelBuilder.Entity<SolutionMessageModuleList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.SolutionMessageModuleLists)
                    .HasForeignKey(d => d.GuestId)
                    .HasConstraintName("FK_SolutionMessageModuleList_GuestList");

                entity.HasOne(d => d.MessageParent)
                    .WithMany(p => p.InverseMessageParent)
                    .HasForeignKey(d => d.MessageParentId)
                    .HasConstraintName("FK_SolutionMessageModuleList_SolutionMessageModuleListParent");

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.SolutionMessageModuleLists)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolutionMessageModuleList_SolutionMessageTypeList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SolutionMessageModuleLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolutionMessageModuleList_SolutionUserList");
            });

            modelBuilder.Entity<SolutionMessageTypeList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SolutionMessageTypeLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolutionMessageTypeList_SolutionUserList");
            });

            modelBuilder.Entity<SolutionMixedEnumList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SolutionMixedEnumLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GlobalMixedEnumList_UserList");
            });

            modelBuilder.Entity<SolutionMottoList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SolutionMottoLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MottoList_UserList");
            });

            modelBuilder.Entity<SolutionOperationList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SolutionOperationLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolutionOperationList_UserList");
            });

            modelBuilder.Entity<SolutionSchedulerList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SolutionSchedulerLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GlobalAutoSchedulerList_UserList");
            });

            modelBuilder.Entity<SolutionSchedulerProcessList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ScheduledTask)
                    .WithMany(p => p.SolutionSchedulerProcessLists)
                    .HasForeignKey(d => d.ScheduledTaskId)
                    .HasConstraintName("FK_SolutionSchedulerProcessList_SolutionSchedulerList");
            });

            modelBuilder.Entity<SolutionUserList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.SolutionUserLists)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserList_UserRoleList");
            });

            modelBuilder.Entity<SolutionUserRoleList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SystemCustomPageList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemCustomPageLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemCustomPageList_UserList");
            });

            modelBuilder.Entity<SystemDocumentAdviceList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.SystemDocumentAdviceLists)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentAdviceList_BranchList");

                entity.HasOne(d => d.DocumentTypeNavigation)
                    .WithMany(p => p.SystemDocumentAdviceLists)
                    .HasPrincipalKey(p => p.SystemName)
                    .HasForeignKey(d => d.DocumentType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentAdviceList_DocumentTypeList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemDocumentAdviceLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentAdvice_UserList");
            });

            modelBuilder.Entity<SystemDocumentTypeList>(entity =>
            {
                entity.Property(e => e.SystemName).HasDefaultValueSql("('MustProgramming')");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemDocumentTypeLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentTypeList_UserList");
            });

            modelBuilder.Entity<SystemGroupMenuList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemGroupMenuLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemGroupMenuList_UserList");
            });

            modelBuilder.Entity<SystemIgnoredExceptionList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemIgnoredExceptionLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IgnoredExceptionList_UserList");
            });

            modelBuilder.Entity<SystemLoginHistoryList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SystemMenuList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.SystemMenuLists)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemMenuList_SystemGroupMenuList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemMenuLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemMenuList_UserList");
            });

            modelBuilder.Entity<SystemModuleList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemModuleLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemModuleList_UserList");
            });

            modelBuilder.Entity<SystemParameterList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemParameterLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ParameterList_UserList");
            });

            modelBuilder.Entity<SystemReportList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemReportLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportList_UserList");
            });

            modelBuilder.Entity<SystemReportQueueList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SystemSvgIconList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemSvgIconLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SvgIconList_UserList");
            });

            modelBuilder.Entity<SystemTranslationList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemTranslationLists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SystemTranslationList_UserList");
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

            modelBuilder.Entity<TermsList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TermsLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TermsList_UserList");
            });

            modelBuilder.Entity<UbytkacInfoList>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UbytkacInfoLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UbytkacInfoList_UserList");
            });

            modelBuilder.Entity<WebMottoList>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WebMottoLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WebMottoList_UserList");
            });

            modelBuilder.Entity<WebSettingList>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WebSettingLists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_WebSettingList_UserList");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
