CREATE TABLE [dbo].[TC_DealerFeature] (
    [TC_DealerFeatureId] INT           IDENTITY (1, 1) NOT NULL,
    [FeatureType]        VARCHAR (200) NULL,
    [IsActive]           BIT           NULL,
    CONSTRAINT [PK_TC_DealerFeatureId] PRIMARY KEY CLUSTERED ([TC_DealerFeatureId] ASC)
);

