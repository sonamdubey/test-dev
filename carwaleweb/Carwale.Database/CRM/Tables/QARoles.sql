CREATE TABLE [CRM].[QARoles] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (150) NOT NULL,
    [CRMRoleId] NUMERIC (18)  NOT NULL,
    [CreatedBy] NUMERIC (18)  NOT NULL,
    [CreatedOn] DATETIME      NOT NULL,
    [UpdatedBy] NUMERIC (18)  NOT NULL,
    [UpdatedOn] DATETIME      NOT NULL,
    [IsActive]  BIT           CONSTRAINT [DF_QARoles_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_QARoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

