CREATE TABLE [dbo].[CMS_SubCampaigns] (
    [Id]                NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CampaignId]        NUMERIC (18)  NOT NULL,
    [CampaignType]      INT           NOT NULL,
    [Name]              VARCHAR (150) NOT NULL,
    [StartDate]         DATETIME      NOT NULL,
    [EndDate]           DATETIME      NOT NULL,
    [BookedQuantity]    NUMERIC (18)  NULL,
    [Rate]              DECIMAL (18)  NULL,
    [BookedAmount]      DECIMAL (18)  NULL,
    [UpdatedOn]         DATETIME      NULL,
    [EntryDate]         DATETIME      CONSTRAINT [DF_CMS_SubCampaigns_EntryDate] DEFAULT (getdate()) NOT NULL,
    [DeliveredQuantity] NUMERIC (18)  NULL,
    CONSTRAINT [PK_CMS_SubCampaigns] PRIMARY KEY CLUSTERED ([Id] ASC)
);

