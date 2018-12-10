CREATE TABLE [dbo].[OprUserPasswordRequest] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [LoginId]      VARCHAR (100) NOT NULL,
    [IPAddress]    VARCHAR (150) NOT NULL,
    [RequestDate]  DATETIME      NOT NULL,
    [RequestCount] INT           NOT NULL,
    CONSTRAINT [PK_OprUserPasswordRequest] PRIMARY KEY CLUSTERED ([Id] ASC)
);

