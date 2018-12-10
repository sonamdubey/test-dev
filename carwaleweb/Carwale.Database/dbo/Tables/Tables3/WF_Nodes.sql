CREATE TABLE [dbo].[WF_Nodes] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NodeCode]        VARCHAR (50)  NOT NULL,
    [NodeName]        VARCHAR (150) NOT NULL,
    [ParentNode]      VARCHAR (150) NULL,
    [Ancestors]       VARCHAR (150) NULL,
    [Depth]           INT           NOT NULL,
    [IsLeaf]          BIT           NOT NULL,
    [BudgetType]      SMALLINT      NULL,
    [ActualType]      SMALLINT      NULL,
    [IsActive]        BIT           CONSTRAINT [DF_WF_Nodes_IsActive] DEFAULT ((1)) NOT NULL,
    [Type]            SMALLINT      NULL,
    [CalculationType] SMALLINT      CONSTRAINT [DF_WF_Nodes_CalculationType] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_WF_Nodes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0-None,1-Amount, 2-Count', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WF_Nodes', @level2type = N'COLUMN', @level2name = N'Type';

