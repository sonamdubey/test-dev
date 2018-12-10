CREATE TABLE [dbo].[Dealer_BuyerFollowup] (
    [InquiryId]      NUMERIC (18)   NOT NULL,
    [CustomerId]     NUMERIC (18)   NOT NULL,
    [NextCallDate]   DATETIME       NOT NULL,
    [Comment]        NVARCHAR (500) NULL,
    [IsCallFinished] BIT            NULL,
    [UpdatedOn]      DATETIME       CONSTRAINT [DF_Dealer_BuyerFollowup_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]      NUMERIC (18)   NOT NULL,
    CONSTRAINT [PK_Dealer_BuyerFollowup] PRIMARY KEY CLUSTERED ([InquiryId] ASC)
);

