CREATE TABLE [dbo].[ConsumerCreditPointsBkp31072014] (
    [ID]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ConsumerType] SMALLINT     NOT NULL,
    [ConsumerId]   NUMERIC (18) NOT NULL,
    [Points]       NUMERIC (18) NOT NULL,
    [ExpiryDate]   DATETIME     NOT NULL,
    [PackageType]  SMALLINT     NOT NULL
);

