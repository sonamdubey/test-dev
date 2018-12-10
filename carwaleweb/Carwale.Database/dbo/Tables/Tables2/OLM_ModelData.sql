CREATE TABLE [dbo].[OLM_ModelData] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [MakeId]      NUMERIC (18) NOT NULL,
    [ModelId]     NUMERIC (18) NOT NULL,
    [MonthVal]    DATETIME     NOT NULL,
    [PageViews]   NUMERIC (18) NULL,
    [PQRequests]  NUMERIC (18) NULL,
    [LastUpdated] DATETIME     CONSTRAINT [DF_OLM_ModelData_LastUpdated] DEFAULT (getdate()) NOT NULL,
    [F1]          SMALLINT     NULL,
    [F2]          SMALLINT     NULL,
    [F3]          SMALLINT     NULL,
    [F4]          SMALLINT     NULL,
    [F5]          SMALLINT     NULL,
    [F6]          SMALLINT     NULL,
    [F7]          SMALLINT     NULL,
    [F8]          SMALLINT     NULL,
    CONSTRAINT [PK_OLM_ModelData] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Running Cost', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLM_ModelData', @level2type = N'COLUMN', @level2name = N'F1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Looks/Style', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLM_ModelData', @level2type = N'COLUMN', @level2name = N'F2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Value For Money', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLM_ModelData', @level2type = N'COLUMN', @level2name = N'F3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Comfort & Space', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLM_ModelData', @level2type = N'COLUMN', @level2name = N'F4';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Performance', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLM_ModelData', @level2type = N'COLUMN', @level2name = N'F5';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Safety', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLM_ModelData', @level2type = N'COLUMN', @level2name = N'F6';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand Value', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLM_ModelData', @level2type = N'COLUMN', @level2name = N'F7';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Features', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLM_ModelData', @level2type = N'COLUMN', @level2name = N'F8';

