CREATE TABLE [dbo].[PM_Conversations] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Subject]       VARCHAR (100) NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_PM_Conversations_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [IsDraft]       BIT           CONSTRAINT [DF_PM_Conversations_IsDraft] DEFAULT ((0)) NOT NULL,
    [CreatedBy]     NUMERIC (18)  NOT NULL,
    [NoOfMessages]  NUMERIC (18)  CONSTRAINT [DF_PM_Conversations_NoOfMessages_1] DEFAULT ((0)) NOT NULL,
    [LastUpdatedBy] NUMERIC (18)  NULL,
    [LastUpdatedOn] DATETIME      NULL,
    CONSTRAINT [PK_PM_Conversations] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

