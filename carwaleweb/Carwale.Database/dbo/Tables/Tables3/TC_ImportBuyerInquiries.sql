CREATE TABLE [dbo].[TC_ImportBuyerInquiries] (
    [TC_ImportBuyerInquiriesId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]                      VARCHAR (100) NULL,
    [Email]                     VARCHAR (100) NULL,
    [Mobile]                    VARCHAR (15)  NULL,
    [Location]                  VARCHAR (50)  NULL,
    [CarMake]                   VARCHAR (50)  NULL,
    [CarModel]                  VARCHAR (50)  NULL,
    [Price]                     VARCHAR (50)  NULL,
    [IsValid]                   BIT           NULL,
    [IsDeleted]                 BIT           CONSTRAINT [DF_TC_ImportBuyerInquiries_IsDeleted] DEFAULT ((0)) NULL,
    [UserId]                    BIGINT        NULL,
    [BranchId]                  BIGINT        NULL,
    [TC_InquiryOtherSourceId]   TINYINT       NULL,
    [TC_BuyerInquiriesId]       BIGINT        NULL,
    [Eagerness]                 TINYINT       NULL,
    [BuyingTime]                VARCHAR (50)  NULL,
    [IsNew]                     BIT           CONSTRAINT [DF_TC_ImportBuyerInquiries_IsNew] DEFAULT ((0)) NULL,
    [EntryDate]                 DATETIME      NULL,
    [CarYear]                   VARCHAR (50)  NULL,
    [CarDetails]                VARCHAR (100) NULL,
    [CarVersion]                VARCHAR (50)  NULL,
    [Comments]                  VARCHAR (500) NULL,
    [TC_InquirySourceId]        TINYINT       NULL,
    [ModifiedDate]              DATETIME      NULL,
    [LeadOwnerId]               INT           NULL,
    [ExcelSheetId]              INT           NULL,
    [MaxPrice]                  INT           NULL,
    [MinPrice]                  INT           NULL,
    [FromYear]                  SMALLINT      NULL,
    [ToYear]                    SMALLINT      NULL
);


GO
CREATE NONCLUSTERED INDEX [IsDeleted_UserId_BranchId_TC_BuyerInquiriesId]
    ON [dbo].[TC_ImportBuyerInquiries]([BranchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_ImportBuyerInquiries_EntryDate]
    ON [dbo].[TC_ImportBuyerInquiries]([EntryDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_ImportBuyerInquiries_TC_BuyerInquiriesId]
    ON [dbo].[TC_ImportBuyerInquiries]([TC_BuyerInquiriesId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_ImportBuyerInquiries_UserId]
    ON [dbo].[TC_ImportBuyerInquiries]([UserId] ASC);

