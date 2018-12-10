CREATE TABLE [dbo].[TC_LeadType] (
    [TC_LeadTypeId] TINYINT      IDENTITY (1, 1) NOT NULL,
    [LeadType]      VARCHAR (20) NULL,
    CONSTRAINT [PK_TC_LeadType] PRIMARY KEY CLUSTERED ([TC_LeadTypeId] ASC)
);

