CREATE TABLE [dbo].[TC_TMTargetScenarioArchive] (
    [TC_TMTargetScenarioDetailId]      INT        IDENTITY (1, 1) NOT NULL,
    [DealerId]                         INT        NULL,
    [CarVersionId]                     INT        NULL,
    [Month]                            TINYINT    NULL,
    [Year]                             SMALLINT   NULL,
    [Target]                           FLOAT (53) NULL,
    [TC_TMDistributionPatternMasterId] INT        NULL,
    [TC_SpecialUsersId]                INT        NULL,
    [CreatedOn]                        DATETIME   CONSTRAINT [DF_TC_TMTargetScenarioArchive_CreatedOn] DEFAULT (getdate()) NULL
);

