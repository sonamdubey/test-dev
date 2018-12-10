CREATE TABLE [dbo].[ActiveDealers] (
    [ADID]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]    NUMERIC (18) NOT NULL,
    [isActive]    BIT          CONSTRAINT [DF_ActiveDealers_isActive] DEFAULT ((1)) NOT NULL,
    [HasShowroom] BIT          CONSTRAINT [DF_ActiveDealers_HasShowroom] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ActiveDealers] PRIMARY KEY CLUSTERED ([ADID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_ActiveDealers_DealerId]
    ON [dbo].[ActiveDealers]([DealerId] ASC);

