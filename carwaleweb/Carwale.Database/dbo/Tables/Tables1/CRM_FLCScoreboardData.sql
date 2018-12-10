CREATE TABLE [dbo].[CRM_FLCScoreboardData] (
    [MakeId]      INT          NULL,
    [MakeName]    VARCHAR (50) NULL,
    [LeadCount]   NUMERIC (18) NULL,
    [LeadTarget]  INT          NULL,
    [ProcessType] SMALLINT     NOT NULL,
    [UserId]      INT          NULL,
    [UserName]    VARCHAR (50) NULL
);

