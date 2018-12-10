CREATE TABLE [dbo].[CRM_ProductStatus] (
    [ID]       INT           NOT NULL,
    [Name]     VARCHAR (100) NOT NULL,
    [IsClosed] BIT           NOT NULL,
    [IsActive] BIT           CONSTRAINT [DF_CRM_ProductStages_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CNS_ProductStages] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

