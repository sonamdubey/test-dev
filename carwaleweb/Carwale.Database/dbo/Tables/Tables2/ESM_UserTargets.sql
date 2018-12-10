CREATE TABLE [dbo].[ESM_UserTargets] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [UserId]        INT          NULL,
    [FinancialYear] VARCHAR (50) NULL,
    [Quarter]       TINYINT      NULL,
    [Target]        NUMERIC (18) NULL,
    [UpdatedBy]     INT          NULL,
    [UpdatedOn]     DATETIME     NULL,
    [IsActive]      BIT          CONSTRAINT [DF_ESM_UserTargets_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ESM_UserTargets] PRIMARY KEY CLUSTERED ([Id] ASC)
);

