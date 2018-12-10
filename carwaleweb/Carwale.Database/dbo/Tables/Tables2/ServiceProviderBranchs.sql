﻿CREATE TABLE [dbo].[ServiceProviderBranchs] (
    [ID]                NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ServiceProviderId] NUMERIC (18)  NOT NULL,
    [Address]           VARCHAR (250) NULL,
    [AreaId]            NUMERIC (18)  NULL,
    [CityId]            NUMERIC (18)  NULL,
    [StateId]           NUMERIC (18)  NOT NULL,
    [Telephone]         VARCHAR (100) NULL,
    [Fax]               VARCHAR (30)  NULL,
    [ContactPerson]     VARCHAR (100) NULL,
    [Mobile]            VARCHAR (15)  NULL,
    [Email1]            VARCHAR (100) NULL,
    [Email2]            VARCHAR (100) NULL,
    [ContactHours]      VARCHAR (50)  NULL,
    [LastUpdated]       DATETIME      NOT NULL,
    [PhotoUrl]          VARCHAR (100) NULL,
    [IsMain]            BIT           CONSTRAINT [DF_ServiceProviderBranchs_IsMain] DEFAULT (0) NOT NULL,
    [EntryDate]         DATETIME      NOT NULL,
    [IsActive]          BIT           CONSTRAINT [DF_ServiceProviderBranchs_IsActive] DEFAULT (1) NOT NULL,
    [IsReplicated]      BIT           DEFAULT ((1)) NULL,
    [HostURL]           VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    CONSTRAINT [PK_ServiceProviderBranchs] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ServiceProviderBranchs_Areas] FOREIGN KEY ([AreaId]) REFERENCES [dbo].[Areas] ([ID]),
    CONSTRAINT [FK_ServiceProviderBranchs_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([ID]),
    CONSTRAINT [FK_ServiceProviderBranchs_ServiceProviders] FOREIGN KEY ([ServiceProviderId]) REFERENCES [dbo].[ServiceProviders] ([ID]),
    CONSTRAINT [FK_ServiceProviderBranchs_States] FOREIGN KEY ([StateId]) REFERENCES [dbo].[States] ([ID])
);

