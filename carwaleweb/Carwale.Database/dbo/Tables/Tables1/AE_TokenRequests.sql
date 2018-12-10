CREATE TABLE [dbo].[AE_TokenRequests] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BidderId]   NUMERIC (18) NOT NULL,
    [NoOfTokens] INT          NOT NULL,
    [EntryDate]  DATETIME     NOT NULL,
    [Status]     BIT          CONSTRAINT [DF_AE_BiddingRequests_Status] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AE_BiddingRequests] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Activated, 2-Pending', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AE_TokenRequests', @level2type = N'COLUMN', @level2name = N'Status';

