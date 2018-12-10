CREATE TABLE [dbo].[CRM_SkodaAllocatePerson] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [CBDId]              BIGINT        NULL,
    [TokenId]            VARCHAR (50)  NULL,
    [LeadId]             BIGINT        NULL,
    [ErrorCode]          VARCHAR (50)  NULL,
    [ErrorMessage]       VARCHAR (500) NULL,
    [LeadStatus]         VARCHAR (50)  NULL,
    [CreatedBy]          INT           NULL,
    [CreatedOn]          DATETIME      CONSTRAINT [DF_CRM_SkodaAllocatePerson_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedOn]          DATETIME      NULL,
    [IncomingXMLListing] VARCHAR (MAX) NULL,
    [OutgoingXMLListing] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_CRM_SkodaAllocatePerson] PRIMARY KEY CLUSTERED ([Id] ASC)
);

