CREATE TABLE [dbo].[BlockedNumbers] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [Mobile]         VARCHAR (20) NULL,
    [IsAutoInserted] BIT          CONSTRAINT [DF_BlockedNumbers_IsAutoInserted] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

