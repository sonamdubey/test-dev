CREATE TABLE [dbo].[Support_Category] (
    [Id]       NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (400) NOT NULL,
    [IsActive] BIT           CONSTRAINT [DF_Support_Category_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Support_Category] PRIMARY KEY CLUSTERED ([Id] ASC)
);

