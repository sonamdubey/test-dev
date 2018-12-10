CREATE TABLE [CRM].[LSLeadCategoryScoreBkp] (
    [LeadCategoryScoreId] INT          IDENTITY (1, 1) NOT NULL,
    [CategoryId]          INT          NOT NULL,
    [LeadId]              NUMERIC (18) NOT NULL,
    [LeadScore]           FLOAT (53)   NOT NULL,
    [CreatedOn]           DATETIME     CONSTRAINT [DF_LSLeadCategoryScore_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]           DATETIME     NULL,
    CONSTRAINT [PK_LSLeadCategoryScore] PRIMARY KEY CLUSTERED ([LeadCategoryScoreId] ASC)
);

