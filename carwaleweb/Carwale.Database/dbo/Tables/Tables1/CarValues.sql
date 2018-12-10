CREATE TABLE [dbo].[CarValues] (
    [CarVersionId] NUMERIC (18) NOT NULL,
    [CarYear]      INT          NOT NULL,
    [CarValue]     NUMERIC (18) NOT NULL,
    [GuideId]      NUMERIC (18) CONSTRAINT [DF_CarValuesS2014_GuideId] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CarValuesS2014] PRIMARY KEY CLUSTERED ([CarVersionId] ASC, [CarYear] ASC, [GuideId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CarValuesS2014__CarYear]
    ON [dbo].[CarValues]([CarYear] ASC)
    INCLUDE([CarVersionId]);

