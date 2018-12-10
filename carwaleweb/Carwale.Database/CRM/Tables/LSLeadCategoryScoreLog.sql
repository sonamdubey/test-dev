CREATE TABLE [CRM].[LSLeadCategoryScoreLog] (
    [LeadCategoryScoreLogId] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CategoryId]             INT          NOT NULL,
    [SubCategoryId]          INT          NOT NULL,
    [LeadId]                 NUMERIC (18) NOT NULL,
    [LeadScore]              FLOAT (53)   NOT NULL,
    [CreatedOn]              DATETIME     NOT NULL,
    CONSTRAINT [PK_LSLeadCategoryScoreLog] PRIMARY KEY CLUSTERED ([LeadCategoryScoreLogId] ASC)
);

