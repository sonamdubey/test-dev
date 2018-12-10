CREATE TABLE [dbo].[AffiliateConversion] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SiteCode]       VARCHAR (50) NOT NULL,
    [ConversionId]   NUMERIC (18) NOT NULL,
    [CategoryId]     SMALLINT     NOT NULL,
    [ConversionDate] DATETIME     NOT NULL,
    [IsVerified]     BIT          CONSTRAINT [DF_AffiliateConversion_IsVerified] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_AffiliateConversion] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

