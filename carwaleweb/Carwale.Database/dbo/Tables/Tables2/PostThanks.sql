CREATE TABLE [dbo].[PostThanks] (
    [ID]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CustomerID]      NUMERIC (18) NULL,
    [PostID]          NUMERIC (18) NULL,
    [CreatedDateTime] DATETIME     NULL,
    CONSTRAINT [PK_PostThanks] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

