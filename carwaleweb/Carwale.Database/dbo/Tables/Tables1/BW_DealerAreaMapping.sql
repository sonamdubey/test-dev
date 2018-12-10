CREATE TABLE [dbo].[BW_DealerAreaMapping] (
    [Id]       BIGINT IDENTITY (1, 1) NOT NULL,
    [DealerId] INT    NULL,
    [AreaId]   INT    NULL,
    [IsActive] BIT    CONSTRAINT [DF_BW_DealerAreaMapping_IsActive] DEFAULT ((1)) NULL
);

