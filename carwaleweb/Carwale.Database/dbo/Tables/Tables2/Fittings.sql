CREATE TABLE [dbo].[Fittings] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]      VARCHAR (50) NOT NULL,
    [isDeleted] BIT          CONSTRAINT [DF_Fittings_isDeleted] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_Fittings] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

