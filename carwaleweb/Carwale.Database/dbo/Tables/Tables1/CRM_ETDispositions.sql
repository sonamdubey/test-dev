CREATE TABLE [dbo].[CRM_ETDispositions] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]      VARCHAR (150) NOT NULL,
    [EventType] NUMERIC (18)  NOT NULL,
    [IsActive]  BIT           CONSTRAINT [DF_CRM_ETDispositions_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CRM_ETDispositions] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_ETDispositions_EventType]
    ON [dbo].[CRM_ETDispositions]([EventType] ASC, [IsActive] ASC);

