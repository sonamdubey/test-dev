CREATE TABLE [dbo].[oprAssignSaleTarget] (
    [ID]          NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ExecutiveID] NUMERIC (18) NOT NULL,
    [BudgetMonth] NUMERIC (18) NOT NULL,
    [BudgetYear]  NUMERIC (18) NOT NULL,
    [Target]      NUMERIC (18) NOT NULL,
    [EntryDate]   DATETIME     NOT NULL,
    CONSTRAINT [PK_oprAssignSaleTarget] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_oprAssignSaleTarget]
    ON [dbo].[oprAssignSaleTarget]([ExecutiveID] ASC, [BudgetMonth] ASC, [BudgetYear] ASC) WITH (FILLFACTOR = 90);

