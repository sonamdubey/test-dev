CREATE TABLE [dbo].[AxisBank_PasswordRecoveryTokens] (
    [Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserId]        BIGINT        NOT NULL,
    [Token]         VARCHAR (200) NOT NULL,
    [IsActiveToken] BIT           CONSTRAINT [DF_AxisBank_PasswordRecoveryTokens_IsActiveToken] DEFAULT ((1)) NOT NULL,
    [EntryDate]     DATETIME      NULL,
    [ExpiryDate]    DATETIME      NULL,
    [LastUpdated]   DATETIME      NULL
);

