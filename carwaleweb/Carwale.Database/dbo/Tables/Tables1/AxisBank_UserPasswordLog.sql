CREATE TABLE [dbo].[AxisBank_UserPasswordLog] (
    [ID]             NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [UserId]         NUMERIC (18) NOT NULL,
    [PasswordSalt]   VARCHAR (10) NOT NULL,
    [PasswordHash]   VARCHAR (64) NOT NULL,
    [ChangeDateTime] DATETIME     NULL,
    CONSTRAINT [PK_AxisBank_UserPasswordLog] PRIMARY KEY CLUSTERED ([ID] ASC)
);

