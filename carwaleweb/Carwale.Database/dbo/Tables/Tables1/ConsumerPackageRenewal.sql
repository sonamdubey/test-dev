CREATE TABLE [dbo].[ConsumerPackageRenewal] (
    [RequestId]   NUMERIC (18) NOT NULL,
    [RenewalDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_ConsumerPackageRenewal] PRIMARY KEY CLUSTERED ([RequestId] ASC) WITH (FILLFACTOR = 90)
);

