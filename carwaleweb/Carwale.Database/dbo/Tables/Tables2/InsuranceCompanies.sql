CREATE TABLE [dbo].[InsuranceCompanies] (
    [ID]       INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_InsuranceCompanies_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_InsuranceCompanies] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

