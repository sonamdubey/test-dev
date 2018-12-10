CREATE TABLE [dbo].[AbSure_WarrantyTypes] (
    [AbSure_WarrantyTypesId] INT           IDENTITY (1, 1) NOT NULL,
    [Warranty]               VARCHAR (100) NULL,
    [IsActive]               BIT           CONSTRAINT [DF_AbSure_WarrantyTypes_IsActive] DEFAULT ((1)) NULL,
    [IsWarranty]             BIT           DEFAULT ((0)) NOT NULL,
    [IsVisible]              BIT           NULL,
    CONSTRAINT [PK_AbSure_WarrantyTypes] PRIMARY KEY CLUSTERED ([AbSure_WarrantyTypesId] ASC)
);

