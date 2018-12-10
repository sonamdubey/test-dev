CREATE TABLE [dbo].[CRM_CarUSPData] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [VersionId]           INT           NULL,
    [CreatedBy]           INT           NULL,
    [CreatedOn]           DATETIME      CONSTRAINT [DF_CRM_CarUSPData_CreatedOn] DEFAULT (getdate()) NULL,
    [CarUSPParameterId]   INT           NULL,
    [CarUSPParameterData] VARCHAR (MAX) NULL,
    [IsActive]            BIT           NULL,
    [Feature]             VARCHAR (100) NULL,
    [Benefit]             VARCHAR (100) NULL,
    [Advantage]           VARCHAR (100) NULL,
    CONSTRAINT [PK_CRM_CarUSPData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

