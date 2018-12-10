CREATE TABLE [dbo].[DCRM_FieldExecutivesTarget] (
    [Id]             INT         IDENTITY (1, 1) NOT NULL,
    [OprUserId]      INT         NOT NULL,
    [BusinessUnitId] SMALLINT    NOT NULL,
    [MetricId]       INT         NOT NULL,
    [UserTarget]     INT         NOT NULL,
    [TargetMonth]    SMALLINT    NOT NULL,
    [TargetYear]     VARCHAR (4) NOT NULL,
    [AddedBy]        INT         NOT NULL,
    [AddedOn]        DATETIME    NOT NULL,
    [UpdatedBy]      INT         NULL,
    [UpdatedOn]      DATETIME    NULL,
    [QuarterId]      SMALLINT    NULL
);

