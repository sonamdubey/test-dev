CREATE TABLE [dbo].[SkodaCarSegmentDetails] (
    [CarVersionId] INT     NOT NULL,
    [CarSegmentId] TINYINT NULL,
    PRIMARY KEY CLUSTERED ([CarVersionId] ASC),
    CONSTRAINT [FK_SkodaCarSegmentDetails_SkodaCarSegments] FOREIGN KEY ([CarSegmentId]) REFERENCES [dbo].[SkodaCarSegments] ([CarSegmentId])
);

