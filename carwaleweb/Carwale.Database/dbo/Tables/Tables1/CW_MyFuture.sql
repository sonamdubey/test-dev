CREATE TABLE [dbo].[CW_MyFuture] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (100) CONSTRAINT [DF_CW_MyFuture_Name] DEFAULT (getdate()) NOT NULL,
    [Comment]   VARCHAR (MAX) NOT NULL,
    [Amount]    NUMERIC (18)  NOT NULL,
    [MailDate]  DATETIME      CONSTRAINT [DF_CW_MyFuture_MailDate] DEFAULT (getdate()) NOT NULL,
    [IPAddress] VARCHAR (50)  NULL,
    CONSTRAINT [PK_CW_MyFuture] PRIMARY KEY CLUSTERED ([Id] ASC)
);

