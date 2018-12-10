CREATE TABLE [dbo].[TC_OprUsersLog] (
    [OprUserId] INT      NOT NULL,
    [DealerId]  INT      NOT NULL,
    [LoginTime] DATETIME CONSTRAINT [DF_TC_OprUserLog_LoginTime] DEFAULT (getdate()) NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'opr user logined from opr and try to access TC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_OprUsersLog', @level2type = N'COLUMN', @level2name = N'OprUserId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Logined user belonged dealer id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_OprUsersLog', @level2type = N'COLUMN', @level2name = N'DealerId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'the time user logined. default date time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_OprUsersLog', @level2type = N'COLUMN', @level2name = N'LoginTime';

