CREATE TABLE [dbo].[PQ_Users] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BankId]        SMALLINT      NULL,
    [UserName]      VARCHAR (100) NULL,
    [LoginId]       VARCHAR (50)  NULL,
    [LoginPassword] VARCHAR (50)  NULL,
    [UserType]      SMALLINT      NULL,
    [CityId]        NUMERIC (18)  NULL,
    [Status]        BIT           CONSTRAINT [DF_PQ_Users_Status] DEFAULT ((1)) NOT NULL,
    [LastLoginDate] DATETIME      NULL,
    CONSTRAINT [PK_PQ_Users] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-ICICIBank', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PQ_Users', @level2type = N'COLUMN', @level2name = N'BankId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Admin, 2-User', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PQ_Users', @level2type = N'COLUMN', @level2name = N'UserType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Active, 2-InActive', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PQ_Users', @level2type = N'COLUMN', @level2name = N'Status';

