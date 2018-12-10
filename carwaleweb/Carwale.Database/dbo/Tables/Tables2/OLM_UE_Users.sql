CREATE TABLE [dbo].[OLM_UE_Users] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [FullName]    VARCHAR (100) NOT NULL,
    [Mobile]      VARCHAR (15)  NOT NULL,
    [Email]       VARCHAR (50)  NOT NULL,
    [DateOfBirth] DATE          NULL,
    [Profession]  VARCHAR (50)  NULL,
    [TwitterId]   VARCHAR (50)  NULL,
    [FacebookId]  VARCHAR (50)  NULL,
    [CreatedOn]   DATETIME      CONSTRAINT [DF_OLM_UE_Users_CreatedOn] DEFAULT (getdate()) NULL,
    [Source]      SMALLINT      NULL,
    [IsWinner]    BIT           NULL,
    CONSTRAINT [PK_OLM_UE_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

