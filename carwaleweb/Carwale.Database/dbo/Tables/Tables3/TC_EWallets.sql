CREATE TABLE [dbo].[TC_EWallets] (
    [Id]          SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)  NULL,
    [IsActive]    BIT           NULL,
    [Description] VARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

