CREATE TABLE [dbo].[DCRM_UserTargets] (
    [Id]         NUMERIC (18) NOT NULL,
    [UserId]     INT          NOT NULL,
    [Month]      VARCHAR (50) NOT NULL,
    [MonthId]    SMALLINT     NOT NULL,
    [RegionId]   SMALLINT     NOT NULL,
    [StateId]    INT          NULL,
    [TargetType] TINYINT      NOT NULL,
    [Target]     INT          NOT NULL,
    [Year]       INT          NULL
);

