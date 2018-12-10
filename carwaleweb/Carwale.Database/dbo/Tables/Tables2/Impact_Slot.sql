CREATE TABLE [dbo].[Impact_Slot] (
    [Impact_SlotId]     BIGINT  IDENTITY (1, 1) NOT NULL,
    [MakeId]            INT     NOT NULL,
    [CityId]            INT     NOT NULL,
    [DealerId]          INT     NULL,
    [PackageTypeId]     TINYINT NOT NULL,
    [Impact_CampaignId] BIGINT  NULL,
    [IsActive]          BIT     CONSTRAINT [DF_Impact_Slot_IsActive] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Impact_Slot] PRIMARY KEY CLUSTERED ([Impact_SlotId] ASC)
);

