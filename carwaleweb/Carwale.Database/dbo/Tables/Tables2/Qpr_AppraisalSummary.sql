CREATE TABLE [dbo].[Qpr_AppraisalSummary] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [UserId]              INT NOT NULL,
    [Qpr_SelfRatingId]    INT NULL,
    [Qpr_ManagerRatingId] INT NULL,
    [Qpr_FinalRatingId]   INT NULL,
    [IsCompleted]         BIT NULL,
    CONSTRAINT [PK_Qpr_AppraisalSummary] PRIMARY KEY CLUSTERED ([Id] ASC)
);

