CREATE TABLE [dbo].[OLM_UE_UserReviews] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [UserId]     BIGINT        NOT NULL,
    [Review]     VARCHAR (MAX) NOT NULL,
    [IsApproved] BIT           CONSTRAINT [DF_OLM_UE_UserReviews_IsApproved] DEFAULT ((0)) NOT NULL,
    [CreatedOn]  DATETIME      CONSTRAINT [DF_OLM_UE_UserReviews_CreatedOn] DEFAULT (getdate()) NOT NULL
);

