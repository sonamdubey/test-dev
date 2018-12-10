CREATE TABLE [dbo].[CRM_ADM_QueueTeams] (
    [QueueId] NUMERIC (18) NOT NULL,
    [TeamId]  NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_ADM_QueueTeams] PRIMARY KEY CLUSTERED ([QueueId] ASC, [TeamId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CRM_ADM_QueueTeams_CRM_ADM_Queues] FOREIGN KEY ([QueueId]) REFERENCES [dbo].[CRM_ADM_Queues] ([Id]),
    CONSTRAINT [FK_CRM_ADM_QueueTeams_CRM_ADM_Teams] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[CRM_ADM_Teams] ([ID])
);

