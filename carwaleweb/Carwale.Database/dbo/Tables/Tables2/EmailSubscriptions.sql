CREATE TABLE [dbo].[EmailSubscriptions] (
    [Id]       INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_EmailSubscriptions_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_EmailSubscriptions] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

