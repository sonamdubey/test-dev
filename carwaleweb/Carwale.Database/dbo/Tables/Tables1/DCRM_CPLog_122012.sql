CREATE TABLE [dbo].[DCRM_CPLog_122012] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [SalesDealerId] NUMERIC (18) NULL,
    [DealerId]      INT          NOT NULL,
    [OldValue]      INT          NOT NULL,
    [NewValue]      INT          NOT NULL,
    [UpdatedOn]     DATETIME     NULL,
    [UpdatedBy]     INT          NULL
);

