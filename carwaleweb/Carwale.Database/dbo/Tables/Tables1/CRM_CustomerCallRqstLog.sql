CREATE TABLE [dbo].[CRM_CustomerCallRqstLog] (
    [id]                        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [EventRaisedOn]             DATETIME       NOT NULL,
    [CallRequestDate]           DATETIME       NULL,
    [DealerId]                  NUMERIC (18)   NULL,
    [CBDId]                     NUMERIC (18)   NULL,
    [LeadId]                    NCHAR (10)     NULL,
    [EventRaisedBy]             NUMERIC (18)   NULL,
    [EventCompletedOn]          DATETIME       NULL,
    [IsApproved]                BIT            NULL,
    [ApprovedOn]                DATETIME       NULL,
    [ApprovedBy]                NUMERIC (18)   NULL,
    [DealerComment]             VARCHAR (1000) NULL,
    [EventIdByDealer]           INT            NULL,
    [CallCompletedDateByDealer] DATETIME       NULL,
    [EventCompletedBy]          NUMERIC (18)   NULL,
    CONSTRAINT [PK_CRM_CustomerCallRqstLog] PRIMARY KEY CLUSTERED ([id] ASC)
);

