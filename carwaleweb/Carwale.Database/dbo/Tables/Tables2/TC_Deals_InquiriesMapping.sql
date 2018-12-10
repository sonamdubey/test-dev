CREATE TABLE [dbo].[TC_Deals_InquiriesMapping] (
    [ID]                INT      IDENTITY (1, 1) NOT NULL,
    [CwDealerInqId]     INT      NOT NULL,
    [ActualDealerInqId] INT      NOT NULL,
    [CreatedOn]         DATETIME DEFAULT (getdate()) NULL,
    [CreatedBy]         INT      NULL,
    [IsPaid]            BIT      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

