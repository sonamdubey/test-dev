CREATE TABLE [dbo].[DealerBuyerFBCall] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [DealerId]        INT            DEFAULT ((0)) NOT NULL,
    [BuyerId]         INT            DEFAULT ((0)) NOT NULL,
    [ReceivedCall]    TINYINT        DEFAULT ((0)) NOT NULL,
    [RcvComments]     NVARCHAR (500) DEFAULT ('') NOT NULL,
    [IsHappy]         TINYINT        DEFAULT ((0)) NOT NULL,
    [IsVisited]       TINYINT        DEFAULT ((0)) NOT NULL,
    [VisitComments]   NVARCHAR (500) DEFAULT ('') NOT NULL,
    [DealStatus]      TINYINT        DEFAULT ((0)) NOT NULL,
    [LikeOtherCar]    TINYINT        DEFAULT ((0)) NOT NULL,
    [WhoMetAtDealer]  TINYINT        DEFAULT ((0)) NOT NULL,
    [MeetComments]    NVARCHAR (500) DEFAULT ('') NOT NULL,
    [StillLooking]    TINYINT        DEFAULT ((0)) NOT NULL,
    [WhenVisit]       TINYINT        DEFAULT ((0)) NOT NULL,
    [LookComment]     NVARCHAR (500) DEFAULT ('') NOT NULL,
    [CallSeller]      TINYINT        DEFAULT ((0)) NOT NULL,
    [SellerComments]  NVARCHAR (500) DEFAULT ('') NOT NULL,
    [PurcaheseStatus] TINYINT        DEFAULT ((0)) NOT NULL,
    [MeetSeller]      TINYINT        DEFAULT ((0)) NOT NULL,
    [SellerResponse]  TINYINT        DEFAULT ((0)) NOT NULL,
    [ResponseComment] NVARCHAR (500) DEFAULT ('') NOT NULL,
    [PlanToCall]      TINYINT        DEFAULT ((0)) NOT NULL,
    [PlanComment]     NVARCHAR (500) DEFAULT ('') NOT NULL,
    [BuyerType]       TINYINT        DEFAULT ((0)) NOT NULL,
    [BuyCarRange]     TINYINT        DEFAULT ((0)) NOT NULL,
    [BuyerCity]       SMALLINT       DEFAULT ((0)) NOT NULL,
    [BuyerCWComments] NVARCHAR (500) DEFAULT ('') NOT NULL,
    [ExecutiveId]     INT            NOT NULL,
    [EntryDate]       DATETIME       DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SellerFeedbackCall] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Yes, 2-No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DealerBuyerFBCall', @level2type = N'COLUMN', @level2name = N'ReceivedCall';

