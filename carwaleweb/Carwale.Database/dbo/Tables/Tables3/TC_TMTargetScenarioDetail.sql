CREATE TABLE [dbo].[TC_TMTargetScenarioDetail] (
    [TC_TMTargetScenarioDetailId]      INT        IDENTITY (1, 1) NOT NULL,
    [DealerId]                         INT        NULL,
    [CarVersionId]                     INT        NULL,
    [Month]                            TINYINT    NULL,
    [Year]                             SMALLINT   NULL,
    [Target]                           FLOAT (53) NULL,
    [TC_TMDistributionPatternMasterId] INT        NULL,
    CONSTRAINT [PK_TC_TMLegacyDetailId] PRIMARY KEY NONCLUSTERED ([TC_TMTargetScenarioDetailId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_TMTargetScenarioDetail_TC_TMDistributionPatternMasterId]
    ON [dbo].[TC_TMTargetScenarioDetail]([TC_TMDistributionPatternMasterId] ASC);

