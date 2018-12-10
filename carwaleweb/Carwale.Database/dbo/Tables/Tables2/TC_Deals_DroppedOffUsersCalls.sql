CREATE TABLE [dbo].[TC_Deals_DroppedOffUsersCalls] (
    [DealInquiries_Id] INT           NOT NULL,
    [LastCallTime]     DATETIME      NULL,
    [FollowUpTime]     DATETIME      NULL,
    [Comments]         VARCHAR (MAX) NULL,
    [Status]           BIT           NULL,
    [DispositionId]    INT           DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([DealInquiries_Id] ASC)
);

