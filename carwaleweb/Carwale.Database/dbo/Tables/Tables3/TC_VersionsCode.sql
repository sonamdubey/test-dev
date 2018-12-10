CREATE TABLE [dbo].[TC_VersionsCode] (
    [CarVersionId]   INT          NULL,
    [CarVersionCode] VARCHAR (30) NULL,
    [IsActive]       BIT          CONSTRAINT [DF_TC_VersionsCode_IsActive] DEFAULT ((1)) NULL
);

