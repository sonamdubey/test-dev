CREATE TABLE [dbo].[TC_Roles] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [RoleName]         VARCHAR (50)  NOT NULL,
    [RoleDescription]  VARCHAR (100) NULL,
    [TaskSet]          VARCHAR (200) NULL,
    [RoleCreationDate] DATETIME      NOT NULL,
    [BranchId]         INT           NULL,
    [IsActive]         BIT           CONSTRAINT [DF_TC_Roles_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]        DATETIME      CONSTRAINT [DF__TC_Roles__EntryD__32D11227] DEFAULT (getdate()) NULL,
    [ModifiedBy]       INT           NULL,
    [ModifiedDate]     DATETIME      NULL,
    CONSTRAINT [PK_TC_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

