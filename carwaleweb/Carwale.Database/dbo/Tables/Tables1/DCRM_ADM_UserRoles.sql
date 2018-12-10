CREATE TABLE [dbo].[DCRM_ADM_UserRoles] (
    [UserId]    NUMERIC (18) NOT NULL,
    [RoleId]    INT          NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_DCRM_ADM_UserRoles_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedOn] DATETIME     NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DCRM_ADM_UserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC)
);

