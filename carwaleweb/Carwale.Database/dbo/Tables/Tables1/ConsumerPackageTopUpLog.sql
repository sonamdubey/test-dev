CREATE TABLE [dbo].[ConsumerPackageTopUpLog] (
    [Id]                     INT             IDENTITY (1, 1) NOT NULL,
    [ConsumerPackageTopUpId] INT             NOT NULL,
    [DealerId]               INT             NULL,
    [PackageId]              INT             NULL,
    [StartDate]              DATETIME        NULL,
    [ExpiryDate]             DATETIME        NULL,
    [CarGroupType]           VARCHAR (80)    NULL,
    [Volume]                 INT             NULL,
    [Status]                 INT             NULL,
    [TopUpAmount]            NUMERIC (18, 2) NULL,
    [CreditNoteAmount]       NUMERIC (18, 2) NULL,
    [StatusUpdatedDate]      DATETIME        NULL,
    [EntryDate]              DATETIME        CONSTRAINT [DF_ConsumerPackageTopUpLog_EntryDate] DEFAULT (getdate()) NULL,
    [ContractId]             INT             NULL,
    [TotalConsumed]          INT             NULL,
    [PackageContractId]      INT             NULL,
    [ActualValidity]         INT             NULL,
    CONSTRAINT [PK_ConsumerPackageTopUpLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

