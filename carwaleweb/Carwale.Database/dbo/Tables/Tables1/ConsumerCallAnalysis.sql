CREATE TABLE [dbo].[ConsumerCallAnalysis] (
    [Id]                NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BuyerCallDue]      INT          NOT NULL,
    [BuyerCalled]       INT          NOT NULL,
    [BuyerConversion]   INT          NOT NULL,
    [BuyerRevenue]      INT          NOT NULL,
    [SellerCallDue]     INT          NOT NULL,
    [SellerCalled]      INT          NOT NULL,
    [SellerConversion]  INT          NOT NULL,
    [SellerRevenue]     INT          NOT NULL,
    [RenewalCallDue]    INT          NOT NULL,
    [RenewalCalled]     INT          NOT NULL,
    [RenewalConversion] INT          NOT NULL,
    [EntryDate]         DATETIME     NOT NULL,
    CONSTRAINT [PK_ConsumerAnalysisData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

