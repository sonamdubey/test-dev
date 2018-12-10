CREATE TABLE [dbo].[UsedSortBucketConfig] (
    [Id]                 INT        IDENTITY (1, 1) NOT NULL,
    [PackageId]          INT        NOT NULL,
    [CarScoreLowerLimit] FLOAT (53) NOT NULL,
    [CarScoreUpperLimit] FLOAT (53) NOT NULL,
    [CustCountThreshold] SMALLINT   NOT NULL,
    [SortBucketUp]       TINYINT    NOT NULL,
    [SortBucketDown]     TINYINT    NULL
);

