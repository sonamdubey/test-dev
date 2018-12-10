CREATE TABLE [dbo].[SkodaCarSegments] (
    [CarSegmentId]   TINYINT      IDENTITY (1, 1) NOT NULL,
    [CarSegmentName] VARCHAR (20) NULL,
    CONSTRAINT [PK_SkodaCarSegments] PRIMARY KEY CLUSTERED ([CarSegmentId] ASC)
);

