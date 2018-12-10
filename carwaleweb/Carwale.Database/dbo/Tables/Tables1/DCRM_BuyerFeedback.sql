CREATE TABLE [dbo].[DCRM_BuyerFeedback] (
    [Id]                    NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ContactStatus]         SMALLINT       NOT NULL,
    [ReceivedDealerCall]    BIT            NULL,
    [DealStatus]            SMALLINT       NULL,
    [BuyerStatus]           SMALLINT       NULL,
    [BuyerCWServiceComment] VARCHAR (1000) NULL,
    [CalledDealer]          BIT            NULL,
    [VisitedDealer]         BIT            NULL,
    [VisitDealStatus]       SMALLINT       NULL,
    [VisitBuyerStatus]      SMALLINT       NULL,
    [VisitComment]          VARCHAR (1000) NULL,
    [NoVisitComment]        VARCHAR (1000) NULL,
    [BuyerCWComment]        VARCHAR (1000) NULL,
    [FeedbackBy]            INT            NOT NULL,
    [FeedbackDate]          DATETIME       CONSTRAINT [DF_DCRM_BuyerFeedback_FeedbackDate] DEFAULT (getdate()) NOT NULL,
    [InquiryId]             INT            NOT NULL,
    CONSTRAINT [PK_DCRM_BuyerFeedback] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Contacted, 2-Ringing, 3-Not Available, 4-Invalid Number, 5-Engaged', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_BuyerFeedback', @level2type = N'COLUMN', @level2name = N'ContactStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Yes, 0-No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_BuyerFeedback', @level2type = N'COLUMN', @level2name = N'ReceivedDealerCall';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Still Looking, 2-Postponed, 3-Not Interested any more, 4-Buying from same dealer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_BuyerFeedback', @level2type = N'COLUMN', @level2name = N'DealStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Genuine, 2-Researching, 3-Not Serious', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_BuyerFeedback', @level2type = N'COLUMN', @level2name = N'BuyerStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Yes, 0-No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_BuyerFeedback', @level2type = N'COLUMN', @level2name = N'CalledDealer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Yes, 0-No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_BuyerFeedback', @level2type = N'COLUMN', @level2name = N'VisitedDealer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Still Looking, 2-Postponed, 3-Not Interested any more, 4-Buying from same dealer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_BuyerFeedback', @level2type = N'COLUMN', @level2name = N'VisitDealStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Genuine, 2-Researching, 3-Not Serious', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_BuyerFeedback', @level2type = N'COLUMN', @level2name = N'VisitBuyerStatus';

