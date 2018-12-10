CREATE TABLE [dbo].[TC_Dealer_Branch] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]   NUMERIC (18)  NOT NULL,
    [BranchName] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_TC_Dealer_Branch] PRIMARY KEY CLUSTERED ([Id] ASC)
);

