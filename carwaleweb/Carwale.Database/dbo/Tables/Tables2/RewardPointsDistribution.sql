CREATE TABLE [dbo].[RewardPointsDistribution] (
    [Id]                 NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PointsPerReview]    INT          NOT NULL,
    [PointsPerAnswer]    INT          NOT NULL,
    [PointsPerForumPost] INT          NOT NULL,
    [ReviewBonus]        INT          NOT NULL,
    [AnswerBonus]        INT          NOT NULL,
    [ForumBonus]         INT          NOT NULL,
    [PointsPerPhoto]     INT          NULL,
    CONSTRAINT [PK_RewardPointsDistribution] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

