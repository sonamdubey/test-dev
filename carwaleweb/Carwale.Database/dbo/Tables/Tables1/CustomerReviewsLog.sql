CREATE TABLE [dbo].[CustomerReviewsLog] (
    [ID]              NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ReviewId]        NUMERIC (18) NULL,
    [UpdatedBy]       NUMERIC (18) NULL,
    [UpdatedDateTime] DATETIME     NULL,
    CONSTRAINT [PK_CustomerReviewsLog] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

