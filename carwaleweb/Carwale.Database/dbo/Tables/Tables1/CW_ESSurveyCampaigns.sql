CREATE TABLE [dbo].[CW_ESSurveyCampaigns] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [MakeId]    INT      NULL,
    [ModelId]   INT      NULL,
    [StartDate] DATETIME NOT NULL,
    [EndDate]   DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

