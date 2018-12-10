CREATE TABLE [dbo].[ESM_ClientTargets] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Type]          SMALLINT     NULL,
    [OrgId]         NUMERIC (18) NULL,
    [FinancialYear] VARCHAR (50) NULL,
    [Target]        NUMERIC (18) NULL,
    [UpdatedBy]     INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

