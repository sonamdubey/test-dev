CREATE TABLE [dbo].[TC_PurchaseInquiries_11092011] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [BranchId]        NUMERIC (18)  NOT NULL,
    [OldCustomerId]   NUMERIC (18)  NOT NULL,
    [StockId]         NUMERIC (18)  NULL,
    [Comments]        VARCHAR (500) NULL,
    [InterestedIn]    VARCHAR (500) NULL,
    [InquiryStatusId] INT           NOT NULL,
    [SourceId]        INT           NOT NULL,
    [FollowUp]        DATETIME      NULL,
    [RequestDateTime] DATETIME      NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [FollowupComment] VARCHAR (200) NULL,
    [IsActionTaken]   BIT           NOT NULL,
    [CustomerId]      NUMERIC (18)  NULL
);

