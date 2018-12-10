CREATE TABLE [dbo].[NewCarTestDriveReq] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [QuoteId]       NUMERIC (18)  NOT NULL,
    [Address]       VARCHAR (100) NOT NULL,
    [TestDriveDate] DATETIME      NULL,
    CONSTRAINT [PK_NewCarTestDriveReq] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

