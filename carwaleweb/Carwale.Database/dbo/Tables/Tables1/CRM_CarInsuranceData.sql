CREATE TABLE [dbo].[CRM_CarInsuranceData] (
    [Id]                     NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarBasicDataId]         NUMERIC (18)   NOT NULL,
    [InsAgencyId]            NUMERIC (18)   NOT NULL,
    [IsInsCoverLetterIssued] BIT            NULL,
    [InsCoverLetterNumber]   VARCHAR (50)   NULL,
    [IsInsPaymentCollected]  BIT            NULL,
    [IsInsPaymentRealised]   BIT            NULL,
    [InsStartDate]           DATETIME       NULL,
    [InsComments]            VARCHAR (1000) NULL,
    [CreatedOn]              DATETIME       NOT NULL,
    [UpdatedOn]              DATETIME       NOT NULL,
    [UpdatedBy]              NUMERIC (18)   NOT NULL,
    CONSTRAINT [PK_CRM_CarInsuranceData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

