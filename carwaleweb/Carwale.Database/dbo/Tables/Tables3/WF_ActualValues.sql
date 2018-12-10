CREATE TABLE [dbo].[WF_ActualValues] (
    [Id]            NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NodeId]        NUMERIC (18)    NOT NULL,
    [ValueDate]     DATETIME        NOT NULL,
    [Value]         DECIMAL (18, 2) NOT NULL,
    [ValueType]     SMALLINT        NOT NULL,
    [CreatedBy]     INT             NULL,
    [UpdatedBy]     NUMERIC (18)    NULL,
    [LastUpdatedOn] DATETIME        CONSTRAINT [DF_WF_ActualValues_LastUpdatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_WF_ActualValues] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

