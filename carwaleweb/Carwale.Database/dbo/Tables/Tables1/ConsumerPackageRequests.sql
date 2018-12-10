CREATE TABLE [dbo].[ConsumerPackageRequests] (
    [Id]                  NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ConsumerType]        SMALLINT        NOT NULL,
    [ConsumerId]          NUMERIC (18)    NOT NULL,
    [PackageId]           INT             NOT NULL,
    [ActualValidity]      INT             NOT NULL,
    [ActualInquiryPoints] INT             NOT NULL,
    [ActualAmount]        NUMERIC (18)    NOT NULL,
    [PaymentModeId]       INT             NOT NULL,
    [Chk_DD_Number]       VARCHAR (50)    NULL,
    [Chk_DD_Date]         DATETIME        NULL,
    [EntryDate]           DATETIME        NOT NULL,
    [isApproved]          BIT             CONSTRAINT [DF_PendingPackageRequests_isApproved] DEFAULT (0) NOT NULL,
    [isActive]            BIT             CONSTRAINT [DF_PendingPakageRequest_isActive] DEFAULT (1) NOT NULL,
    [EnteredBy]           SMALLINT        NOT NULL,
    [EnteredById]         NUMERIC (18)    NOT NULL,
    [BankName]            VARCHAR (150)   NULL,
    [ReceivedPayment]     BIT             CONSTRAINT [DF_ConsumerPackageRequests_ReceivedPayment] DEFAULT (0) NOT NULL,
    [ItemId]              NUMERIC (18)    CONSTRAINT [DF_ConsumerPackageRequests_ItemId] DEFAULT ((-1)) NOT NULL,
    [Comments]            NVARCHAR (500)  NULL,
    [ApprovedBy]          NUMERIC (18)    NULL,
    [ApprovalDate]        DATETIME        NULL,
    [ContractId]          INT             NULL,
    [CarGroupType]        INT             NULL,
    [CreditNoteAmount]    NUMERIC (18, 2) NULL,
    [TotalConsumed]       INT             NULL,
    CONSTRAINT [PK_PendingPakageRequest] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Idx_ConsumerPackageRequests_ConTyp_ConId_IA]
    ON [dbo].[ConsumerPackageRequests]([ConsumerType] ASC, [ConsumerId] ASC, [isApproved] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_ConsumerPackageRequests__ConsumerType__PackageId__ItemId]
    ON [dbo].[ConsumerPackageRequests]([ConsumerType] ASC, [PackageId] ASC);

