CREATE TABLE [dbo].[CRM_Invoices] (
    [Id]        INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Inv_No]    VARCHAR (50) NULL,
    [Inv_Date]  DATETIME     NULL,
    [BasicData] NUMERIC (18) NULL,
    [CreatedOn] DATETIME     NULL,
    [CreatedBy] NUMERIC (18) NULL,
    [UpdatedOn] DATETIME     NULL,
    [UpdatedBy] NUMERIC (18) NULL,
    [IsActive]  BIT          NULL,
    CONSTRAINT [PK_CRM_Invoices] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

