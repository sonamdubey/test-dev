CREATE TABLE [dbo].[TempCustRegSMS] (
    [CustomerId]    NUMERIC (18) NOT NULL,
    [EntryDateTime] DATETIME     NOT NULL,
    CONSTRAINT [PK_TempCustRegSMS] PRIMARY KEY CLUSTERED ([CustomerId] ASC) WITH (FILLFACTOR = 90)
);

