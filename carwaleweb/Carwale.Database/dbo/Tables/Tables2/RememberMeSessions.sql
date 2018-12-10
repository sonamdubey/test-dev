CREATE TABLE [dbo].[RememberMeSessions] (
    [CustomerId]   NUMERIC (18)  NOT NULL,
    [Identifier]   VARCHAR (30)  NOT NULL,
    [AccessToken]  VARCHAR (30)  NOT NULL,
    [DateCreated]  DATETIME      CONSTRAINT [DF_RememberMeSessions_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]  DATETIME      CONSTRAINT [DF_RememberMeSessions_DateUpdated] DEFAULT (getdate()) NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_RememberMeSessions_IsActive] DEFAULT ((1)) NOT NULL,
    [IPAddress]    VARCHAR (40)  NOT NULL,
    [UserAgent]    VARCHAR (MAX) NOT NULL,
    [SessionCount] NUMERIC (18)  CONSTRAINT [DF_RememberMeSessions_SessionCount] DEFAULT ((1)) NOT NULL,
    [IsHacked]     BIT           CONSTRAINT [DF_RememberMeSessions_IsHacked] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RememberMeSessions] PRIMARY KEY CLUSTERED ([CustomerId] ASC, [Identifier] ASC, [AccessToken] ASC)
);

