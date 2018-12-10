IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SelectModelsByBodyStyle]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SelectModelsByBodyStyle]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		Satish Sharma
-- Create date: 10/10/2011 2:20PM
-- Description:	This sp will return list of models on the basis of Body Style Id
-- 
-- =============================================
CREATE PROCEDURE SelectModelsByBodyStyle
	-- Add the parameters for the stored procedure here
	@ReturnRecords SMALLINT,
	@BodyStyleId SMALLINT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
	SELECT distinct top (@ReturnRecords) MO.ID AS ModelId, MA.Name CarMake, MO.Name AS CarModel, MA.ID AS MakeId,	
	MO.SmallPic, CB.Name AS BodyStyle, MIN(ISNULL(SP.AvgPrice , 0)) OVER( PARTITION BY MO.ID ) AS MinPrice,  
	MAX(ISNULL(SP.AvgPrice ,0)) OVER( PARTITION BY MO.ID ) AS MaxPrice,  ISNULL(MO.ReviewRate, 0) 
	MoReviewRate, ISNULL(MO.ReviewCount,0) MoReviewCount 

	From  CarMakes AS MA, CarModels AS MO, CarBodyStyles AS CB, Con_NewCarNationalPrices AS SP,  CarVersions AS CV 

	Where  MA.ID = MO.CarMakeId AND MO.ID = CV.CarModelId AND CV.New = 1 AND  SP.VersionId = CV.ID AND CB.ID = CV.BodyStyleId AND  
	CV.BodyStyleId = @BodyStyleId ORDER BY MoReviewCount DESC
	
END

