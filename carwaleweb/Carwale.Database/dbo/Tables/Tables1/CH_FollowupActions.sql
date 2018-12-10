CREATE TABLE [dbo].[CH_FollowupActions] (
    [Id]             INT          NOT NULL,
    [TBCTypeId]      INT          NOT NULL,
    [CallTypeId]     INT          NOT NULL,
    [FollowupAction] VARCHAR (50) NOT NULL,
    [isTimeBased]    BIT          NOT NULL,
    [isActive]       BIT          NOT NULL,
    [isConnected]    BIT          CONSTRAINT [DF_CH_FollowupActions_isConnected] DEFAULT ((1)) NULL,
    [IsConverted]    BIT          CONSTRAINT [DF_CH_FollowupActions_IsConverted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Ch_Test] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

