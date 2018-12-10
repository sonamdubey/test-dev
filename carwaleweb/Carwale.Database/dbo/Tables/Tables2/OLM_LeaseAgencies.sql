CREATE TABLE [dbo].[OLM_LeaseAgencies] (
    [Id]      NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Company] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_OLM_LeaseAgencies] PRIMARY KEY CLUSTERED ([Id] ASC)
);

