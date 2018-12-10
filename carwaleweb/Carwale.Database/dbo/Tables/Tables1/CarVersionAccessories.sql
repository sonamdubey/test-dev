CREATE TABLE [dbo].[CarVersionAccessories] (
    [CarVersionId]     NUMERIC (18) NOT NULL,
    [CarAccessoriesId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CarVersionAccessories] PRIMARY KEY CLUSTERED ([CarVersionId] ASC, [CarAccessoriesId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CarVersionAccessories_CarAccessories] FOREIGN KEY ([CarAccessoriesId]) REFERENCES [dbo].[CarAccessories] ([ID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_CarVersionAccessories_CarVersions] FOREIGN KEY ([CarVersionId]) REFERENCES [dbo].[CarVersions] ([ID]) ON UPDATE CASCADE
);

