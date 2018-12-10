CREATE TABLE [dbo].[TC_MessageBoardDetail] (
    [TC_MessageBoardDetailId]       INT           IDENTITY (1, 1) NOT NULL,
    [TC_SpecialUsersId]             INT           NULL,
    [Subject]                       VARCHAR (150) NULL,
    [Message]                       VARCHAR (MAX) NULL,
    [TC_UserTypesForMessageAlertId] INT           NULL,
    [MessageStartDate]              DATE          NULL,
    [MessageEndDate]                DATE          NULL,
    [CreatedOn]                     DATETIME      CONSTRAINT [DF_TC_MessageBoardDetail_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_TC_MessageBoardDetailId] PRIMARY KEY NONCLUSTERED ([TC_MessageBoardDetailId] ASC)
);

