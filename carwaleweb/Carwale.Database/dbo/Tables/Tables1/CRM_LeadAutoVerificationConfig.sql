CREATE TABLE [dbo].[CRM_LeadAutoVerificationConfig] (
    [Id]            NUMERIC (18) IDENTITY (0, 1) NOT NULL,
    [Day]           VARCHAR (15) NOT NULL,
    [StartTime]     TINYINT      NULL,
    [EndTime]       TINYINT      NULL,
    [LastUpdatedOn] DATETIME     NULL,
    [LastUpdatedBy] VARCHAR (50) NULL
);

