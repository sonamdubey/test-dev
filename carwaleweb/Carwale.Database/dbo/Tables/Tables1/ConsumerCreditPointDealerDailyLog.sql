CREATE TABLE [dbo].[ConsumerCreditPointDealerDailyLog] (
    [AsOnDate]              DATE     NULL,
    [Id]                    INT      NULL,
    [ConsumerType]          SMALLINT NULL,
    [ConsumerId]            INT      NULL,
    [Points]                INT      NULL,
    [ExpiryDate]            DATETIME NULL,
    [PackageType]           SMALLINT NULL,
    [CustomerPackageId]     INT      NULL,
    [IsDealerPackageActive] BIT      NULL
);

