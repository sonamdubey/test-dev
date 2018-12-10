CREATE TABLE [dbo].[DealerSiteCreditPoints] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [DealerId]    INT          NOT NULL,
    [Points]      NUMERIC (18) NOT NULL,
    [ExpiryDate]  DATETIME     NOT NULL,
    [PackageType] SMALLINT     NOT NULL,
    CONSTRAINT [PK_DealerSiteCreditPoints] PRIMARY KEY CLUSTERED ([Id] ASC)
);

