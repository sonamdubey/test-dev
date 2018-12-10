CREATE TABLE [dbo].[DLS_EventSubTypes] (
    [Id]       NUMERIC (18) NOT NULL,
    [EventId]  NUMERIC (18) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_DLS_EventSubTypes_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DLS_EventSubTypes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

