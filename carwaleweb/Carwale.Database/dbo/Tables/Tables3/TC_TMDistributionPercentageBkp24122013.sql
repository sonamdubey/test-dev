CREATE TABLE [dbo].[TC_TMDistributionPercentageBkp24122013] (
    [TC_TMDistributionPercentageId]    INT        IDENTITY (1, 1) NOT NULL,
    [DealerId]                         INT        NULL,
    [CarVersionId]                     INT        NULL,
    [Month]                            TINYINT    NULL,
    [Year]                             SMALLINT   NULL,
    [TargetPercentage]                 FLOAT (53) NULL,
    [TC_TMDistributionPatternMasterId] INT        NULL
);

