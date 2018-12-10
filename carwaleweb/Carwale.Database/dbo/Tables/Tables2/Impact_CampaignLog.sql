CREATE TABLE [dbo].[Impact_CampaignLog] (
    [Impact_CampaignLogId] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Impact_CampaignId]    NUMERIC (18) NOT NULL,
    [MakeId]               INT          NOT NULL,
    [CityId]               INT          NOT NULL,
    [DealerId]             INT          NOT NULL,
    [PackageTypeId]        TINYINT      NOT NULL,
    [StartDate]            DATETIME     NOT NULL,
    [EndDate]              DATETIME     NOT NULL,
    [IsActive]             BIT          NOT NULL,
    [LogDate]              DATETIME     CONSTRAINT [DF_Impact_CampaignLog_LogDate] DEFAULT (getdate()) NOT NULL,
    [LoggedBy]             INT          NOT NULL,
    CONSTRAINT [PK_Impact_CampaignLog] PRIMARY KEY CLUSTERED ([Impact_CampaignLogId] ASC)
);

