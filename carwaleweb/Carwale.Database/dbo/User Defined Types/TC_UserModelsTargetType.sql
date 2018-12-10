CREATE TYPE [dbo].[TC_UserModelsTargetType] AS TABLE (
    [TC_UserModelsTargetId] INT      NULL,
    [TC_UsersId]            INT      NULL,
    [CarModelId]            INT      NULL,
    [Month]                 TINYINT  NULL,
    [Year]                  SMALLINT NULL,
    [Target]                INT      NULL,
    [CreatedBy]             INT      NULL,
    [TC_TargetTypeId]       TINYINT  NULL);

