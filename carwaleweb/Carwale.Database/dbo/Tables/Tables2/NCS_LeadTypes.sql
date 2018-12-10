CREATE TABLE [dbo].[NCS_LeadTypes] (
    [Id]        SMALLINT      NULL,
    [Descr]     VARCHAR (100) NULL,
    [createdon] DATETIME      DEFAULT (getdate()) NULL
);

