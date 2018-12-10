CREATE TABLE [dbo].[AE_Yard] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]             VARCHAR (100) NOT NULL,
    [ShortCode]        VARCHAR (50)  NULL,
    [CityId]           NUMERIC (18)  NOT NULL,
    [Address]          VARCHAR (250) NULL,
    [Area]             VARCHAR (50)  NULL,
    [Lattitude]        VARCHAR (50)  NULL,
    [Longitude]        VARCHAR (50)  NULL,
    [AvailabilityTime] VARCHAR (50)  NULL,
    [Mobile]           VARCHAR (15)  NULL,
    [Landline]         VARCHAR (50)  NULL,
    [OtherContacts]    VARCHAR (50)  NULL,
    [FaxNo]            VARCHAR (50)  NULL,
    [ContactPerson]    VARCHAR (100) NULL,
    [ContactEmail]     VARCHAR (50)  NULL,
    [CWOwned]          BIT           NULL,
    CONSTRAINT [PK_AE_Yard] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

