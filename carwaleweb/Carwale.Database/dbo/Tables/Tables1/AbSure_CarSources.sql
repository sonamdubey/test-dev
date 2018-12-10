CREATE TABLE [dbo].[AbSure_CarSources] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (100) NULL,
    [IsActive] BIT           DEFAULT ((1)) NULL
);

