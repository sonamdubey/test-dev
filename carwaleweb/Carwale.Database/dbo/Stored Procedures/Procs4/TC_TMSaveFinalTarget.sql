IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMSaveFinalTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMSaveFinalTarget]
GO

	-- =============================================
-- Author	    :	Vinayak Patil
-- Create date	:	20-11-2013
-- Description	:	Update the target scenario for All Target types.
-- Modified By  :	Nilesh Utture on 12-12-13, Deleted data from approval tables as final target is replaced
-- Modified By  :  Manish Chourasiya on 11-03-2014 changing percentage values and insert records for the target othe than retail also.
-- =============================================
 CREATE PROCEDURE [dbo].[TC_TMSaveFinalTarget]
 @TC_TMDistributionPatternMasterId INT,
 @TC_SpecialUsersId INT 
 
 AS
   BEGIN

		DECLARE @Master_Year SMALLINT,  -- To store the value of year against @TC_TMDistributionPatternMasterId in TC_TMDistributionPatternMaster
				@InquiryTarget float = 10,
				@BookingTarget float = 1.10,
				@TDTarget float = 10 * 0.75

		SELECT @Master_Year = LegacyYear
		FROM   TC_TMDistributionPatternMaster
		WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId  



		------------ Copies data into TC_TMDealersTargetArchive before deleting from TC_DealersTarget--------
		------------ So that we will have backup copy of the that data---------------------------------------

		INSERT INTO TC_TMDealersTargetArchive
					(DealerId,
					 Month,
					 Year,
					 Target,
					 CreatedBy,
					 IsDeleted,
					 TC_TargetTypeId,
					 CarVersionId,
					 TC_SpecialUsersId,
					 CreatedOn)
		SELECT DealerId,
			   Month,
			   Year,
			   Target,
			   CreatedBy,
			   IsDeleted,
			   TC_TargetTypeId,
			   CarVersionId,
			   @TC_SpecialUsersId,
			   GETDATE()
		FROM   TC_DealersTarget WITH(NOLOCK)
		WHERE  Year = @Master_Year
			  AND TC_TargetTypeId = 4  

	 
		-------------- Delete data from the TC_DealersTarget because now we will have------------------
		-------------- new dealer target set by new Special User in this table ------------------------	  
		DELETE FROM TC_DealersTarget
		WHERE  Year = @Master_Year
			 --  AND TC_TargetTypeId = 4  
			   AND  ( [Month]>=MONTH(GETDATE())  OR [Year]>YEAR(GETDATE())) --- not delete the records for past month


	 


		-------Inserts updated data into TC_DealersTarget from TC_TMTargetScenarioDetail----------------
		------- So as to save this data in this table---------------------------------------------------	 
		INSERT INTO TC_DealersTarget
					(DealerId,
					 Month,
					 Year,
					 Target,
					 CreatedBy,
					 IsDeleted,
					 TC_TargetTypeId,
					 CarVersionId)
		SELECT DealerId,
			   Month,
			   Year,
			   Target,
			   @TC_SpecialUsersId,
			   0,
			   4,
			   CarVersionId
		FROM   TC_TMTargetScenarioDetail WITH(NOLOCK)
		WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId  
		  AND ( [Month]>=MONTH(GETDATE())  OR [Year]>YEAR(GETDATE()))

         UNION ALL
				--  ---------------- INSERT DATA FOR INQUIRY---------------------------------------

				SELECT DealerId,
					   Month,
					   Year,
					   ROUND(Target * @InquiryTarget*1.0000,0),
					   @TC_SpecialUsersId,
					   0,
					   1,
					   CarVersionId
				FROM   TC_TMTargetScenarioDetail WITH(NOLOCK)
				WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId  
				  AND ( [Month]>=MONTH(GETDATE())  OR [Year]>YEAR(GETDATE()))
				   UNION ALL
		  -- 		  ---------------- INSERT DATA FOR TEST DRIVE---------------------------------------
		         SELECT DealerId,
					   Month,
					   Year,
					   ROUND(Target * @TDTarget*1.0000,0),
					   @TC_SpecialUsersId,
					   0,
					   2,
					   CarVersionId
				FROM   TC_TMTargetScenarioDetail WITH(NOLOCK)
				WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId  
				  AND ( [Month]>=MONTH(GETDATE())  OR [Year]>YEAR(GETDATE())) 
		          UNION ALL 
				--   ---------------- INSERT DATA FOR BOOKINGS---------------------------------------
		        SELECT DealerId,
					   Month,
					   Year,
					  ROUND(Target * 1.0000 *@BookingTarget,0),
					   @TC_SpecialUsersId,
					   0,
					   3,
					   CarVersionId
				FROM   TC_TMTargetScenarioDetail WITH(NOLOCK)
				WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId  
				  AND ( [Month]>=MONTH(GETDATE())  OR [Year]>YEAR(GETDATE()))

	  
	 

		----------Update to set IsTargetApplied bit to 0 for all the records in the year ---------------
		----------because new updated target will be applied, so set IsTrgetApplied bit ----------------
		---------- to 0 for alll other records in master table------------------------------------------
		UPDATE TC_TMDistributionPatternMaster  
		SET    IsTargetApplied = 0
		WHERE  LegacyYear = @Master_Year 


		----------Update to set IsTargetApplied bit to 1 after update So ---------------
		----------that we will come to know which target active at this moment----------	 
		UPDATE TC_TMDistributionPatternMaster
		SET    IsTargetApplied = 1
		WHERE  TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId 
		AND	   LegacyYear = @Master_Year


		-- Modified By  :	Nilesh Utture on 12-12-13
		DELETE FROM TC_TMAMTargetChangeApprovalReq WHERE Year = @Master_Year
		UPDATE TC_TMAMTargetChangeMaster SET IsActive = 0 WHERE Year = @Master_Year
		

END
