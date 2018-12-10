IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CV].[GetUsedCarValues]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CV].[GetUsedCarValues]
GO

	-- =============================================    
-- Author:  Umesh Ojha    
-- Create date: 12/2/2013    
-- Description: Fetching data for car values    
-- =============================================    
CREATE PROCEDURE [CV].[GetUsedCarValues]   -- EXEC GetUsedCarValues 2007,130,1 
 -- Add the parameters for the stored procedure here    
 @Year SMALLINT,  
 @VersionId BIGINT,  
 @CityId INT  
    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;  
    
    --Insert statements for procedure here    
 --SELECT Deviation,  
 --IsNull((SELECT CarValue FROM CarValues WHERE CarYear=@Year AND CarVersionId=@VersionId AND GuideId=CD.GuideId), 0) CurrentYearCarValue,   
 --IsNull((SELECT CarValue FROM CarValues WHERE CarYear=@Year+1  AND CarVersionId=@VersionId AND GuideId=CD.GuideId), 0) NextYearCarValue   
 --FROM CarValuesCityDeviation CD  
 --WHERE CD.CityId=@CityId   
   
	SELECT Deviation,IsNull(CV1.CarValue,0) AS CurrentYearCarValue,  
		IsNull(CV2.CarValue,0) AS NextYearCarValue        
		FROM CarValuesCityDeviation CD  
		LEFT OUTER  JOIN  CarValues AS CV1 ON CV1.GuideId=CD.GuideId  AND cv1.CarYear = @Year AND cv1.CarVersionId=@VersionId
		LEFT OUTER  JOIN CarValues AS CV2 ON CV2.GuideId=CD.GuideId AND CV2.CarYear = CV1.CarYear+1  AND cv2.CarVersionId=@VersionId 
	WHERE CD.CityId = @CityId  		
 
END
