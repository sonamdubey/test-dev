CREATE TABLE [dbo].[DCRM_DB_Renewals] (
    [Id]             INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]       INT      NULL,
    [ContractId]     INT      NULL,
    [CampaignId]     INT      NULL,
    [PkgExpiryDate]  DATETIME CONSTRAINT [DF_DCRM_DB_Renewals_PkgExpiryDate] DEFAULT (getdate()) NULL,
    [BusinessUnitId] INT      NULL,
    [UserId]         INT      NULL,
    [IsRenewed]      BIT      NULL,
    [UpdatedOn]      DATETIME NULL,
    [EntryDate]      DATETIME CONSTRAINT [DF_DCRM_DB_Renewals_EntryDate] DEFAULT (getdate()) NOT NULL
);

