CREATE TABLE [dbo].[BackupCarValues] (
    [CarVersionId] NUMERIC (18) NOT NULL,
    [CarYear]      INT          NOT NULL,
    [CarValue]     NUMERIC (18) NOT NULL,
    [GuideId]      NUMERIC (18) CONSTRAINT [DF_CarValuesNew_GuideId] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_CarValuesNew] PRIMARY KEY CLUSTERED ([CarVersionId] ASC, [CarYear] ASC, [GuideId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CarValues__CarYear]
    ON [dbo].[BackupCarValues]([CarYear] ASC)
    INCLUDE([CarVersionId]);

