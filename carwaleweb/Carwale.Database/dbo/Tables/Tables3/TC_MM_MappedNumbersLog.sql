CREATE TABLE [dbo].[TC_MM_MappedNumbersLog] (
    [TC_MM_MappedNumbersLogId] INT          IDENTITY (1, 1) NOT NULL,
    [MappedNumber]             VARCHAR (10) NULL,
    [MaskingNumber]            VARCHAR (10) NULL,
    [DealerId]                 INT          NULL,
    [TC_InquirySourceId]       INT          NULL,
    [IsPrimaryContact]         BIT          NULL,
    [ModifiedOn]               DATETIME     NULL,
    [ModifiedBy]               INT          NULL,
    CONSTRAINT [PK_TC_MM_MappedNumbersLog] PRIMARY KEY CLUSTERED ([TC_MM_MappedNumbersLogId] ASC)
);

