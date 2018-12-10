CREATE TABLE [CRM].[FLCDealerPriority] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [ModelId]     NUMERIC (18) NOT NULL,
    [DealerId]    NUMERIC (18) NOT NULL,
    [Priority]    TINYINT      NOT NULL,
    [UpdatedDate] DATETIME     NULL,
    [UpdatedBy]   NUMERIC (18) NULL,
    CONSTRAINT [PK_FLCDealerPriority] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_FLCDealerPriority_ModelId]
    ON [CRM].[FLCDealerPriority]([ModelId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_FLCDealerPriority_DealerId]
    ON [CRM].[FLCDealerPriority]([DealerId] ASC);

