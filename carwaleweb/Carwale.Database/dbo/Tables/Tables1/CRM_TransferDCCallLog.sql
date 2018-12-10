CREATE TABLE [dbo].[CRM_TransferDCCallLog] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CallId]       NUMERIC (18) NOT NULL,
    [FromDC]       NUMERIC (18) NOT NULL,
    [ToDC]         NUMERIC (18) NOT NULL,
    [CallerType]   SMALLINT     NOT NULL,
    [TransferDate] DATETIME     NOT NULL,
    [TransferBy]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_TransferDCCall] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 : Dealer Coordinator,2 : Car Consultant', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_TransferDCCallLog', @level2type = N'COLUMN', @level2name = N'CallerType';

