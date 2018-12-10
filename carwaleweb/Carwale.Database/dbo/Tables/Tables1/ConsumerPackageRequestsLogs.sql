CREATE TABLE [dbo].[ConsumerPackageRequestsLogs] (
    [Id]                  NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ConsumerPkgReqId]    NUMERIC (18)  NOT NULL,
    [ActualValidity]      INT           NOT NULL,
    [ActualInquiryPoints] INT           NOT NULL,
    [ActualAmount]        NUMERIC (18)  NOT NULL,
    [PaymentModeId]       INT           NOT NULL,
    [Chk_DD_Number]       NUMERIC (18)  NULL,
    [Chk_DD_Date]         DATETIME      NULL,
    [EntryDate]           DATETIME      NOT NULL,
    [EnteredBy]           SMALLINT      NOT NULL,
    [EnteredById]         NUMERIC (18)  NOT NULL,
    [BankName]            VARCHAR (150) NULL,
    [TotalConsumed]       INT           NULL,
    CONSTRAINT [PK_ConsumerPackageRequestsLogs] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

