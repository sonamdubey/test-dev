CREATE TABLE [dbo].[CarSubSegments] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [IsActive] BIT          CONSTRAINT [DF_CarSubSegments_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CarSubSegments] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

