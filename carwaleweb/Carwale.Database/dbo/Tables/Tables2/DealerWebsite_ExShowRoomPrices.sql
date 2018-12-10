CREATE TABLE [dbo].[DealerWebsite_ExShowRoomPrices] (
    [Id]              INT        IDENTITY (100, 1) NOT NULL,
    [DealerId]        INT        NULL,
    [CityId]          INT        NULL,
    [CarVersionId]    INT        NULL,
    [ExShowroomPrice] FLOAT (53) NULL,
    [RTO]             FLOAT (53) NULL,
    [Insurance]       FLOAT (53) NULL,
    [EntryDate]       DATE       CONSTRAINT [DealerWebsite_ExShowRoomPrices_entry_date] DEFAULT (getdate()) NULL,
    [CRTMCharges]     FLOAT (53) NULL,
    [DriveAssure]     FLOAT (53) NULL,
    [WithOctroi]      BIT        NULL,
    CONSTRAINT [PK_DealerWebsite_ExShowRoomPrices] PRIMARY KEY CLUSTERED ([Id] ASC)
);

