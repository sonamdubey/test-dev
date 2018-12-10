CREATE TABLE [dbo].[ESM_Brands] (
    [id]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BrandName]  VARCHAR (50) NOT NULL,
    [ClientId]   NUMERIC (18) NOT NULL,
    [CategoryId] NUMERIC (18) NOT NULL,
    [RegionId]   NUMERIC (18) NOT NULL,
    [IsActive]   BIT          CONSTRAINT [DF_ESM_Brands_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedOn]  DATETIME     NOT NULL,
    [UpdatedBy]  NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ESM_Brands] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

