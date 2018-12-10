CREATE TABLE [dbo].[CW_CarModelDetails] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CarModelId]      NUMERIC (18) NOT NULL,
    [CW_CarSegmentId] NUMERIC (18) NOT NULL,
    [CW_CarTierId]    NUMERIC (18) NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_CW_CarModelDetails_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedBy]       INT          NULL,
    [UpdatedOn]       DATETIME     NULL,
    [EntryDate]       DATETIME     DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CW_CarModelDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

