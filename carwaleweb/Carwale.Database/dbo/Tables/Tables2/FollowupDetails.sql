CREATE TABLE [dbo].[FollowupDetails] (
    [ID]                  NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FollowupId]          NUMERIC (18)   NOT NULL,
    [FollowupDescription] VARCHAR (1500) NULL,
    [FollowedById]        NUMERIC (18)   NOT NULL,
    [FollowupDate]        DATETIME       NOT NULL,
    [NextFollowupDate]    DATETIME       NOT NULL,
    [ForServiceProvider]  BIT            CONSTRAINT [DF_FollowupDetails_ForServiceProvider] DEFAULT (1) NOT NULL,
    [StatusId]            NUMERIC (18)   CONSTRAINT [DF_FollowupDetails_StatusId] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_FollowupDetails] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

