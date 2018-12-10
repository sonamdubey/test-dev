CREATE TABLE [dbo].[TC_Deals_Stock_17022016] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
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

