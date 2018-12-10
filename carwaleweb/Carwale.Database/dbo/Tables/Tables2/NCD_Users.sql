CREATE TABLE [dbo].[NCD_Users] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]     NUMERIC (18)  NOT NULL,
    [RoleId]       INT           NULL,
    [UserName]     VARCHAR (50)  NOT NULL,
    [Email]        VARCHAR (100) NOT NULL,
    [Password]     VARCHAR (20)  NOT NULL,
    [Mobile]       VARCHAR (200) NOT NULL,
    [EntryDate]    DATETIME      NOT NULL,
    [DOB]          DATE          NULL,
    [DOJ]          DATE          NULL,
    [Sex]          VARCHAR (6)   NULL,
    [Address]      VARCHAR (200) NULL,
    [IsActive]     BIT           CONSTRAINT [DF_NCD_Users_IsActive] DEFAULT ((1)) NOT NULL,
    [IsHeadBranch] BIT           CONSTRAINT [DF_NCD_Users_IsHeadBranch] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_NCD_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ__NCD_Users__A9D1053425B3E0CF] UNIQUE NONCLUSTERED ([Email] ASC)
);

