CREATE TABLE [dbo].[BestDealCarValuations] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarId]     NUMERIC (18) NOT NULL,
    [UserType]  SMALLINT     NOT NULL,
    [Valuation] NUMERIC (18) NOT NULL,
    [Entrydate] DATETIME     NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_BestDealCarValuations_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_BestDealCarValuations] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_BestDealCarValuations__CarId__UserType]
    ON [dbo].[BestDealCarValuations]([CarId] ASC, [UserType] ASC)
    INCLUDE([Valuation]);

