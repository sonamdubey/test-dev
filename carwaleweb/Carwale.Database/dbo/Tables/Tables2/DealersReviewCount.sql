CREATE TABLE [dbo].[DealersReviewCount] (
    [DealerId]             NUMERIC (18)    NOT NULL,
    [ReviewRate]           DECIMAL (10, 2) CONSTRAINT [DF_DealersReviewCount_ReviewRate] DEFAULT (0) NOT NULL,
    [ReviewCount]          INT             CONSTRAINT [DF_DealersReviewCount_ReviewCount] DEFAULT (0) NOT NULL,
    [AvgFinancePlans]      DECIMAL (10, 2) CONSTRAINT [DF_DealersReviewCount_AvgFInancePlans] DEFAULT (0) NOT NULL,
    [AvgServiceAndSupport] DECIMAL (10, 2) CONSTRAINT [DF_DealersReviewCount_AvgServiceAndSupport] DEFAULT (0) NOT NULL,
    [AvgStaffCourtesy]     DECIMAL (10, 2) CONSTRAINT [DF_DealersReviewCount_AvgStaffCourtesy] DEFAULT (0) NOT NULL,
    [AvgTimeliness]        DECIMAL (10, 2) CONSTRAINT [DF_DealersReviewCount_AvgTimeliness] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_DealersReviewCount] PRIMARY KEY CLUSTERED ([DealerId] ASC) WITH (FILLFACTOR = 90)
);

