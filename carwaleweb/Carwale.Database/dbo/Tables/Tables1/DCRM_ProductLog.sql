CREATE TABLE [dbo].[DCRM_ProductLog] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [DealerID]         NUMERIC (18)   NOT NULL,
    [SalesDealerID]    NUMERIC (18)   NOT NULL,
    [OldPackage]       INT            NOT NULL,
    [NewPackage]       INT            NOT NULL,
    [OldClosingAmount] NUMERIC (18)   NOT NULL,
    [NewClosingAmount] NUMERIC (18)   NOT NULL,
    [OldClosingDate]   DATETIME       NOT NULL,
    [NewClosingDate]   DATETIME       NOT NULL,
    [OldDuration]      INT            NOT NULL,
    [NewDuration]      INT            NOT NULL,
    [UpdatedBy]        INT            NOT NULL,
    [UpdatedOn]        DATETIME       NOT NULL,
    [SalesMeetingId]   NUMERIC (18)   NOT NULL,
    [NewComments]      VARCHAR (1000) NULL,
    [OldComments]      VARCHAR (1000) NULL,
    [OldContractId]    INT            NULL,
    [NewContractId]    INT            NULL
);

