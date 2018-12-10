CREATE TABLE [dbo].[CRM_LoanCarDetails] (
    [Id]               NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LoanDataId]       NUMERIC (18) NOT NULL,
    [VersionId]        NUMERIC (18) NOT NULL,
    [PQId]             NUMERIC (18) NOT NULL,
    [LoanAmount]       NUMERIC (18) NULL,
    [LoanTenure]       NUMERIC (18) NULL,
    [EMI]              NUMERIC (18) NULL,
    [IsCaseRegistered] BIT          CONSTRAINT [DF_CRM_LoanCarDetails_IsCaseRegistered] DEFAULT ((0)) NULL,
    [CreatedOn]        DATETIME     NOT NULL,
    [UpdatedOn]        DATETIME     NOT NULL,
    [UpdatedBy]        NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_LoanCarDetails] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

