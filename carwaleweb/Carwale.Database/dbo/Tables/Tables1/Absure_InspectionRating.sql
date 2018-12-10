CREATE TABLE [dbo].[Absure_InspectionRating] (
    [Absure_InspectionRatingId] INT     IDENTITY (1, 1) NOT NULL,
    [InspectionFeedbackId]      BIGINT  NOT NULL,
    [RatingCategoryId]          TINYINT NOT NULL,
    [RatingValue]               TINYINT NOT NULL,
    CONSTRAINT [PK_Absure_InspectionRating] PRIMARY KEY CLUSTERED ([Absure_InspectionRatingId] ASC)
);

