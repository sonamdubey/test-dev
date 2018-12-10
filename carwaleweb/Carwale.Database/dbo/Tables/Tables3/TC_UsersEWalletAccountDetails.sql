CREATE TABLE [dbo].[TC_UsersEWalletAccountDetails] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [TC_EWalletsId] SMALLINT      NULL,
    [DealerId]      INT           NULL,
    [Tc_UsersId]    INT           NULL,
    [EmailId]       VARCHAR (100) NULL,
    [MobileNo]      VARCHAR (15)  NULL,
    [EntryDate]     DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

