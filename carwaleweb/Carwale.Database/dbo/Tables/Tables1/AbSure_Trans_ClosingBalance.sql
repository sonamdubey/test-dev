CREATE TABLE [dbo].[AbSure_Trans_ClosingBalance] (
    [DealerId]        NUMERIC (18) NOT NULL,
    [ClosingBalance]  NUMERIC (18) NOT NULL,
    [DiscountPer]     SMALLINT     NOT NULL,
    [LastUpdatedDate] DATETIME     CONSTRAINT [DF_AbSure_Trans_DealerClosingBalance_LastUpdatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedBy]   INT          NOT NULL,
    CONSTRAINT [PK_AbSure_Trans_DealerClosingBalance] PRIMARY KEY CLUSTERED ([DealerId] ASC)
);

