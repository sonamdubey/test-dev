CREATE TABLE [dbo].[AbSure_QSubCategory] (
    [AbSure_QSubCategoryId] INT           IDENTITY (1, 1) NOT NULL,
    [SubCategory]           VARCHAR (200) NULL,
    [AbSure_QCategoryId]    INT           NULL,
    [IsActive]              BIT           CONSTRAINT [DF_AbSure_QSubCategory_IsActive] DEFAULT ((1)) NULL,
    [IsParameterDependent]  BIT           NULL,
    CONSTRAINT [PK_AbSure_QSubCategory] PRIMARY KEY CLUSTERED ([AbSure_QSubCategoryId] ASC)
);

