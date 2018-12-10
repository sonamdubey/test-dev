CREATE TABLE [dbo].[PqAdLeadOwner] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [LeadId]  INT NULL,
    [OwnerId] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

