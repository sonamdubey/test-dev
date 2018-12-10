CREATE TABLE [dbo].[TC_LeadSchedulingTypeMapping] (
    [Id]                      INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]                INT      NULL,
    [TC_LeadSchedulingTypeId] SMALLINT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

