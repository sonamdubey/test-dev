CREATE TABLE [dbo].[CarValuesUpdated250716] (
    [CarVersionId] NUMERIC (18) NOT NULL,
    [CarYear]      INT          NOT NULL,
    [CarValue]     NUMERIC (18) NOT NULL,
    [GuideId]      NUMERIC (18) CONSTRAINT [DF_CarValuesS2014_GuideId_1] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CarValuesS2014_1] PRIMARY KEY CLUSTERED ([CarVersionId] ASC, [CarYear] ASC, [GuideId] ASC)
);

