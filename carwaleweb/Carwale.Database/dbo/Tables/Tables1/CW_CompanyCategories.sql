CREATE TABLE [dbo].[CW_CompanyCategories] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [CategoryName]   VARCHAR (100) NULL,
    [ProcessingFees] INT           NULL,
    [ROI]            FLOAT (53)    NULL,
    [IsActive]       BIT           CONSTRAINT [DF_CW_CompanyCategories_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_CW_CompanyCategories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

