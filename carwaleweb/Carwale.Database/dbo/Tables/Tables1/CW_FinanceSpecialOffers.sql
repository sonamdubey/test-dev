CREATE TABLE [dbo].[CW_FinanceSpecialOffers] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [CW_CompanyCategoryId] INT            NULL,
    [ROI]                  DECIMAL (5, 2) NULL,
    [MinLoanAmount]        INT            NULL,
    [MaxLoanAmount]        INT            NULL,
    [IsCampaignActive]     BIT            NULL,
    [StartDate]            DATETIME       NULL,
    [EndDate]              DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

