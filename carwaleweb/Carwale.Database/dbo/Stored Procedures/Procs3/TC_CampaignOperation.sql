IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CampaignOperation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CampaignOperation]
GO

	
-- Author		:	Tejashree Patil.
-- Create date	:	8 Oct 2013.
-- Description	:	This SP used to add and udate camapign.
-- Modified By  :   Vivek on 21stOct,2013, removed @Arealist,@modelId, Added @ModelList,@Details,@CityList
-- =============================================    
CREATE PROCEDURE [dbo].[TC_CampaignOperation]
 -- Add the parameters for the stored procedure here    
 @BranchId BIGINT,
 @UserId BIGINT,
 @IsSpecialUser BIT,
 @MainCampaignId INT,
 @SubCampaignId INT,
 @CampaignName VARCHAR(150),
 @CampaignFromDate DATETIME,
 @CampaignToDate DATETIME,
 @CityList VARCHAR(500),
 @Details VARCHAR(MAX),
 @Amount INT,
 @LeadTarget INT,
 @ModelList VARCHAR(500),
 @TC_CampaignSchedulingId INT,
 @Status SMALLINT OUTPUT
AS    
BEGIN    
		DECLARE @ExistingCampaignSchedulingId INT = NULL
		SET @Status = 1

		SET @ExistingCampaignSchedulingId = (SELECT  Top 1 CS.TC_CampaignSchedulingId
								FROM	TC_CampaignScheduling CS
										INNER JOIN TC_CampaignCityMapping CCM ON CCM.TC_CampaignSchedulingId=CS.TC_CampaignSchedulingId
										INNER JOIN TC_CampaignModelMapping CMM ON CMM.TC_CampaignSchedulingId=CS.TC_CampaignSchedulingId
								WHERE	(@BranchId IS NULL AND CS.IsSpecialUser=1 AND CS.IsActive=1) 
									AND 
									(
										(CampaignName = @CampaignName)
										OR																				
										(											
										    (CMM.ModelId IN (	SELECT  ListMember
																FROM	fnSplitCSV(@ModelList) 
																WHERE	CMM.IsActive=1))
											AND
											(	(CONVERT(DATE,@CampaignFromDate) BETWEEN CONVERT(DATE,CampaignFromDate) AND CONVERT(DATE,CampaignToDate))
												OR 
												(CONVERT(DATE,@CampaignToDate) BETWEEN CONVERT(DATE,CampaignFromDate) AND CONVERT(DATE,CampaignToDate))
												OR
												(CONVERT(DATE,CampaignFromDate) BETWEEN CONVERT(DATE,@CampaignFromDate) AND CONVERT(DATE,@CampaignToDate))	
											)

											AND
											(
												((CCM.CityId = -1 AND CCM.IsActive=1) 
													OR 
												 (CCM.CityId IN (	SELECT  ListMember
																FROM	fnSplitCSV(@CityList) 
																WHERE	CCM.IsActive=1))
											        OR													
										        (@CityList = '-1'
													AND
												 CCM.CityId <> -1)
												 )
											)
										)										
									 )
								 )
										

   IF (@TC_CampaignSchedulingId IS NULL AND @ExistingCampaignSchedulingId IS NOT NULL)
   BEGIN
    SET @Status = 0
   END

	IF (@TC_CampaignSchedulingId IS NULL AND @ExistingCampaignSchedulingId IS NULL)
	BEGIN
	     -- Modified By : Vivek on 21stOct,2013
		INSERT INTO  TC_CampaignScheduling (TC_MainCampaignId,TC_SubCampaignId,CampaignName,CampaignFromDate,
					 CampaignToDate,Amount,LeadTarget,BranchId,UserId,IsActive,IsSpecialUser,EntryDate,Details)
		VALUES		(@MainCampaignId,@SubCampaignId,@CampaignName,@CampaignFromDate,@CampaignToDate,
					 @Amount,@LeadTarget,@BranchId,@UserId,1,@IsSpecialUser,GETDATE(),@Details)

	  SET @TC_CampaignSchedulingId=SCOPE_IDENTITY()


	  -- ADDED By : Vivek on 21stOct,2013
	  INSERT INTO TC_CampaignModelMapping (TC_CampaignSchedulingId , 
	                                      ModelId,
										  IsActive)
							SELECT  @TC_CampaignSchedulingId,
							        ListMember,
									1
							FROM fnSplitCSV(@ModelList)
	     -- ADDED By : Vivek on 21stOct,2013
	  INSERT INTO TC_CampaignCityMapping (TC_CampaignSchedulingId , 
	                                      CityId,
										  IsActive)
							SELECT  @TC_CampaignSchedulingId,
							        ListMember,
									1
							FROM fnSplitCSV(@CityList)
	END
	ELSE IF(@TC_CampaignSchedulingId IS NOT NULL AND (@IsSpecialUser = 0 OR @TC_CampaignSchedulingId = @ExistingCampaignSchedulingId))
	BEGIN
	-- Modified By : Vivek on 21stOct,2013
		UPDATE	TC_CampaignScheduling
		SET		CampaignName = @CampaignName,
				CampaignFromDate = @CampaignFromDate,
				CampaignToDate = @CampaignToDate,
				--CityId = @CityId,
				Amount = @Amount,
				LeadTarget = @LeadTarget,
				ModifiedBy=@UserId,
				ModifiedDate=GETDATE(),
				--ModelId = @ModelId
				Details = @Details
		WHERE	TC_CampaignSchedulingId=@TC_CampaignSchedulingId;

		-- Added By : Vivek on 21stOct,2013    	 
	  INSERT INTO TC_CampaignModelMapping (TC_CampaignSchedulingId , 
	                                      ModelId,
										  IsActive)
							SELECT  @TC_CampaignSchedulingId AS TC_CampaignSchedulingId,
							        ListMember AS ModelId,
									1 AS IsActive
							FROM fnSplitCSV(@ModelList)
							WHERE  ListMember NOT IN ( SELECT ModelId FROM TC_CampaignModelMapping 
							                           WHERE TC_CampaignSchedulingId=@TC_CampaignSchedulingId
													   AND IsActive=1
													  )
-- Added By : Vivek on 21stOct,2013

       IF(@CityList <> '-1') --this if block is given for differentiating All City and selected cities coz -1 indicates all cities.
	   BEGIN
	     UPDATE TC_CampaignCityMapping SET IsActive = 0
		 WHERE TC_CampaignSchedulingId = @TC_CampaignSchedulingId AND CityId = -1
	   END
	   INSERT INTO TC_CampaignCityMapping (TC_CampaignSchedulingId , 
	                                    CityId,
										IsActive)
							SELECT  @TC_CampaignSchedulingId AS TC_CampaignSchedulingId,
									ListMember AS CityId,
									1 AS IsActive
							FROM fnSplitCSV(@CityList)
							WHERE  ListMember NOT IN ( SELECT CityId FROM TC_CampaignCityMapping 
														WHERE TC_CampaignSchedulingId=@TC_CampaignSchedulingId
														AND IsActive=1
														)

	END

	ELSE 
	  SET @Status = 0

END    



