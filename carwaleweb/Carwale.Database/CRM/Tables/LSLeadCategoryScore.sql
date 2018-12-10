CREATE TABLE [CRM].[LSLeadCategoryScore] (
    [LeadCategoryScoreId] INT          IDENTITY (1, 1) NOT NULL,
    [LeadId]              NUMERIC (18) NULL,
    [CBDId]               NUMERIC (18) NOT NULL,
    [PQ_Id]               NUMERIC (18) NULL,
    [CategoryId]          INT          NOT NULL,
    [LeadScore]           FLOAT (53)   NOT NULL,
    [CreatedOn]           DATETIME     CONSTRAINT [DF_LSLeadCategoryScore1_CreatedOn] DEFAULT (getdate()) NOT NULL
);

