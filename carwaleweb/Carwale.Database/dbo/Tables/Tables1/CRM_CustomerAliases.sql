CREATE TABLE [dbo].[CRM_CustomerAliases] (
    [ID]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CRM_CustomerId] NUMERIC (18)  NOT NULL,
    [FirstName]      VARCHAR (200) NOT NULL,
    [LastName]       VARCHAR (100) NOT NULL,
    [Email]          VARCHAR (100) NOT NULL,
    [MobileNo]       VARCHAR (20)  NOT NULL,
    [LandlineNo]     VARCHAR (20)  NOT NULL,
    [CityId]         NUMERIC (18)  NOT NULL,
    [CWCustomerId]   NUMERIC (18)  NOT NULL,
    [Source]         VARCHAR (100) NOT NULL,
    [CreatedOn]      DATETIME      NOT NULL,
    CONSTRAINT [PK_CRM_CustomerAliases] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CustomerAliases_CRM_CustomerId]
    ON [dbo].[CRM_CustomerAliases]([CRM_CustomerId] ASC);

