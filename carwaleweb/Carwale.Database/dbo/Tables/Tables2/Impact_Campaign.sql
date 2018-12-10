CREATE TABLE [dbo].[Impact_Campaign] (
    [Impact_CampaignId] BIGINT   IDENTITY (1, 1) NOT NULL,
    [MakeId]            INT      NOT NULL,
    [CityId]            INT      NOT NULL,
    [DealerId]          INT      NOT NULL,
    [PackageTypeId]     TINYINT  NOT NULL,
    [StartDate]         DATETIME NOT NULL,
    [EndDate]           DATETIME NOT NULL,
    [IsActive]          BIT      CONSTRAINT [DF_Impact_Campaign_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]       DATETIME CONSTRAINT [DF_Impact_Campaign_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         INT      NOT NULL,
    CONSTRAINT [PK_Impact_Campaign] PRIMARY KEY CLUSTERED ([Impact_CampaignId] ASC)
);

