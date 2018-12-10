CREATE TABLE [dbo].[LTS_Sync] (
    [id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CampaignID]   BIGINT        NULL,
    [CampaignCode] VARCHAR (100) NULL,
    [TrackCount]   BIGINT        NULL,
    [SyncDate]     DATETIME      NULL,
    [Type]         BIGINT        NULL,
    CONSTRAINT [PK_LTS_Sync] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

