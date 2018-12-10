CREATE TABLE [dbo].[DLS_EventTypes] (
    [Id]       NUMERIC (18) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_DLS_EventTypes_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DLS_EventTypes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

