CREATE TABLE [dbo].[WF_NodeUnitValues] (
    [id]        NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NodeId]    NUMERIC (18)    NULL,
    [UnitValue] NUMERIC (18, 2) NULL,
    [CreatedOn] DATETIME        CONSTRAINT [DF_WF_NodeUnitValues_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_WF_NodeUnitValues] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

