CREATE TABLE [dbo].[TC_ExcelOtherDetails] (
    [TC_ExcelOtherDetailsId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TC_LeadId]              BIGINT        NULL,
    [Make]                   VARCHAR (100) NULL,
    [Model]                  VARCHAR (100) NULL,
    [Version]                VARCHAR (50)  NULL,
    [EntryDate]              DATETIME      NULL,
    [UserId]                 BIGINT        NULL,
    [BranchId]               BIGINT        NULL,
    [PurchaseYear]           VARCHAR (15)  NULL,
    [TC_ExcelInquiryId]      BIGINT        NULL,
    [InquiryDate]            DATETIME      NULL,
    CONSTRAINT [PK_TC_ExcelOtherDetails] PRIMARY KEY CLUSTERED ([TC_ExcelOtherDetailsId] ASC)
);

