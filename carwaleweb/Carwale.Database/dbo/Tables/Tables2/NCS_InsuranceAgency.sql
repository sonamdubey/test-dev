CREATE TABLE [dbo].[NCS_InsuranceAgency] (
    [ID]       NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]     VARCHAR (100) NOT NULL,
    [IsActive] BIT           CONSTRAINT [DF_NCS_InsuranceAgency_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_NCS_InsuranceAgency] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

