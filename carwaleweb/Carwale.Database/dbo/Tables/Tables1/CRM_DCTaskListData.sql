CREATE TABLE [dbo].[CRM_DCTaskListData] (
    [Id]              INT IDENTITY (1, 1) NOT NULL,
    [DealerId]        INT NULL,
    [TotalRequest]    INT NULL,
    [PQPending]       INT NULL,
    [TDPending]       INT NULL,
    [BookingPending]  INT NULL,
    [ActiveFBPending] INT NULL,
    [InvoicePending]  INT NULL,
    [ApprovalPending] INT NULL
);

