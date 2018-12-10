CREATE TABLE [dbo].[OLM_SkodaVersionFeatures] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [FeatureId] INT          NOT NULL,
    [VersionId] INT          NOT NULL,
    [Value]     SMALLINT     NOT NULL,
    [IsActive]  BIT          NOT NULL,
    [UpdatedOn] DATETIME     NULL,
    [UpdatedBy] BIGINT       NULL,
    CONSTRAINT [PK_OLM_SkodaVersionFeatures] PRIMARY KEY CLUSTERED ([Id] ASC)
);

