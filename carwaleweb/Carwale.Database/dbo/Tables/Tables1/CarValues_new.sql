CREATE TABLE [dbo].[CarValues_new] (
    [CCV_ID]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CarVersionId] NUMERIC (18) NOT NULL,
    [CarYear]      INT          NOT NULL,
    [CarValue]     NUMERIC (18) NOT NULL,
    [GuideId]      NUMERIC (18) NOT NULL
);

