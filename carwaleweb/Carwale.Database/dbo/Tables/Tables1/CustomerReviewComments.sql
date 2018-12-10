CREATE TABLE [dbo].[CustomerReviewComments] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ReviewId]      NUMERIC (18)  NOT NULL,
    [CustomerId]    NUMERIC (18)  NOT NULL,
    [Comments]      VARCHAR (500) NOT NULL,
    [PostDateTime]  DATETIME      NOT NULL,
    [IsApproved]    BIT           NOT NULL,
    [ReportedAbuse] BIT           CONSTRAINT [DF_CustomerReviewComments_ReportedAbuse] DEFAULT (0) NULL,
    [IsActive]      BIT           CONSTRAINT [DF_CustomerReviewComments_IsActive] DEFAULT (1) NULL,
    [SourceId]      SMALLINT      CONSTRAINT [DF_CustomerReviewComments_SourceId] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CustomerReviewComments] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CustomerReviewComments__ReviewId__IsActive]
    ON [dbo].[CustomerReviewComments]([ReviewId] ASC, [IsActive] ASC);

