CREATE TABLE [dbo].[NCS_FARates] (
    [ID]            NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FAID]          NUMERIC (18)    NOT NULL,
    [ZoneId]        NUMERIC (18)    NOT NULL,
    [GroupId]       NUMERIC (18)    NOT NULL,
    [StartTenure]   NUMERIC (18)    NOT NULL,
    [EndTenure]     NUMERIC (18)    NOT NULL,
    [IRSalaried]    DECIMAL (18, 2) NOT NULL,
    [IRSelfEmp]     DECIMAL (18, 2) NOT NULL,
    [FinCommission] DECIMAL (18, 2) NOT NULL,
    [CWCommission]  DECIMAL (18, 2) NOT NULL,
    [Waiver]        DECIMAL (18, 2) NOT NULL,
    [LastUpdated]   DATETIME        NOT NULL,
    CONSTRAINT [PK_NCS_FARates] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

