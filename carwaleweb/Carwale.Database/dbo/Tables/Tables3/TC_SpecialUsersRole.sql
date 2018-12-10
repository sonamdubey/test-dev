CREATE TABLE [dbo].[TC_SpecialUsersRole] (
    [TC_SpecialUsersRoleId]   INT      IDENTITY (1, 1) NOT NULL,
    [TC_SpecialUsersId]       INT      NOT NULL,
    [TC_SpecialRolesMasterId] SMALLINT NOT NULL,
    PRIMARY KEY CLUSTERED ([TC_SpecialUsersRoleId] ASC)
);

