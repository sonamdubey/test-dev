CREATE TABLE [dbo].[History_CRM_CientPendingApprovals] (
    [Id]                  NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ClientId]            NUMERIC (18)   NOT NULL,
    [ClientType]          SMALLINT       NOT NULL,
    [LeadId]              NUMERIC (18)   NOT NULL,
    [CBDId]               NUMERIC (18)   NOT NULL,
    [CurrentEventType]    INT            NOT NULL,
    [ChangedEventType]    INT            NOT NULL,
    [IsApproved]          BIT            NOT NULL,
    [IsValid]             BIT            NULL,
    [CreatedOn]           DATETIME       NOT NULL,
    [UpdatedOn]           DATETIME       NULL,
    [UpdatedBy]           NUMERIC (18)   NULL,
    [UpdatedByDealer]     VARCHAR (50)   NULL,
    [Comments]            VARCHAR (1000) NULL,
    [DateValue]           DATETIME       NULL,
    [IsDCApproved]        BIT            NULL,
    [ChangedSubEventType] INT            NULL,
    [SubDispositionId]    INT            NULL
);

