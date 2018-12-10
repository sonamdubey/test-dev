CREATE TABLE [dbo].[ICB_CarVersionsTemp] (
    [ID]           NUMERIC (18) NOT NULL,
    [CarVersionId] NUMERIC (18) NULL,
    [IsActive]     BIT          NULL,
    [CreatedOn]    DATETIME     NULL,
    [CreatedBy]    NUMERIC (18) NULL,
    CONSTRAINT [PK_ICB_CarVersionsTemp] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

