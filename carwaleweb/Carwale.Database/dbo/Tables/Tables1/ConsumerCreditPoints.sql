CREATE TABLE [dbo].[ConsumerCreditPoints] (
    [ID]                NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ConsumerType]      SMALLINT     NOT NULL,
    [ConsumerId]        NUMERIC (18) NOT NULL,
    [Points]            NUMERIC (18) CONSTRAINT [DF_CreditPoints_Points] DEFAULT (0) NOT NULL,
    [ExpiryDate]        DATETIME     NOT NULL,
    [PackageType]       SMALLINT     CONSTRAINT [DF_ConsumerCreditPoints_PomitType] DEFAULT (1) NOT NULL,
    [CustomerPackageId] INT          NULL,
    [Amount]            INT          NULL,
    [CarGroupType]      VARCHAR (80) NULL,
    [TopUpCarGroupType] VARCHAR (80) NULL,
    [ContractId]        INT          NULL,
    CONSTRAINT [PK_CreditPoints] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_ConsumerCreditPoints__ConsumerType__ConsumerId__PackageType]
    ON [dbo].[ConsumerCreditPoints]([ConsumerType] ASC, [ConsumerId] ASC, [PackageType] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_ConsumerCreditPoints__ConsumerType__PackageType__ExpiryDate]
    ON [dbo].[ConsumerCreditPoints]([ConsumerType] ASC, [PackageType] ASC, [ExpiryDate] ASC)
    INCLUDE([ConsumerId]);

