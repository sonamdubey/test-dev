IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCampaignDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCampaignDetails]
GO
	-- =============================================
-- Author:		Vivek gupta
-- Create date: 8th oct,2013
-- Description:	Load Campaign Details
-- Modified By: Vivek Gupta on 21-10-2013, commented area retriving statements and added model and city retreiving statements
-- Modified By: Vivek Gupta on 02-06-2014 added condition IsDeleted on Cities table.
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCampaignDetails]
@BranchId INT,
@CampaignId INT,
@UserId INT
AS
BEGIN	
	SET NOCOUNT ON;
    
	SELECT TC_MainCampaignId AS ID, MainCampaignName AS Campaign 
	FROM TC_MainCampaign 
	WHERE IsActive = 1 AND MakeId = 20

	IF(@BranchId IS NULL)
	BEGIN
		SELECT C.ID AS Value, C.Name AS Text FROM Cities AS C WHERE C.IsDeleted=0
	    Order By C.Name  
	END
	ELSE 
	BEGIN
	    EXECUTE TC_DealerCitiesView @BranchId
	END

	DECLARE @MainCampaignId INT = NULL
	--DECLARE @CityId INT = NULL

	IF(@CampaignId IS NOT NULL)
	BEGIN
	   SELECT TC_MainCampaignId,TC_SubCampaignId,CampaignName,
	          Amount,LeadTarget,CampaignFromDate,CampaignToDate,Details,IsSpecialUser
	   FROM TC_CampaignScheduling 
	   WHERE 
	           --(BranchId=@BranchId OR @Branchid IS NULL)
		   --AND UserId = @UserId 
		    TC_CampaignSchedulingId = @CampaignId
		   AND IsActive = 1
	   
	   SELECT @MainCampaignId=TC_MainCampaignId--,@CityId=CityId
	   FROM TC_CampaignScheduling 
	   WHERE 
	           --(BranchId=@BranchId OR @Branchid IS NULL)
		  -- AND UserId = @UserId 
		    TC_CampaignSchedulingId = @CampaignId
		   AND IsActive = 1
	END

	IF(@MainCampaignId IS NOT NULL)
	BEGIN
	   EXEC TC_GetSubCampaigns @MainCampaignId
	END

	--IF(@CityId IS NOT NULL)
	--BEGIN
	--  EXEC TC_GetAreas @BranchId, @CityId
	--END

	--IF(@CampaignId IS NOT NULL)
	--BEGIN
	--	SELECT TCAM.AreasId 
	--	FROM TC_CampaignAreaMapping TCAM WITH(NOLOCK)
	--	WHERE TCAM.TC_CampaignSchedulingId = @CampaignId AND IsActive = 1
	--END
	-- Added By Vivek Gupta on 21stoct,2013
	IF(@CampaignId IS NOT NULL)
	BEGIN
		SELECT TCMM.ModelId 
		FROM TC_CampaignModelMapping TCMM WITH(NOLOCK)
		WHERE TCMM.TC_CampaignSchedulingId = @CampaignId AND IsActive = 1
	END

	IF(@CampaignId IS NOT NULL)
	BEGIN
		SELECT TCCM.CityId
		FROM TC_CampaignCityMapping TCCM WITH(NOLOCK)
		WHERE TCCM.TC_CampaignSchedulingId = @CampaignId AND IsActive = 1
	END
END


