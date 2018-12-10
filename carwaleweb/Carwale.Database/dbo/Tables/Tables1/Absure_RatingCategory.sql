CREATE TABLE [dbo].[Absure_RatingCategory] (
    [Absure_RatingCategoryId] TINYINT       IDENTITY (1, 1) NOT NULL,
    [CategoryText]            VARCHAR (50)  NOT NULL,
    [OrderPriority]           TINYINT       NOT NULL,
    [IsActive]                BIT           NOT NULL,
    [Description]             VARCHAR (150) NULL,
    CONSTRAINT [PK_Absure_RatingCategory] PRIMARY KEY CLUSTERED ([Absure_RatingCategoryId] ASC)
);

