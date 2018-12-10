CREATE TABLE [dbo].[Con_WrongDataLog] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [VersionId]      NUMERIC (18) NOT NULL,
    [FeatureId]      NUMERIC (18) NOT NULL,
    [CurrentValue]   FLOAT (53)   NOT NULL,
    [CapturedValue]  FLOAT (53)   NOT NULL,
    [SuggestedValue] FLOAT (53)   NOT NULL,
    [CustomerId]     INT          NOT NULL,
    [IsResolved]     BIT          NOT NULL,
    [ResolvedBy]     INT          NULL,
    [ResolvedOn]     DATETIME     NULL,
    CONSTRAINT [PK_Con_WrongDataLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

