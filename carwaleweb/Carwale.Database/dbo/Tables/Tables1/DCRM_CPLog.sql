CREATE TABLE [dbo].[DCRM_CPLog] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [SalesDealerId]  NUMERIC (18) NULL,
    [DealerId]       INT          NOT NULL,
    [OldValue]       INT          NOT NULL,
    [NewValue]       INT          NOT NULL,
    [UpdatedOn]      DATETIME     NULL,
    [UpdatedBy]      INT          NULL,
    [SalesMeetingId] NUMERIC (18) NULL,
    CONSTRAINT [PK_DCRM_CPLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

