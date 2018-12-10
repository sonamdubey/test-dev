CREATE TABLE [dbo].[CAExpertCategories] (
    [CustomerId]      NUMERIC (18) NOT NULL,
    [AskUsCategoryId] INT          NOT NULL,
    CONSTRAINT [PK_CAExpertCategories] PRIMARY KEY CLUSTERED ([CustomerId] ASC, [AskUsCategoryId] ASC) WITH (FILLFACTOR = 90)
);

