CREATE TABLE [dbo].[TC_TMIntermediateLegacyDetail] (
    [TC_TMIntermediateLegacyDetailId] INT        IDENTITY (1, 1) NOT NULL,
    [DealerId]                        INT        NULL,
    [CarVersionId]                    INT        NULL,
    [Month]                           TINYINT    NULL,
    [Year]                            SMALLINT   NULL,
    [Target]                          FLOAT (53) NULL,
    CONSTRAINT [PK_TC_TMIntermediateLegacyDetail] PRIMARY KEY CLUSTERED ([TC_TMIntermediateLegacyDetailId] ASC)
);

