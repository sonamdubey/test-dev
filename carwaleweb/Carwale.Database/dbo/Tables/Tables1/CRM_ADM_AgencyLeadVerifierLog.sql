CREATE TABLE [dbo].[CRM_ADM_AgencyLeadVerifierLog] (
    [UserId]    NUMERIC (18) NOT NULL,
    [Type]      SMALLINT     NOT NULL,
    [CreatedOn] DATETIME     NOT NULL,
    [CreatedBy] NUMERIC (18) NOT NULL,
    [DeletedOn] DATETIME     CONSTRAINT [DF_CRM_ADM_AgencyLeadVerifierLog_DeletedOn] DEFAULT (getdate()) NOT NULL,
    [DeletedBy] NUMERIC (18) NULL
);

