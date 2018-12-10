CREATE TABLE [dbo].[NCS_IACommission] (
    [ID]            NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [IAID]          NUMERIC (18)    NOT NULL,
    [ModelId]       NUMERIC (18)    NOT NULL,
    [InsCommission] DECIMAL (18, 2) NOT NULL,
    [CWCommission]  DECIMAL (18, 2) NOT NULL,
    [LastUpdated]   DATETIME        NOT NULL,
    CONSTRAINT [PK_INS_IACommission] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

