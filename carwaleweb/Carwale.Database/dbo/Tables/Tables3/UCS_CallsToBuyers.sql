CREATE TABLE [dbo].[UCS_CallsToBuyers] (
    [ID]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ProfileId]    VARCHAR (50) NULL,
    [CustomerId]   NUMERIC (18) NULL,
    [IsInterested] BIT          NULL,
    [ExcecutiveId] NUMERIC (18) NULL,
    [EnteredOn]    DATETIME     NULL,
    CONSTRAINT [PK_UCS_CallsToBuyers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

