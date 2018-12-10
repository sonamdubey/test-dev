CREATE TABLE [dbo].[DCRM_DB_NewPaidDealers] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]  INT          NOT NULL,
    [PackageId] INT          NOT NULL,
    [StartDate] DATETIME     NULL,
    [EndDate]   DATETIME     NULL,
    [UserId]    INT          NULL,
    [EntryDate] DATETIME     CONSTRAINT [DF_DCRM_DB_NewPaidDealers_EntryDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_DCRM_DB_NewPaidDealers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

