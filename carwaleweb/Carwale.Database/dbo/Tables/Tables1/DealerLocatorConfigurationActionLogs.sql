CREATE TABLE [dbo].[DealerLocatorConfigurationActionLogs] (
    [DealerLocatorConfigurationId] INT          NOT NULL,
    [DealerId]                     INT          NOT NULL,
    [PQ_DealerSponsoredId]         INT          NULL,
    [IsDealerLocatorPremium]       BIT          NULL,
    [IsLocatorActive]              BIT          NULL,
    [CreatedOn]                    DATETIME     NOT NULL,
    [CreatedBy]                    INT          NOT NULL,
    [LastUpdatedOn]                DATETIME     NOT NULL,
    [LastUpdatedBy]                INT          NOT NULL,
    [ActionTaken]                  VARCHAR (50) NULL,
    [ActionTakenOn]                DATETIME     CONSTRAINT [DF_dealerlocatorconfigurationActionLogs_ActionTakenOn] DEFAULT (getdate()) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_DealerLocatorConfigurationActionLogs_DealerId]
    ON [dbo].[DealerLocatorConfigurationActionLogs]([DealerId] ASC);

