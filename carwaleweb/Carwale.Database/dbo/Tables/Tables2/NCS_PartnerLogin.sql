CREATE TABLE [dbo].[NCS_PartnerLogin] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LoginId]   VARCHAR (50)  NULL,
    [Password]  VARCHAR (20)  NULL,
    [LastLogin] DATETIME      NULL,
    [IsActive]  BIT           CONSTRAINT [DF_NCS_PartnerLogin_IsActive] DEFAULT ((1)) NULL,
    [MakeId]    NUMERIC (18)  NULL,
    [Name]      VARCHAR (100) NULL,
    CONSTRAINT [PK_NCS_PartnerLogin] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

