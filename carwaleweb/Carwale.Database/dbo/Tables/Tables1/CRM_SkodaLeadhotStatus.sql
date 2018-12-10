CREATE TABLE [dbo].[CRM_SkodaLeadhotStatus] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [LeadId]             NUMERIC (18)  NOT NULL,
    [CBDId]              NUMERIC (18)  NULL,
    [TokenId]            VARCHAR (50)  NOT NULL,
    [ErrorCode]          VARCHAR (50)  NULL,
    [ErrorMessage]       VARCHAR (500) NULL,
    [LeadStatus]         VARCHAR (50)  NULL,
    [CreateDate]         DATETIME      CONSTRAINT [DF_CRM_SkodaLeadhotStatus_CreateDate] DEFAULT (getdate()) NOT NULL,
    [RepushId]           NUMERIC (18)  NULL,
    [UpdatedOn]          DATETIME      NULL,
    [IncomingXMLListing] VARCHAR (MAX) NULL,
    [OutgoingXMLListing] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_CRM_SkodaLeadhotStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

