CREATE TABLE [dbo].[CRM_VerificationLog] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [LeadId]         NUMERIC (18)  NOT NULL,
    [CBDId]          NUMERIC (18)  NOT NULL,
    [VersionId]      NUMERIC (18)  NOT NULL,
    [IsPDRequired]   BIT           NULL,
    [IsTDRequired]   BIT           NULL,
    [IsPQRequired]   BIT           NULL,
    [DealerId]       NUMERIC (18)  NOT NULL,
    [UpdatedBy]      NUMERIC (18)  NOT NULL,
    [UpdatedOn]      DATETIME      NULL,
    [Comments]       VARCHAR (500) NULL,
    [IsConCall]      BIT           NULL,
    [isIdProcessed]  BIT           CONSTRAINT [DF_CRM_VerificationLog_isIdProcessed] DEFAULT ((0)) NULL,
    [TDDate]         DATETIME      NULL,
    [Source]         TINYINT       NULL,
    [StepsProcessed] INT           CONSTRAINT [DF_CRM_VerificationLog_StepsProcessed] DEFAULT ((-1)) NULL,
    [IsFinance]      BIT           NULL,
    [IsOffer]        BIT           NULL,
    [IsUrgent]       BIT           NULL,
    CONSTRAINT [PK_CRM_VerificationLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_VerificationLog__CBDId]
    ON [dbo].[CRM_VerificationLog]([CBDId] ASC);

