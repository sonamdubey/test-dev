CREATE TABLE [dbo].[NCD_Roles] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [RoleName]         VARCHAR (50)  NOT NULL,
    [RoleDescription]  VARCHAR (100) NULL,
    [TaskSet]          VARCHAR (200) NOT NULL,
    [RoleCreationDate] DATETIME      NOT NULL,
    [DealerId]         INT           NULL,
    [IsActive]         BIT           NOT NULL,
    CONSTRAINT [PK_NCD_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

