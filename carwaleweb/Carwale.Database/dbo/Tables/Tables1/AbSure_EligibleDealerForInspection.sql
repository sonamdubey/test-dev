CREATE TABLE [dbo].[AbSure_EligibleDealerForInspection] (
    [Id]       INT    IDENTITY (1, 1) NOT NULL,
    [DealerId] BIGINT NOT NULL,
    [Isactive] BIT    CONSTRAINT [DF_AbSure_EligibleDealerForInspection_Isactive] DEFAULT ((1)) NOT NULL
);

