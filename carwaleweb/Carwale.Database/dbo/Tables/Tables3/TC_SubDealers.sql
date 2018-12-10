CREATE TABLE [dbo].[TC_SubDealers] (
    [TC_DealerMappingId] INT      NOT NULL,
    [SubDealerId]        INT      NOT NULL,
    [IsActive]           BIT      CONSTRAINT [DF_TC_SubDealers_IsActive] DEFAULT ((1)) NULL,
    [EntryDate]          DATETIME NULL,
    [ModifiedDate]       DATETIME NULL
);

