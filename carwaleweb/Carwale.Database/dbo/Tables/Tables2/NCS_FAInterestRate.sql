CREATE TABLE [dbo].[NCS_FAInterestRate] (
    [ID]            NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ModelId]       NUMERIC (18)    NOT NULL,
    [FAID]          NUMERIC (18)    NOT NULL,
    [StartTenure]   INT             NOT NULL,
    [EndTenure]     INT             NOT NULL,
    [InterestRate]  DECIMAL (18, 2) NOT NULL,
    [FinCommission] DECIMAL (18, 2) NOT NULL,
    [CWCommission]  DECIMAL (18, 2) NOT NULL,
    [Waiver]        DECIMAL (18, 2) NOT NULL,
    [Tag]           VARCHAR (100)   NULL,
    [LastUpdated]   DATETIME        NOT NULL,
    CONSTRAINT [PK_NCS_FAInterestRate] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

