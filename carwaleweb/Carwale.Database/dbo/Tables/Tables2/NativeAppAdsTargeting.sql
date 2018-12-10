CREATE TABLE [dbo].[NativeAppAdsTargeting] (
    [Id]              INT IDENTITY (1, 1) NOT NULL,
    [CampaignId]      INT NOT NULL,
    [TargetModelId]   INT NOT NULL,
    [IsActive]        BIT DEFAULT ((0)) NOT NULL,
    [TargetVersionId] INT DEFAULT (NULL) NULL
);

