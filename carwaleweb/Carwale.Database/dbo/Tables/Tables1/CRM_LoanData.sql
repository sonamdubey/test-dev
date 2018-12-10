CREATE TABLE [dbo].[CRM_LoanData] (
    [ID]                      NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]                  NUMERIC (18)    NOT NULL,
    [LoanAmount]              NUMERIC (18)    NULL,
    [LoanTenure]              NUMERIC (18)    NULL,
    [EMI]                     NUMERIC (18)    NULL,
    [InterestRate]            DECIMAL (18, 2) NULL,
    [FinancerId]              NUMERIC (18)    NOT NULL,
    [IsLoanInterested]        BIT             NULL,
    [IsCaseRegistered]        BIT             NULL,
    [LoanApprovalStatusId]    SMALLINT        NULL,
    [APCNumber]               VARCHAR (50)    NULL,
    [ApprovalRemarks]         VARCHAR (500)   NULL,
    [ApprovedOn]              DATETIME        NULL,
    [DocPickupRequestDate]    DATETIME        NULL,
    [DocPickupDate]           DATETIME        NULL,
    [IsDocCollected]          BIT             NULL,
    [DocStatusId]             SMALLINT        NULL,
    [DocPickupComments]       VARCHAR (1000)  NULL,
    [IsDisbursementCompleted] BIT             NULL,
    [LoanDisbursedBy]         NUMERIC (18)    NULL,
    [DisbursementDate]        DATETIME        NULL,
    [DisbursementRemark]      VARCHAR (1000)  NULL,
    [IsReleaseOrderIssued]    BIT             NULL,
    [CreatedOn]               DATETIME        NOT NULL,
    [UpdatedOn]               DATETIME        NOT NULL,
    [UpdatedBy]               NUMERIC (18)    NOT NULL,
    CONSTRAINT [PK_CRM_LoanData] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_LoanData__LeadId]
    ON [dbo].[CRM_LoanData]([LeadId] ASC);

