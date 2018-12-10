CREATE TABLE [dbo].[DCRM_BuyerFeedBackNew] (
    [Id]                         NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ContactStatus]              SMALLINT       NOT NULL,
    [ReceivedDealerCall]         BIT            NULL,
    [ReceivedDealerCallOn]       SMALLINT       NULL,
    [DealerResponseSatisfaction] BIT            NULL,
    [CommentsDealerSatisfaction] VARCHAR (1000) NULL,
    [FoundCarInterestedIn]       BIT            NULL,
    [NeedHelpFromCW]             BIT            NULL,
    [CommentsHelp]               VARCHAR (1000) NULL,
    [DealerId]                   INT            NULL,
    [CustomerId]                 INT            NOT NULL,
    [FeedBackDate]               DATETIME       NOT NULL,
    [FeedBackBy]                 VARCHAR (50)   NOT NULL,
    CONSTRAINT [PK_DCRM_BuyerFeedBackNew] PRIMARY KEY CLUSTERED ([Id] ASC)
);

