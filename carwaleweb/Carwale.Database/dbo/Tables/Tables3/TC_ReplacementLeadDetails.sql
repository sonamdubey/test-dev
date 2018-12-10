CREATE TABLE [dbo].[TC_ReplacementLeadDetails] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [ContractId]    INT           NULL,
    [InquiryLeadId] INT           NULL,
    [Status]        INT           DEFAULT ((0)) NULL,
    [Comment]       VARCHAR (MAX) NULL,
    [TagDate]       DATETIME      NULL,
    [TaggedBy]      INT           NULL,
    CONSTRAINT [PK_TC_ReplacementLeadDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

