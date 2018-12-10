CREATE TABLE [dbo].[TC_Deals_Stock_24012016] (
    [Id]             INT           NOT NULL,
    [BranchId]       INT           NOT NULL,
    [CarVersionId]   INT           NOT NULL,
    [MakeYear]       DATE          NOT NULL,
    [VersionColorId] INT           NOT NULL,
    [InteriorColor]  VARCHAR (50)  NULL,
    [EnteredOn]      DATETIME      NOT NULL,
    [EnteredBy]      INT           NOT NULL,
    [LastUpdatedOn]  DATETIME      NULL,
    [LastUpdatedBy]  INT           NULL,
    [Offers]         VARCHAR (500) NULL,
    [IsApproved]     BIT           NULL
);

