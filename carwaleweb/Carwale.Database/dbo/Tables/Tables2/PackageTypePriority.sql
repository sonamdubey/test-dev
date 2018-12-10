CREATE TABLE [dbo].[PackageTypePriority] (
    [ID]          SMALLINT IDENTITY (1, 1) NOT NULL,
    [PackageType] SMALLINT NOT NULL,
    [Priority]    TINYINT  NOT NULL,
    CONSTRAINT [PK_PackageTypePriority] PRIMARY KEY CLUSTERED ([PackageType] ASC)
);

