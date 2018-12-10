CREATE TABLE [dbo].[DiscardLeads] (
    [Id]               INT          IDENTITY (1, 1) NOT NULL,
    [PqdealerAdLeadId] NUMERIC (20) NULL,
    [UpdatedBy]        INT          NULL,
    [UpdatedOn]        DATETIME     NULL
);

