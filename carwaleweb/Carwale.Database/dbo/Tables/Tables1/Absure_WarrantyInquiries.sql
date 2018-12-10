CREATE TABLE [dbo].[Absure_WarrantyInquiries] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [WarrantyType]     SMALLINT      NULL,
    [WarrantyPrice]    FLOAT (53)    NULL,
    [VersionId]        INT           NOT NULL,
    [RegistrationNo]   VARCHAR (50)  NULL,
    [RegistrationDate] DATETIME      NULL,
    [FuelType]         TINYINT       NULL,
    [CarFittedWith]    TINYINT       NULL,
    [Kilometers]       INT           NULL,
    [CustomerName]     VARCHAR (30)  NULL,
    [CustomerEmail]    VARCHAR (50)  NULL,
    [CustomerMobile]   VARCHAR (10)  NULL,
    [CustomerAddress]  VARCHAR (100) NULL,
    [CityId]           INT           NULL,
    [AreaId]           INT           NULL,
    [AbsureCarId]      INT           NULL,
    [EntryDate]        DATETIME      NULL,
    [ProductType]      TINYINT       NULL,
    [VIN]              VARCHAR (50)  NULL,
    [EngineNumber]     VARCHAR (50)  NULL
);

