CREATE TABLE [dbo].[OLM_UE_ReviewsVotes] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Email]    VARCHAR (50) NOT NULL,
    [ReviewId] BIGINT       NOT NULL
);

