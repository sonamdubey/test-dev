CREATE TABLE [dbo].[WF_BudgetValues] (
    [Id]            NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NodeId]        NUMERIC (18)    NOT NULL,
    [BudgetOfDate]  DATETIME        NOT NULL,
    [BudgetOnDate]  DATETIME        NOT NULL,
    [Value]         NUMERIC (18, 2) NOT NULL,
    [BudgetType]    SMALLINT        NOT NULL,
    [CreatedBy]     NUMERIC (18)    NULL,
    [UpdatedBy]     NUMERIC (18)    NULL,
    [LastUpdatedOn] DATETIME        CONSTRAINT [DF_WF_BudgetValues_LastUpdatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_WF_BudgetValues] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

