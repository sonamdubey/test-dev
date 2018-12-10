CREATE TABLE [dbo].[PQConversionCodes] (
    [PQConversionCodeId] INT            IDENTITY (1, 1) NOT NULL,
    [Make]               VARCHAR (50)   NULL,
    [CarModel]           VARCHAR (50)   NULL,
    [BuyCode]            CHAR (2)       NULL,
    [ModelId]            SMALLINT       NULL,
    [Name]               NVARCHAR (255) NULL,
    [ConversionId]       INT            NULL,
    [Label]              VARCHAR (50)   NULL,
    [Value]              INT            NULL
);

