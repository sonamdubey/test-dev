CREATE TABLE [dbo].[CRM_VerificationDumpLog] (
    [Id]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [LeadId]    NUMERIC (18)   NOT NULL,
    [Comment]   NVARCHAR (MAX) NOT NULL,
    [UpdatedBy] NUMERIC (18)   NOT NULL,
    [UpdatedOn] DATETIME       NOT NULL,
    [LeadType]  TINYINT        NOT NULL,
    CONSTRAINT [PK_CRM_VerificationDumpLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

