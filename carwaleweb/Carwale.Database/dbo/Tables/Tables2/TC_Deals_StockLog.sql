CREATE TABLE [dbo].[TC_Deals_StockLog] (
    [TC_Deals_StockId] INT           NOT NULL,
    [BranchId]         INT           NOT NULL,
    [CarVersionId]     INT           NOT NULL,
    [MakeYear]         DATE          NOT NULL,
    [VersionColorId]   INT           NOT NULL,
    [InteriorColor]    VARCHAR (50)  NULL,
    [LastUpdatedOn]    DATETIME      NULL,
    [LastUpdatedBy]    INT           NULL,
    [Offers]           VARCHAR (500) NULL,
    [IsApproved]       TINYINT       NULL,
    [TermsConditions]  VARCHAR (600) NULL
);

