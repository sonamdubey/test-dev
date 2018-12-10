CREATE TABLE [dbo].[DCRM_RestrictedDealerAlert] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]  NUMERIC (18) NOT NULL,
    [AlertId]   NUMERIC (18) NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    [UpdatedOn] DATETIME     NOT NULL,
    CONSTRAINT [PK_DCRM_RestrictedDealerAlert] PRIMARY KEY CLUSTERED ([Id] ASC)
);

