CREATE TABLE [dbo].[Microsite_DWModelFeatures] (
    [Id]                                   INT           IDENTITY (1, 1) NOT NULL,
    [Microsite_DWModelFeatureCategoriesId] INT           NULL,
    [FeatureTitle]                         VARCHAR (100) NULL,
    [FeatureDescription]                   VARCHAR (MAX) NULL,
    [HostUrl]                              VARCHAR (50)  NULL,
    [ImgPath]                              VARCHAR (50)  NULL,
    [ImgName]                              VARCHAR (50)  NULL,
    [IsActive]                             BIT           CONSTRAINT [DF_Microsite_DWModelFeatures_IsActive] DEFAULT ((1)) NULL,
    [EntryDate]                            DATETIME      CONSTRAINT [DF_Microsite_DWModelFeatures_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]                         DATETIME      NULL,
    [SortOrder]                            INT           NULL,
    [OriginalImgPath]                      VARCHAR (250) NULL,
    CONSTRAINT [PK_Microsite_DWrModelFeatures] PRIMARY KEY CLUSTERED ([Id] ASC)
);

