CREATE TABLE [dbo].[TC_MM_MappedNumbers] (
    [TC_MM_MappedNumberId] INT          IDENTITY (1, 1) NOT NULL,
    [MaskingNumber]        VARCHAR (10) NULL,
    [MappedNumber]         VARCHAR (10) NULL,
    [DealerId]             INT          NULL,
    [TC_InquirySourceId]   INT          NULL,
    [IsPrimaryContact]     BIT          NULL,
    [EntryDate]            DATETIME     NULL,
    [EnteredBy]            INT          NULL,
    CONSTRAINT [PK_TC_MM_MappedNumbers] PRIMARY KEY CLUSTERED ([TC_MM_MappedNumberId] ASC)
);

