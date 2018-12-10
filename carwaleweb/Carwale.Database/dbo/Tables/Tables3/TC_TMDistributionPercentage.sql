CREATE TABLE [dbo].[TC_TMDistributionPercentage] (
    [TC_TMDistributionPercentageId]    INT        IDENTITY (1, 1) NOT NULL,
    [DealerId]                         INT        NULL,
    [CarVersionId]                     INT        NULL,
    [Month]                            TINYINT    NULL,
    [Year]                             SMALLINT   NULL,
    [TargetPercentage]                 FLOAT (53) NULL,
    [TC_TMDistributionPatternMasterId] INT        NULL,
    CONSTRAINT [PK_TC_TMDistributionPercentageId] PRIMARY KEY NONCLUSTERED ([TC_TMDistributionPercentageId] ASC)
);

