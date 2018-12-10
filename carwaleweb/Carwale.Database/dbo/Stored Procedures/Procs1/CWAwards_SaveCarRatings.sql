IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CWAwards_SaveCarRatings]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CWAwards_SaveCarRatings]
GO

	
-- =============================================  
-- Author:  Vaibhav K  
-- Create date: 17-Jan-2013  
-- Description: Save the details about the car in case of CWAwards (Page-2 Questions about the car)  
-- =============================================  
CREATE PROCEDURE [dbo].[CWAwards_SaveCarRatings]  
 -- Add the parameters for the stored procedure here  
 @SurveyId			BIGINT,  
 @Reliability		TINYINT,  
 @Braking			TINYINT,  
 @Handling			TINYINT,  
 @RideQuality		TINYINT,
 @EaseOfDriving		TINYINT,
 @Design			TINYINT,  
 @BuildQuality		TINYINT,  
 @Interior			TINYINT,  
 @SafetyFeature		TINYINT,  
 @Status			SMALLINT OUTPUT  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
    -- Insert statements for procedure here  
    SET @Status = -1  
      
    IF @SurveyId <> -1  
  BEGIN  
   UPDATE CWAwards  
    SET Reliability = @Reliability, Braking = @Braking, Handling = @Handling, RideQuality = @RideQuality, EaseOfDriving = @EaseOfDriving, 
		Design = @Design, BuildQuality = @BuildQuality, Interior = @Interior, SafetyFeature = @SafetyFeature  
   WHERE SurveyId = @SurveyId  
     
   SET @Status = 1  
  END   
END  
