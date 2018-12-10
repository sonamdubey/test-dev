CREATE TABLE [dbo].[trkt_TaskType] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [TaskType] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_trk_TaskType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

