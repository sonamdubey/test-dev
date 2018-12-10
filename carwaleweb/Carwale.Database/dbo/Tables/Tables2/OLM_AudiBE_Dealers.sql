CREATE TABLE [dbo].[OLM_AudiBE_Dealers] (
    [Id]             NUMERIC (18)  NOT NULL,
    [Name]           VARCHAR (50)  NULL,
    [Company]        VARCHAR (100) NULL,
    [ContactPerson]  VARCHAR (100) NULL,
    [Address]        VARCHAR (200) NULL,
    [CityId]         NUMERIC (18)  NULL,
    [ZipCode]        VARCHAR (10)  NULL,
    [Lattitude]      VARCHAR (50)  NULL,
    [Longitude]      VARCHAR (50)  NULL,
    [ContactNumbers] VARCHAR (100) NULL,
    [MobileNumber]   VARCHAR (15)  NULL,
    [EmailID]        VARCHAR (100) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_OLM_AudiBE_Dealers_IsActive] DEFAULT ((1)) NULL
);

