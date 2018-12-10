CREATE TABLE [dbo].[DCRM_CallCategories] (
    [Id]             INT           NOT NULL,
    [CallCategories] VARCHAR (100) NOT NULL,
    [IsActive]       BIT           CONSTRAINT [DF_DCRM_CallCategories_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DCRM_CallCategories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

