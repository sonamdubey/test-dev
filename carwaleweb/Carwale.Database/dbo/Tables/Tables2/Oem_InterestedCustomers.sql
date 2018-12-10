CREATE TABLE [dbo].[Oem_InterestedCustomers] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MakeId]       SMALLINT      NOT NULL,
    [CustomerName] VARCHAR (50)  NOT NULL,
    [Email]        VARCHAR (100) NOT NULL,
    [Phone]        VARCHAR (15)  NULL,
    [Mobile]       VARCHAR (15)  NULL,
    [CityId]       NCHAR (10)    NOT NULL,
    [VersionId]    INT           NOT NULL,
    [EntryDate]    DATETIME      NOT NULL,
    [Source]       VARCHAR (50)  NULL,
    [VersionName]  VARCHAR (150) NULL,
    [ModelName]    VARCHAR (150) NULL,
    [ModelId]      NUMERIC (18)  NULL,
    [Price]        NUMERIC (18)  NULL,
    [utm_source]   VARCHAR (200) NULL,
    [utm_medium]   VARCHAR (200) NULL,
    [utm_content]  VARCHAR (200) NULL,
    [utm_campaign] VARCHAR (200) NULL,
    [FuelType]     VARCHAR (20)  NULL,
    [Transmission] VARCHAR (20)  NULL,
    CONSTRAINT [PK_Oem_InterestedCustomers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

