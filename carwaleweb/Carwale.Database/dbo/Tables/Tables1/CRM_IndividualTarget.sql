CREATE TABLE [dbo].[CRM_IndividualTarget] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [UserId]    INT      NOT NULL,
    [Brand]     INT      NOT NULL,
    [Type]      INT      NOT NULL,
    [Date]      DATE     NOT NULL,
    [Value]     INT      NOT NULL,
    [CreatedOn] DATETIME CONSTRAINT [DF_CRM_IndividualTarget_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy] INT      NOT NULL,
    [UpdatedOn] DATETIME NULL,
    [UpdatedBy] INT      NULL,
    CONSTRAINT [PK_CRM_IndividualTarget] PRIMARY KEY CLUSTERED ([Id] ASC)
);

