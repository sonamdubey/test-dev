CREATE TABLE [dbo].[PwdResetAT] (
    [CustomerId]    NUMERIC (18) NOT NULL,
    [AccessToken]   VARCHAR (50) NOT NULL,
    [StartDateTime] DATETIME     CONSTRAINT [DF_PwdResetAT_StartDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_PwdResetAT] PRIMARY KEY CLUSTERED ([AccessToken] ASC)
);

