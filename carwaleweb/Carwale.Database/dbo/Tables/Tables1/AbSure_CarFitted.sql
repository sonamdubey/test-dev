CREATE TABLE [dbo].[AbSure_CarFitted] (
    [Id]        INT          NOT NULL,
    [Name]      VARCHAR (50) NULL,
    [IsActive]  BIT          NULL,
    [IsAllowed] BIT          NULL,
    CONSTRAINT [PK_AbSure_CarFitted] PRIMARY KEY CLUSTERED ([Id] ASC)
);

