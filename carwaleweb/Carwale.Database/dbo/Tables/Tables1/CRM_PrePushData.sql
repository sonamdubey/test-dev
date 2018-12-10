CREATE TABLE [dbo].[CRM_PrePushData] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerName]       VARCHAR (250) NULL,
    [Mobile]             VARCHAR (100) NULL,
    [EMail]              VARCHAR (100) NULL,
    [State]              VARCHAR (50)  NULL,
    [City]               VARCHAR (50)  NULL,
    [CarName]            VARCHAR (100) NULL,
    [LeadId]             NUMERIC (18)  NULL,
    [TokenId]            VARCHAR (50)  NULL,
    [Status]             VARCHAR (500) NULL,
    [StartDate]          DATETIME      NULL,
    [EndDate]            DATETIME      NULL,
    [IsMailSent]         BIT           CONSTRAINT [DF_CRM_PrePushData_IsMailSent] DEFAULT ((0)) NULL,
    [RepushId]           NUMERIC (18)  NULL,
    [ErrorCode]          VARCHAR (20)  NULL,
    [Result]             VARCHAR (50)  NULL,
    [IncomingXMLListing] VARCHAR (MAX) NULL,
    [OutgoingXMLListing] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_CRM_PrePushData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Idx_CRM_PrePushData_LeadId]
    ON [dbo].[CRM_PrePushData]([LeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_PrePushData_Result]
    ON [dbo].[CRM_PrePushData]([Result] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_PrePushData]
    ON [dbo].[CRM_PrePushData]([LeadId] ASC, [Result] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_PrePushData__RepushId__Result]
    ON [dbo].[CRM_PrePushData]([RepushId] ASC, [Result] ASC);

