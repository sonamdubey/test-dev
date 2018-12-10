CREATE TABLE [dbo].[CRM_ADM_DCDealers] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]  NUMERIC (18) NOT NULL,
    [DCID]      NUMERIC (18) NOT NULL,
    [UpdatedOn] DATETIME     NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    [CreatedOn] DATETIME     NULL,
    CONSTRAINT [PK_CRM_ADM_DCDealers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

