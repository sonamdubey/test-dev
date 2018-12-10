CREATE TABLE [dbo].[TC_ExcelInquiries] (
    [Id]                        NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]                      VARCHAR (100) NULL,
    [Email]                     VARCHAR (100) NULL,
    [Mobile]                    VARCHAR (50)  NULL,
    [City]                      VARCHAR (50)  NULL,
    [CarMake]                   VARCHAR (50)  NULL,
    [CarModel]                  VARCHAR (50)  NULL,
    [IsValid]                   BIT           CONSTRAINT [TC_ExcelInquiries_IsValid] DEFAULT ((0)) NULL,
    [IsDeleted]                 BIT           CONSTRAINT [TC_ExcelInquiries_IsDeleted] DEFAULT ((0)) NOT NULL,
    [UserId]                    BIGINT        NULL,
    [BranchId]                  BIGINT        NULL,
    [TC_InquirySourceId]        SMALLINT      NULL,
    [TC_NewCarInquiriesId]      VARCHAR (100) NULL,
    [IsNew]                     BIT           DEFAULT ((0)) NULL,
    [EntryDate]                 DATETIME      NULL,
    [ModifiedDate]              DATETIME      NULL,
    [LeadOwnerId]               INT           NULL,
    [ExcelSheetId]              INT           NULL,
    [CityId]                    INT           NULL,
    [VersionId]                 INT           NULL,
    [Comments]                  VARCHAR (500) DEFAULT (NULL) NULL,
    [CarVersion]                VARCHAR (50)  NULL,
    [AlternateCarVersion]       VARCHAR (50)  NULL,
    [AlternateVersionId]        INT           NULL,
    [SalesConsultant]           VARCHAR (50)  NULL,
    [SalesConsultantId]         BIGINT        NULL,
    [InquirySource]             VARCHAR (50)  NULL,
    [InquiryDate]               VARCHAR (50)  NULL,
    [AlternateNo]               VARCHAR (15)  NULL,
    [CompanyName]               VARCHAR (150) NULL,
    [Area]                      VARCHAR (50)  NULL,
    [AreaId]                    INT           NULL,
    [CompanyId]                 INT           NULL,
    [PreferedVersionColour]     VARCHAR (150) NULL,
    [PreferedVersionColourIds]  VARCHAR (150) NULL,
    [AlternateModel]            VARCHAR (50)  NULL,
    [AlternateVersionColour]    VARCHAR (150) NULL,
    [AlternateVersionColourIds] VARCHAR (150) NULL,
    [IsEligibleForCorporate]    BIT           NULL,
    [IsWillingnessToExchange]   BIT           NULL,
    [IsFinanceRequired]         BIT           NULL,
    [IsAccessoriesRequired]     BIT           NULL,
    [IsSchemesOffered]          BIT           NULL,
    [IsTestDriveRequested]      BIT           NULL,
    [TestDriveDate]             VARCHAR (50)  NULL,
    [NextFollowUpDate]          VARCHAR (50)  NULL,
    [RecentComment]             VARCHAR (500) NULL,
    [RecentCommentDate]         VARCHAR (50)  NULL,
    [ActivityFeed]              VARCHAR (MAX) NULL,
    [IsCustomerSuspended]       BIT           NULL,
    [IsCarBooked]               BIT           NULL,
    [PanNo]                     VARCHAR (25)  NULL,
    [BookingAmount]             VARCHAR (25)  NULL,
    [BookingReceiptNo]          VARCHAR (25)  NULL,
    [TentativeDeliveryDate]     VARCHAR (50)  NULL,
    [IsAvailedFinance]          BIT           NULL,
    [IsBoughtAccessories]       BIT           NULL,
    [IsCarDelivered]            BIT           NULL,
    [DeliveryDate]              VARCHAR (50)  NULL,
    [IsLeadLost]                BIT           NULL,
    [LeadLostReason]            VARCHAR (50)  NULL,
    [LostDispositionId]         SMALLINT      NULL,
    [TDStatusId]                SMALLINT      NULL,
    [TDStatus]                  VARCHAR (50)  NULL,
    [Eagerness]                 VARCHAR (50)  NULL,
    [BuyingTime]                VARCHAR (25)  NULL,
    [BookingDate]               VARCHAR (50)  NULL,
    [IsSpecialUser]             BIT           NULL,
    [DealerCode]                VARCHAR (50)  NULL,
    [Salutation]                VARCHAR (10)  NULL,
    [LastName]                  VARCHAR (100) NULL,
    CONSTRAINT [PK_TC_ExcelInquiries_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-UserId ]
    ON [dbo].[TC_ExcelInquiries]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BranchId_B.IsDeleted_UserId_TC_NewCarInquiriesId]
    ON [dbo].[TC_ExcelInquiries]([BranchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_ExcelInquiries_TC_NewCarInquiriesId]
    ON [dbo].[TC_ExcelInquiries]([TC_NewCarInquiriesId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_ExcelInquiries_EntryDate]
    ON [dbo].[TC_ExcelInquiries]([EntryDate] ASC);

