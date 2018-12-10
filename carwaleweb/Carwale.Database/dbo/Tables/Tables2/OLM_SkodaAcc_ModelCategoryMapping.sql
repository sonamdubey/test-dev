CREATE TABLE [dbo].[OLM_SkodaAcc_ModelCategoryMapping] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [ModelId]    INT NOT NULL,
    [CategoryId] INT NOT NULL,
    [IsActive]   BIT CONSTRAINT [DF_OLM_SkodaAcc_ModelCategoryMapping_IsActive] DEFAULT ((1)) NULL
);

