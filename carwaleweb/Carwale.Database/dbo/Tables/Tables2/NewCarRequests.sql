CREATE TABLE [dbo].[NewCarRequests] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]      NUMERIC (18)  NOT NULL,
    [VersionId]     NUMERIC (18)  NOT NULL,
    [CustomerId]    NUMERIC (18)  NOT NULL,
    [RequestType]   SMALLINT      NOT NULL,
    [DateTimeValue] DATETIME      NULL,
    [TDLocation]    VARCHAR (250) NULL,
    [CreatedOn]     DATETIME      CONSTRAINT [DF_NewCarRequests_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_NewCarRequests] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-PQ, 2-TD, 3-Callback, 4-SMSContacts', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NewCarRequests', @level2type = N'COLUMN', @level2name = N'RequestType';

