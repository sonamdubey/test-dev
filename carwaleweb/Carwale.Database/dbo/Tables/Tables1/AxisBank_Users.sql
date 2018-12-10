CREATE TABLE [dbo].[AxisBank_Users] (
    [UserId]         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [LoginId]        VARCHAR (50)  NOT NULL,
    [FirstName]      VARCHAR (50)  NOT NULL,
    [LastName]       VARCHAR (50)  NOT NULL,
    [Email]          VARCHAR (100) NOT NULL,
    [CreatedOn]      DATETIME      NULL,
    [IsVerified]     BIT           CONSTRAINT [DF_AxisBank_Users_IsVerified] DEFAULT ((0)) NULL,
    [IsAdmin]        BIT           CONSTRAINT [DF_AxisBank_Users_IsAdmin] DEFAULT ((0)) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_AxisBank_Users_IsActive] DEFAULT ((1)) NULL,
    [PasswordSalt]   VARCHAR (10)  NULL,
    [PasswordHash]   VARCHAR (64)  NULL,
    [PasswordExpiry] DATETIME      NULL,
    CONSTRAINT [PK_AxisBank_Users] PRIMARY KEY CLUSTERED ([UserId] ASC) WITH (FILLFACTOR = 90)
);

