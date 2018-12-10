CREATE TABLE [dbo].[DealerLoginLogs] (
    [ID]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]  NUMERIC (18) NOT NULL,
    [LoginTime] DATETIME     NOT NULL,
    [LoginType] SMALLINT     CONSTRAINT [DF_DealerLoginLogs_LoginType] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DealerLoginLogs] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-DealerLogin, 2-AutomaticLogin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DealerLoginLogs', @level2type = N'COLUMN', @level2name = N'LoginType';

