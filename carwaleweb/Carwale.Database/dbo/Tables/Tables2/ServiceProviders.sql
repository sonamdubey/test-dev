CREATE TABLE [dbo].[ServiceProviders] (
    [ID]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]       VARCHAR (150) NOT NULL,
    [WebsiteUrl] VARCHAR (100) NULL,
    [IsActive]   BIT           CONSTRAINT [DF_ServiceProviders_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_ServiceProviders] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

