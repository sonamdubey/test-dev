CREATE TABLE [dbo].[CRM_MobDealers] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId] NUMERIC (18) NOT NULL,
    [LoginId]  VARCHAR (50) NOT NULL,
    [Password] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CRM_MobDealers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

