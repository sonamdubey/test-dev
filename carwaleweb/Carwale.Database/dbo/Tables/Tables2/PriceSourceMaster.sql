CREATE TABLE [dbo].[PriceSourceMaster] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (100) NOT NULL,
    [IsActive]  BIT           NOT NULL,
    [UpdatedBy] INT           NOT NULL,
    [UpdatedOn] DATETIME      NOT NULL,
    CONSTRAINT [PK_PriceSourceMaster] PRIMARY KEY CLUSTERED ([Id] ASC)
);

