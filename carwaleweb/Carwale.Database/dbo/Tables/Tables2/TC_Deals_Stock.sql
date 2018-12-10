CREATE TABLE [dbo].[TC_Deals_Stock] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [BranchId]        INT           NOT NULL,
    [CarVersionId]    INT           NOT NULL,
    [MakeYear]        DATE          NOT NULL,
    [VersionColorId]  INT           NOT NULL,
    [InteriorColor]   VARCHAR (50)  NULL,
    [EnteredOn]       DATETIME      NOT NULL,
    [EnteredBy]       INT           NOT NULL,
    [LastUpdatedOn]   DATETIME      NULL,
    [LastUpdatedBy]   INT           NULL,
    [Offers]          VARCHAR (500) NULL,
    [IsApproved]      BIT           NULL,
    [TermsConditions] VARCHAR (500) NULL,
    [PriceUpdated]    BIT           CONSTRAINT [DF_tc_deals_priceupdated] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TC_Deals_Stock] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Deals_Stock_BranchId]
    ON [dbo].[TC_Deals_Stock]([BranchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Deals_Stock_VersionColorId]
    ON [dbo].[TC_Deals_Stock]([VersionColorId] ASC);

