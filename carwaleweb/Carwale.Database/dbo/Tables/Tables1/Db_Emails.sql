CREATE TABLE [dbo].[Db_Emails] (
    [DomainName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Db_Emails] PRIMARY KEY CLUSTERED ([DomainName] ASC) WITH (FILLFACTOR = 90)
);

