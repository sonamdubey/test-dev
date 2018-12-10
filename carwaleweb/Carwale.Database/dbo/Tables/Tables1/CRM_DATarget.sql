CREATE TABLE [dbo].[CRM_DATarget] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [GroupType] INT          NULL,
    [Date]      DATETIME     NULL,
    [CreatedBy] NUMERIC (18) NULL,
    [CreatedOn] DATETIME     NULL,
    [DA_Target] NUMERIC (18) NULL,
    CONSTRAINT [PK_CRM_DATarget] PRIMARY KEY CLUSTERED ([Id] ASC)
);

