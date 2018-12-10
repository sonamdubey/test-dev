CREATE TABLE [dbo].[AbSure_QCategory] (
    [AbSure_QCategoryId] INT           IDENTITY (1, 1) NOT NULL,
    [Category]           VARCHAR (200) NULL,
    [IsActive]           BIT           CONSTRAINT [DF_AbSure_QCategory_IsActive] DEFAULT ((1)) NULL,
    [Sequence]           SMALLINT      NULL,
    CONSTRAINT [PK_AbSure_QCategory] PRIMARY KEY CLUSTERED ([AbSure_QCategoryId] ASC)
);

