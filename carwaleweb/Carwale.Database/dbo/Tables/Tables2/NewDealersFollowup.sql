CREATE TABLE [dbo].[NewDealersFollowup] (
    [ID]               NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealersId]        NUMERIC (18)  NOT NULL,
    [Comments]         VARCHAR (500) NOT NULL,
    [SubmissionDate]   DATETIME      NOT NULL,
    [NextFollowupDate] DATETIME      NOT NULL,
    [followedById]     NUMERIC (18)  NOT NULL,
    [StatusId]         NUMERIC (18)  NOT NULL,
    CONSTRAINT [PK_NewDealersFollowup] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_NewDealersFollowup_NewDealers] FOREIGN KEY ([DealersId]) REFERENCES [dbo].[NewDealers] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_NewDealersFollowup_OprFollowupStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[OprFollowupStatus] ([ID])
);

