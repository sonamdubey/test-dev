CREATE TABLE [dbo].[AE_CarSellers] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarId]         NUMERIC (18)  NOT NULL,
    [Name]          VARCHAR (100) NULL,
    [Mobile]        VARCHAR (15)  NULL,
    [OtherContacts] VARCHAR (50)  NULL,
    [CityId]        NUMERIC (18)  NULL,
    [Address]       VARCHAR (150) NULL,
    [CreatedOn]     DATETIME      CONSTRAINT [DF_AE_CarSellers_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]     DATETIME      NULL,
    [UpdatedBy]     BIGINT        NULL,
    CONSTRAINT [PK_AE_CarSellers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

