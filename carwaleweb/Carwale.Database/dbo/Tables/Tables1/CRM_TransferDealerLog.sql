CREATE TABLE [dbo].[CRM_TransferDealerLog] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]     NUMERIC (18) NOT NULL,
    [FromDC]       NUMERIC (18) NOT NULL,
    [ToDC]         NUMERIC (18) NOT NULL,
    [TransferDate] DATETIME     NOT NULL,
    [TransferBy]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_TransferDealerLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

