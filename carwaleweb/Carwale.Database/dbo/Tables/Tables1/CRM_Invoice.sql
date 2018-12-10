CREATE TABLE [dbo].[CRM_Invoice] (
    [InvoiceID]       NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [TempInvNumber]   VARCHAR (50)  NULL,
    [TempInvDate]     DATETIME      NULL,
    [ActualInvNumber] VARCHAR (50)  NULL,
    [ActualInvDate]   DATETIME      NULL,
    [AcceptedOn]      DATETIME      NULL,
    [CreatedOn]       DATETIME      NULL,
    [UpdatedOn]       DATETIME      NULL,
    [UpdatedBy]       NUMERIC (18)  NULL,
    [Comments]        VARCHAR (500) NULL,
    [IsActive]        BIT           CONSTRAINT [DF_CRM_Invoice_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CRM_Invoice] PRIMARY KEY CLUSTERED ([InvoiceID] ASC) WITH (FILLFACTOR = 90)
);

