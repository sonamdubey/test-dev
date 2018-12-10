CREATE TABLE [dbo].[CL_DroppedCalls] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CalledNumber]    VARCHAR (50)  NOT NULL,
    [StartTime]       DATETIME      NOT NULL,
    [NodeData]        VARCHAR (250) NULL,
    [BusinessProcess] VARCHAR (50)  NULL,
    [CallType]        SMALLINT      NOT NULL,
    [TypePath]        VARCHAR (250) NULL,
    CONSTRAINT [PK_CL_DroppedCalls] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Abandone Call, 2-Voice Mail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CL_DroppedCalls', @level2type = N'COLUMN', @level2name = N'CallType';

