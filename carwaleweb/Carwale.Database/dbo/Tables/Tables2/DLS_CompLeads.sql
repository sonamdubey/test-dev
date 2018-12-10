CREATE TABLE [dbo].[DLS_CompLeads] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [QuoteId]    NUMERIC (18) NOT NULL,
    [CustomerId] NUMERIC (18) NOT NULL,
    [DealerId]   NUMERIC (18) NOT NULL,
    [VersionId]  NUMERIC (18) NOT NULL,
    [IsVerified] BIT          NULL,
    [LeadDate]   DATETIME     CONSTRAINT [DF_DLS_CompLeads_LeadDate] DEFAULT (getdate()) NOT NULL,
    [UpdateDate] DATETIME     NULL,
    [UpdatedBy]  NUMERIC (18) NULL,
    [IsCalled]   BIT          CONSTRAINT [DF_DLS_CompLeads_IsCalled] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DLS_CompLeads] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

