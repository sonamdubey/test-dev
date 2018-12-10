CREATE TABLE [dbo].[TC_RolesMaster] (
    [TC_RolesMasterId] SMALLINT      IDENTITY (1, 1) NOT NULL,
    [RoleName]         VARCHAR (50)  NOT NULL,
    [RoleDescription]  VARCHAR (200) NULL,
    [IsActive]         BIT           CONSTRAINT [DF_TC_RolesMaster_IsActive] DEFAULT ((1)) NOT NULL,
    [TC_DealerTypeId]  TINYINT       NULL,
    [TC_InquiryTypeId] SMALLINT      NULL,
    [IsVisible]        BIT           CONSTRAINT [DF_TC_RolesMaster_IsVisible] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TC_RolesMaster] PRIMARY KEY CLUSTERED ([TC_RolesMasterId] ASC)
);

