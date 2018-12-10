CREATE TABLE [dbo].[ConsumerCreditPointsLogs] (
    [Id]               NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ConsumerId]       NUMERIC (18) NOT NULL,
    [PackageId]        NUMERIC (18) NOT NULL,
    [ConsumerPkgReqId] NUMERIC (18) NOT NULL,
    [SuggestedInquiry] NUMERIC (10) NOT NULL,
    [ActualInquiry]    NUMERIC (10) NOT NULL,
    [SuggestedPrice]   NUMERIC (10) NOT NULL,
    [ActualPrice]      NUMERIC (10) NOT NULL,
    [EntryDate]        DATETIME     NOT NULL,
    [EnteredBy]        SMALLINT     NOT NULL,
    [ConsumerType]     SMALLINT     NOT NULL,
    [EnteredById]      NUMERIC (18) NOT NULL,
    [ContractId]       INT          NULL,
    CONSTRAINT [PK_ConsumerCreditPointsLogs] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

