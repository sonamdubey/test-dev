CREATE TABLE [dbo].[CarValues_31Dec06] (
    [CarVersionId] NUMERIC (18) NOT NULL,
    [CarYear]      INT          NOT NULL,
    [CarValue]     NUMERIC (18) NOT NULL,
    [GuideId]      NUMERIC (18) CONSTRAINT [DF_CarValues_GuideId] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_CarValues] PRIMARY KEY CLUSTERED ([CarVersionId] ASC, [CarYear] ASC, [GuideId] ASC) WITH (FILLFACTOR = 90)
);

