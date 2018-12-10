CREATE TABLE [dbo].[CRM_ADM_Brands] (
    [MakeId]   NUMERIC (18) NOT NULL,
    [IsOEM]    BIT          CONSTRAINT [DF_CRM_ADM_Brands_IsOEM] DEFAULT ((1)) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_CRM_ADM_Brands_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CRM_ADM_Brands] PRIMARY KEY CLUSTERED ([MakeId] ASC)
);

