CREATE TABLE [dbo].[TC_DealerFeatureLog] (
    [Id]                 INT          IDENTITY (1, 1) NOT NULL,
    [DealerId]           INT          NULL,
    [TC_DealerFeatureId] INT          NULL,
    [ActionDate]         DATE         NULL,
    [ActionTakenBy]      INT          NULL,
    [Action]             VARCHAR (10) NULL,
    CONSTRAINT [PK_TC_DealerFeatureLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

