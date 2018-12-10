CREATE TABLE [dbo].[Dealer_ListingFollowUp] (
    [LogID]        NUMERIC (18)   NOT NULL,
    [CustomerID]   NUMERIC (18)   NOT NULL,
    [NextCallDate] DATETIME       NOT NULL,
    [IsConverted]  BIT            NULL,
    [Comments]     NVARCHAR (500) NULL,
    [UpdatedOn]    DATETIME       NULL,
    [UpdatedBy]    NUMERIC (18)   NULL,
    CONSTRAINT [PK_Dealer_ListingFollowUp] PRIMARY KEY CLUSTERED ([LogID] ASC)
);

