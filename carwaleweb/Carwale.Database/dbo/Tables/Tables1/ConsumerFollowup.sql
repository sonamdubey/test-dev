CREATE TABLE [dbo].[ConsumerFollowup] (
    [ID]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ConsumerType]  SMALLINT      NOT NULL,
    [ConsumerId]    NUMERIC (18)  NOT NULL,
    [Comments]      VARCHAR (200) NULL,
    [FollCatId]     SMALLINT      CONSTRAINT [DF_ConsumerFollowup_FollCatId] DEFAULT (1) NOT NULL,
    [EntryDateTime] DATETIME      NOT NULL,
    [FollowupType]  SMALLINT      CONSTRAINT [DF_ConsumerFollowup_FollowupType] DEFAULT (1) NULL,
    [FollowedBy]    NUMERIC (18)  NULL,
    CONSTRAINT [PK_ConsumerType] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

