CREATE TABLE [dbo].[BW_PQ_DealerCampaigns] (
    [Id]                      INT           IDENTITY (1, 1) NOT NULL,
    [ContractId]              INT           NOT NULL,
    [DealerId]                INT           NOT NULL,
    [DealerName]              VARCHAR (80)  NOT NULL,
    [Number]                  VARCHAR (50)  NOT NULL,
    [IsActive]                BIT           NOT NULL,
    [DealerEmailId]           VARCHAR (250) NULL,
    [DealerLeadServingRadius] INT           NULL,
    [IsBookingAvailable]      BIT           DEFAULT ((0)) NULL,
    [EntryDate]               DATETIME      DEFAULT (getdate()) NULL,
    [EnteredBy]               INT           NULL,
    [UpdatedBy]               INT           NULL,
    [UpdatedOn]               DATETIME      NULL
);

