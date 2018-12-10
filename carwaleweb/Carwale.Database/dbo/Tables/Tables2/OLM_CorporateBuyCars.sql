CREATE TABLE [dbo].[OLM_CorporateBuyCars] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [InquiryId]   NUMERIC (18) NOT NULL,
    [ModelId]     NUMERIC (18) NOT NULL,
    [ModelCounts] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_OLM_CorporateBuyCars] PRIMARY KEY CLUSTERED ([Id] ASC)
);

