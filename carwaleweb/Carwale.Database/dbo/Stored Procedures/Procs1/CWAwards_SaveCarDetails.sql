IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CWAwards_SaveCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CWAwards_SaveCarDetails]
GO

	
-- =============================================  
-- Author:  Vaibhav K  
-- Create date: 17-Jan-2013  
-- Description: Save the details about the car in case of CWAwards (Page-2 Questions about the car)  
-- =============================================  
CREATE PROCEDURE [dbo].[CWAwards_SaveCarDetails]  
 -- Add the parameters for the stored procedure here  
 @SurveyId			BIGINT,  
 @RecommendPoint	TINYINT,  
 @Mileage			TINYINT,  
 @Economy			TINYINT,  
 @Exterior			TINYINT,  
 @Comfort			TINYINT,  
 @Performance		TINYINT,  
 @ValueForMoney		TINYINT,  
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
    SET RecommendPoint = @RecommendPoint, Mileage = @Mileage, Economy = @Economy, Exterior = @Exterior,  
		Comfort = @Comfort, Performance = @Performance, ValueForMoney = @ValueForMoney  
		WHERE SurveyId = @SurveyId
   SET @Status = 1  
  END   
END  
