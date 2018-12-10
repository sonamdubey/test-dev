CREATE TABLE [dbo].[ConsumerPackageTopUp] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [DealerId]          INT             NULL,
    [PackageId]         INT             NULL,
    [StartDate]         DATETIME        NULL,
    [ExpiryDate]        DATETIME        NULL,
    [CarGroupType]      VARCHAR (80)    NULL,
    [Volume]            INT             NULL,
    [Status]            INT             NULL,
    [TopUpAmount]       NUMERIC (18, 2) NULL,
    [CreditNoteAmount]  NUMERIC (18, 2) NULL,
    [StatusUpdatedDate] DATETIME        NULL,
    [EntryDate]         DATETIME        NULL,
    [ContractId]        INT             NULL,
    [TotalConsumed]     INT             NULL,
    [ActualValidity]    INT             NULL,
    CONSTRAINT [PK__Consumer__3214EC077CDF15CA] PRIMARY KEY CLUSTERED ([Id] ASC)
);

