CREATE TABLE [dbo].[ModelDescriptions] (
    [ModelId]      NUMERIC (18)  NOT NULL,
    [Pros]         VARCHAR (100) NOT NULL,
    [Cons]         VARCHAR (100) NOT NULL,
    [VersionId]    NUMERIC (18)  NOT NULL,
    [VersionName]  VARCHAR (50)  NOT NULL,
    [MDescription] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ModelDescriptions] PRIMARY KEY CLUSTERED ([ModelId] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CarWale Choice', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ModelDescriptions', @level2type = N'COLUMN', @level2name = N'VersionId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CarWale Choice', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ModelDescriptions', @level2type = N'COLUMN', @level2name = N'VersionName';

