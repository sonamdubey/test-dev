CREATE TABLE [dbo].[TC_ImportSellerInquiries] (
    [TC_ImportSellerInquiriesId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]                       VARCHAR (100) NULL,
    [Email]                      VARCHAR (100) NULL,
    [Mobile]                     VARCHAR (15)  NULL,
    [Location]                   VARCHAR (50)  NULL,
    [CarMake]                    VARCHAR (50)  NULL,
    [CarModel]                   VARCHAR (50)  NULL,
    [CarVersion]                 VARCHAR (50)  NULL,
    [Price]                      VARCHAR (50)  NULL,
    [IsValid]                    BIT           NULL,
    [IsDeleted]                  BIT           CONSTRAINT [DF_TC_ImportSellerInquiries_IsDeleted] DEFAULT ((0)) NULL,
    [UserId]                     BIGINT        NULL,
    [BranchId]                   BIGINT        NULL,
    [TC_InquiryOtherSourceId]    TINYINT       NULL,
    [TC_SellerInquiriesId]       BIGINT        NULL,
    [Eagerness]                  TINYINT       NULL,
    [IsNew]                      BIT           CONSTRAINT [DF_TC_ImportSellerInquiries_IsNew] DEFAULT ((0)) NULL,
    [EntryDate]                  DATETIME      NULL,
    [CarYear]                    INT           NULL,
    [CarMileage]                 INT           NULL,
    [CarColor]                   VARCHAR (50)  NULL,
    [RegistrationNo]             VARCHAR (15)  NULL,
    [Comments]                   VARCHAR (500) NULL,
    [TC_InquirySourceId]         TINYINT       NULL,
    [Address]                    VARCHAR (100) NULL,
    [ModifiedDate]               DATETIME      NULL,
    [LeadOwnerId]                INT           NULL,
    [ExcelSheetId]               INT           NULL,
    [VersionId]                  INT           NULL
);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndexIX_branchId]
    ON [dbo].[TC_ImportSellerInquiries]([BranchId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_ImportSellerInquiries_UserId]
    ON [dbo].[TC_ImportSellerInquiries]([UserId] ASC);

