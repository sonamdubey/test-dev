CREATE TABLE [dbo].[DCRM_ActionLog] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]   NUMERIC (18) NULL,
    [InquiryId]  NUMERIC (18) NULL,
    [CustomerId] NUMERIC (18) NULL,
    [ActionId]   INT          NOT NULL,
    [LogDate]    DATETIME     CONSTRAINT [DF_DCRM_ActionLog_LogDate] DEFAULT (getdate()) NOT NULL,
    [LogBy]      NUMERIC (18) NULL,
    CONSTRAINT [PK_DCRM_ActionLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

