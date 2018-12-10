﻿CREATE TABLE [dbo].[CMS_Campaigns] (
    [ID]                 NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AgencyId]           NUMERIC (18)   NOT NULL,
    [CampaignName]       VARCHAR (150)  NOT NULL,
    [CampaignType]       SMALLINT       NOT NULL,
    [CampaignCategory]   INT            NULL,
    [StartDate]          DATETIME       NULL,
    [EndDate]            DATETIME       NULL,
    [BookedQuantity]     NUMERIC (18)   NULL,
    [Rate]               DECIMAL (9, 2) NULL,
    [BookedAmount]       DECIMAL (9, 2) NULL,
    [RONumber]           VARCHAR (50)   NULL,
    [RODate]             DATETIME       NULL,
    [DeliveredQuantity]  NUMERIC (18)   NULL,
    [DeliveryStatus]     BIT            CONSTRAINT [DF_CMS_Campaigns_DeliveryStatus] DEFAULT ((0)) NULL,
    [InvoiceNo]          VARCHAR (50)   NULL,
    [InvoiceAmount]      DECIMAL (18)   NULL,
    [InvoiceDate]        DATETIME       NULL,
    [ReceivedAmount]     DECIMAL (18)   NULL,
    [ReceivedOn]         DATETIME       NULL,
    [ChequeDDNo]         VARCHAR (100)  NULL,
    [ChequeDDDate]       DATETIME       NULL,
    [BillStatus]         SMALLINT       NULL,
    [IsActive]           BIT            NULL,
    [ROFilePath]         VARCHAR (100)  NULL,
    [InvoiceFilePath]    VARCHAR (100)  NULL,
    [CampaignStatus]     SMALLINT       CONSTRAINT [DF_CMS_Campaigns_CampaignStatus] DEFAULT ((1)) NOT NULL,
    [QueryUpdate]        VARCHAR (1000) NULL,
    [LastQuantityUpdate] DATETIME       NULL,
    [Comments]           VARCHAR (2000) NULL,
    [EntryDate]          DATETIME       CONSTRAINT [DF_CMS_Campaigns_EntryDate] DEFAULT (getdate()) NULL,
    [IsReplicated]       BIT            DEFAULT ((1)) NULL,
    [HostURL]            VARCHAR (100)  DEFAULT ('img.carwale.com') NULL,
    CONSTRAINT [PK_CMS_Campaigns] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

