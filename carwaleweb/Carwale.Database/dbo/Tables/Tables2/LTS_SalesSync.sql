CREATE TABLE [dbo].[LTS_SalesSync] (
    [id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CampaignId]   NUMERIC (18)  NOT NULL,
    [CampaignCode] VARCHAR (100) NOT NULL,
    [Count]        NUMERIC (18)  NOT NULL,
    [SyncDate]     DATETIME      NOT NULL,
    [Type]         SMALLINT      CONSTRAINT [DF_LTS_SalesSync_Type] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_LTS_SalesSync] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 For New Car Sales and 2 For Used Car Sales', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LTS_SalesSync', @level2type = N'COLUMN', @level2name = N'Type';

