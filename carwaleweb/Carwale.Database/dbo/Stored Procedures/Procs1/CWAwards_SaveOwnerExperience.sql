IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CWAwards_SaveOwnerExperience]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CWAwards_SaveOwnerExperience]
GO

	
-- =============================================  
-- Author:  Vaibhav K  
-- Create date: 17-Jan-2013  
-- Description: Save the details about the car in case of CWAwards (Page-2 Questions about the car)  
-- =============================================  
CREATE PROCEDURE [dbo].[CWAwards_SaveOwnerExperience]  
 -- Add the parameters for the stored procedure here  
 @SurveyId			BIGINT,  
 @ServiceCost		TINYINT,  
 @ServiceQuality	TINYINT,  
 @TechKnowledge		TINYINT,  
 @Staff				TINYINT,  
 @BuyAgain			TINYINT,
 @IsReplace			BIT,
 @ReplaceDuration	VARCHAR(50),
 @ReplaceMakedId	SMALLINT,
 @ReplaceMake		VARCHAR(50),
 @ReplaceModelId	SMALLINT,
 @ReplaceModel		VARCHAR(50),
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
		SET ServiceCost = @ServiceCost, QualityofService = @ServiceQuality, TechnicalKnowledge = @TechKnowledge, Staff = @Staff, BuyAgain = @BuyAgain,  
			IsReplace = @IsReplace, ReplaceDuration = @ReplaceDuration, ReplaceMakedId = @ReplaceMakedId, ReplaceMake = @ReplaceMake,
			ReplaceModelId = @ReplaceModelId, ReplaceModel = @ReplaceModel
	   WHERE SurveyId = @SurveyId  
	     
	   SET @Status = 1  
	  END   
END  
