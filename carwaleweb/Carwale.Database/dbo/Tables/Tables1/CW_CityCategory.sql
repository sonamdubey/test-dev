CREATE TABLE [dbo].[CW_CityCategory] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Category] VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_CW_CityCategory_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CW_CityCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

