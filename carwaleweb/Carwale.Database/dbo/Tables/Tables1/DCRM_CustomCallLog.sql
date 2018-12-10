CREATE TABLE [dbo].[DCRM_CustomCallLog] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ProspectId]    NUMERIC (18) NOT NULL,
    [ContactNumber] VARCHAR (20) NOT NULL,
    [SystemType]    SMALLINT     NOT NULL,
    [CallId]        NUMERIC (18) NULL,
    [CreatedOn]     DATETIME     CONSTRAINT [DF_DCRM_CustomCallLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DCRM_CustomCallLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-DealerClassified, 2-Individual Classified, 3-NCS', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_CustomCallLog', @level2type = N'COLUMN', @level2name = N'SystemType';

