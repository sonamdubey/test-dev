CREATE TABLE [dbo].[TC_SpecialRolesMaster] (
    [TC_SpecialRolesMasterId] SMALLINT      IDENTITY (1, 1) NOT NULL,
    [RoleName]                VARCHAR (50)  NOT NULL,
    [RoleDescription]         VARCHAR (200) NULL,
    [IsActive]                BIT           CONSTRAINT [DF_TC_SpecialRolesMaster_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TC_SpecialRolesMaster] PRIMARY KEY CLUSTERED ([TC_SpecialRolesMasterId] ASC)
);

