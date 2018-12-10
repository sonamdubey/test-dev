CREATE TABLE [dbo].[MM_ProductTypes] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (80) NULL,
    [IsActive] BIT          CONSTRAINT [DF_MM_ProductTypes_IsActive] DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

