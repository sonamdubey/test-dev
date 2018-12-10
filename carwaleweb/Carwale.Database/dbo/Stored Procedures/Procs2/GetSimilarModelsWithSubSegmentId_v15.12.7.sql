IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSimilarModelsWithSubSegmentId_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSimilarModelsWithSubSegmentId_v15]
GO

	-- =============================================
-- Author:		Anchal Gupta
-- Create date: 2/12/2015
-- Description:	Getting the similar models based subsegment Id only order by popularity
-- Modified By Chetan <14/12/2015> - Removing SubSegmentID as a parameter coming from repository
-- EXEC [GetSimilarModelsWithSubSegmentId_v15.12.7] 113
-- =============================================
CREATE PROCEDURE [dbo].[GetSimilarModelsWithSubSegmentId_v15.12.7] 
	-- Add the parameters for the stored procedure here
	 @ModelId INT
AS
BEGIN
	DECLARE  @BodyStyle INT,
	@SubSegment INT

	select @BodyStyle = ModelBodyStyle, @SubSegment = SubSegmentID from CarModels WITH (NOLOCK) where ID = @ModelId

	SELECT Id AS ModelId, ModelBodyStyle, ModelPopularity, SubSegmentID
	FROM Carmodels  WITH (NOLOCK)
	WHERE SubSegmentId = @SubSegment-- Modified by Chetan
		AND ModelBodyStyle = @BodyStyle
		AND New = 1
		And IsDeleted = 0
		AND Id <> @ModelId
		
END

