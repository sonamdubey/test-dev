CREATE TABLE [dbo].[Microsite_DWModelFeatures_Bkp15082015] (
    [Id]                                   INT           IDENTITY (1, 1) NOT NULL,
    [Microsite_DWModelFeatureCategoriesId] INT           NULL,
    [FeatureTitle]                         VARCHAR (100) NULL,
    [FeatureDescription]                   VARCHAR (MAX) NULL,
    [HostUrl]                              VARCHAR (50)  NULL,
    [ImgPath]                              VARCHAR (50)  NULL,
    [ImgName]                              VARCHAR (50)  NULL,
    [IsActive]                             BIT           NULL,
    [EntryDate]                            DATETIME      NULL,
    [ModifiedDate]                         DATETIME      NULL,
    [SortOrder]                            INT           NULL,
    [OriginalImgPath]                      VARCHAR (250) NULL
);

