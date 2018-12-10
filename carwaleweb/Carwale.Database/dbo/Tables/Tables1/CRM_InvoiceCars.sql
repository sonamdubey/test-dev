CREATE TABLE [dbo].[CRM_InvoiceCars] (
    [InvoiceCarID] NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [InvID]        NUMERIC (18)  NULL,
    [CBDID]        NUMERIC (18)  NULL,
    [IsAccepted]   BIT           NULL,
    [Reason]       VARCHAR (500) NULL,
    [RejectedBy]   NUMERIC (18)  NULL,
    CONSTRAINT [PK_CRM_InvoiceCars] PRIMARY KEY CLUSTERED ([InvoiceCarID] ASC) WITH (FILLFACTOR = 90)
);

