CREATE TABLE [dbo].[FollowupStatus] (
    [ID]       SMALLINT     NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_FollowupStatus_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_FollowupStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

