CREATE TABLE [dbo].[ESM_Platforms] (
    [ESM_PlatformId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (100) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_ESM_Platforms_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_ESM_Platforms] PRIMARY KEY CLUSTERED ([ESM_PlatformId] ASC)
);

