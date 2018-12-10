CREATE TABLE [dbo].[TC_UserPasswordRecoveryTokens] (
    [TC_UserPasswordRecoveryTokensId] INT           IDENTITY (1, 1) NOT NULL,
    [TC_UserId]                       INT           NOT NULL,
    [Token]                           VARCHAR (200) NOT NULL,
    [IsActiveToken]                   BIT           NOT NULL,
    [EntryDate]                       DATETIME      NULL,
    [ExpiryDate]                      DATETIME      NULL,
    [LastUpdated]                     DATETIME      NULL,
    CONSTRAINT [PK_TC_UserPasswordRecoveryTokens] PRIMARY KEY CLUSTERED ([TC_UserPasswordRecoveryTokensId] ASC)
);

