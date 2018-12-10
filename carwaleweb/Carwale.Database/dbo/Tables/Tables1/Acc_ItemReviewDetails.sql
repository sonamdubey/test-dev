CREATE TABLE [dbo].[Acc_ItemReviewDetails] (
    [Id]                NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ItemReviewId]      NUMERIC (18) NOT NULL,
    [RatingParameterId] NUMERIC (18) NOT NULL,
    [RatingValue]       DECIMAL (18) NOT NULL,
    CONSTRAINT [PK_Acc_ItemReviewDetails] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

