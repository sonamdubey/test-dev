CREATE TABLE [dbo].[CRM_TeamCapacityLog] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [RefType]   INT          NOT NULL,
    [TeamId]    INT          NOT NULL,
    [Capacity]  NUMERIC (18) NULL,
    [Actual]    NUMERIC (18) NULL,
    [EntryDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_CRM_TeamCapacityLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

