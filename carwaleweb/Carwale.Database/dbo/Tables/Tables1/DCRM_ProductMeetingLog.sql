CREATE TABLE [dbo].[DCRM_ProductMeetingLog] (
    [ID]             NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [SalesMeetingId] NUMERIC (18) NOT NULL,
    [SalesDealerId]  NUMERIC (18) NOT NULL
);

