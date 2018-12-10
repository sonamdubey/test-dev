CREATE TABLE [dbo].[DCRM_DB_NewPaidDealers_backup09032016] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]  INT          NOT NULL,
    [PackageId] INT          NOT NULL,
    [StartDate] DATETIME     NULL,
    [EndDate]   DATETIME     NULL,
    [UserId]    INT          NULL,
    [EntryDate] DATETIME     NOT NULL
);

