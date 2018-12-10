CREATE TABLE [CRM].[LSLeadScore] (
    [LeadId]    NUMERIC (18) NOT NULL,
    [CreatedOn] DATETIME     CONSTRAINT [DF_LSLeadScore_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn] DATETIME     NULL,
    [LeadScore] FLOAT (53)   NOT NULL,
    CONSTRAINT [PK_LSLeadScore] PRIMARY KEY CLUSTERED ([LeadId] ASC)
);

