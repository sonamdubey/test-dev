CREATE TABLE [dbo].[CRM_MailerRequests] (
    [LeadId]        NUMERIC (18)  NOT NULL,
    [NewName]       VARCHAR (100) NULL,
    [NewMobile]     VARCHAR (20)  NULL,
    [UpdatedOn]     DATETIME      NOT NULL,
    [IsCallback]    BIT           CONSTRAINT [DF_CRM_MailerRequests_IsCallback] DEFAULT ((0)) NOT NULL,
    [IsTestDrive]   BIT           CONSTRAINT [DF_CRM_MailerRequests_IsTestDrive] DEFAULT ((0)) NOT NULL,
    [IsQuotation]   BIT           CONSTRAINT [DF_CRM_MailerRequests_IsQuotation] DEFAULT ((0)) NOT NULL,
    [CityId]        NUMERIC (18)  NULL,
    [DealerId]      NUMERIC (18)  NULL,
    [IsActionTaken] BIT           CONSTRAINT [DF_CRM_MailerRequests_IsActionTaken] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CRM_MailerRequests] PRIMARY KEY CLUSTERED ([LeadId] ASC) WITH (FILLFACTOR = 90)
);

