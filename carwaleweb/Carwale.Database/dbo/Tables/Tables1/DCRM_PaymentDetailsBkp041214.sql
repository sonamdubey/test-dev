CREATE TABLE [dbo].[DCRM_PaymentDetailsBkp041214] (
    [ID]                     INT           IDENTITY (1, 1) NOT NULL,
    [Mode]                   INT           NOT NULL,
    [Amount]                 INT           NULL,
    [CheckOrDDNumber]        VARCHAR (50)  NULL,
    [BankName]               VARCHAR (100) NULL,
    [PaymentDate]            DATETIME      NOT NULL,
    [AddedBy]                INT           NOT NULL,
    [AddedOn]                DATETIME      NOT NULL,
    [SalesDealerID]          INT           NULL,
    [IsApproved]             BIT           NULL,
    [ApprovedBy]             INT           NULL,
    [ApprovedOn]             DATETIME      NULL,
    [DealerPackageFeatureId] INT           NULL
);

