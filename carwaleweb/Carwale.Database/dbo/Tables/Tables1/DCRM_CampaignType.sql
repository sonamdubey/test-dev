CREATE TABLE [dbo].[DCRM_CampaignType] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [CampaignType] VARCHAR (50) NULL,
    [IsActive]     BIT          DEFAULT ((1)) NULL
);

