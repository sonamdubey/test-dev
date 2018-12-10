CREATE TABLE [dbo].[TC_UserModelsTarget] (
    [TC_UserModelsTargetId] INT      IDENTITY (1, 1) NOT NULL,
    [TC_UsersId]            INT      NULL,
    [CarModelId]            INT      NULL,
    [Month]                 TINYINT  NULL,
    [Year]                  SMALLINT NULL,
    [Target]                INT      NULL,
    [CreatedBy]             INT      NULL,
    [IsDeleted]             BIT      CONSTRAINT [DF_TC_UserModelsTarget_IsDeleted] DEFAULT ((0)) NULL,
    [TC_TargetTypeId]       SMALLINT NULL,
    PRIMARY KEY CLUSTERED ([TC_UserModelsTargetId] ASC)
);

