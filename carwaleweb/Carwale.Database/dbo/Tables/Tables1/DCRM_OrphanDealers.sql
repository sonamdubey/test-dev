CREATE TABLE [dbo].[DCRM_OrphanDealers] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]  NUMERIC (18) NOT NULL,
    [EntryDate] DATETIME     CONSTRAINT [DF_DCRM_OrphanDealers_EntryDate] DEFAULT (getdate()) NOT NULL,
    [Type]      SMALLINT     NULL,
    CONSTRAINT [PK_DCRM_OrphanDealers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

