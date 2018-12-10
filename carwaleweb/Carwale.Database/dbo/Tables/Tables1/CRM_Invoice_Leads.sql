CREATE TABLE [dbo].[CRM_Invoice_Leads] (
    [id]           INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarBasicdata] BIGINT        NULL,
    [InvoiceId]    NUMERIC (18)  CONSTRAINT [DF_CRM_Invoice_Leads_InvoiceId] DEFAULT ((-1)) NULL,
    [IsAccepted]   BIT           NULL,
    [Reason]       VARCHAR (300) NULL,
    [AcceptedOn]   DATETIME      NULL,
    [CreatedOn]    DATETIME      CONSTRAINT [DF_CRM_Invoice_Leads_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedOn]    DATETIME      NULL,
    [UpdatedBy]    NUMERIC (18)  NULL,
    [RejectedBy]   NUMERIC (18)  NULL,
    [RejectedOn]   DATETIME      NULL,
    CONSTRAINT [PK_CRM_Invoice_Leads] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

