CREATE TABLE [dbo].[CW_NewCarROI] (
    [Id]                NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CW_CarSegmentId]   NUMERIC (18) NULL,
    [CW_CityCategoryId] INT          NULL,
    [Tenor]             INT          NULL,
    [ROI]               FLOAT (53)   NULL,
    [IsActive]          BIT          CONSTRAINT [DF_CW_NewCarROI_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedBy]         INT          NULL,
    [UpdatedOn]         DATETIME     NULL,
    [EntryDate]         DATETIME     DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CW_NewCarROI] PRIMARY KEY CLUSTERED ([Id] ASC)
);

