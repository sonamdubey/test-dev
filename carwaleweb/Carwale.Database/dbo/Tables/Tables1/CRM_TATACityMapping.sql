CREATE TABLE [dbo].[CRM_TATACityMapping] (
    [CityId]   NUMERIC (18) NOT NULL,
    [CityName] VARCHAR (50) NULL,
    CONSTRAINT [PK_CRM_TATACityMapping] PRIMARY KEY CLUSTERED ([CityId] ASC)
);

