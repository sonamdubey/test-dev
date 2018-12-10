CREATE TABLE [dbo].[NCD_HeadBranchDealers] (
    [DealerId]   INT NOT NULL,
    [NCD_UserId] INT CONSTRAINT [DF_Table_1_NCD_HeadBranchDealer] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_NCD_HeadBranchDealers] PRIMARY KEY CLUSTERED ([DealerId] ASC)
);

