CREATE TABLE [dbo].[TC_FeedbackCalling_DealerMapping] (
    [TC_FeedbackCalling_DealerMappingId] INT      IDENTITY (1, 1) NOT NULL,
    [UserId]                             INT      NOT NULL,
    [DealerId]                           INT      NOT NULL,
    [EntryDate]                          DATETIME NULL,
    [IsActive]                           BIT      NULL
);

