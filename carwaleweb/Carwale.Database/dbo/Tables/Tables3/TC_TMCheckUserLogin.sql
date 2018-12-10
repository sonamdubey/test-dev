CREATE TABLE [dbo].[TC_TMCheckUserLogin] (
    [TC_TMCheckUserLoginId] INT      IDENTITY (1, 1) NOT NULL,
    [TC_SpecialUsersId]     INT      NULL,
    [IsUserLogged]          BIT      NULL,
    [CreatedOn]             DATETIME NULL,
    [LastUpdatedOn]         DATETIME NULL,
    PRIMARY KEY CLUSTERED ([TC_TMCheckUserLoginId] ASC)
);

