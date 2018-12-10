CREATE TABLE [dbo].[CRM_DFakeCustomers] (
    [id]     NUMERIC (18) NOT NULL,
    [mobile] VARCHAR (50) NOT NULL,
    [isfake] BIT          NOT NULL,
    CONSTRAINT [PK_CRM_DFakeCustomers] PRIMARY KEY CLUSTERED ([id] ASC)
);

