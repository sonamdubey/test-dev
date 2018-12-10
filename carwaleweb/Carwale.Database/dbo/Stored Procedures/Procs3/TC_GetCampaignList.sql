IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCampaignList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCampaignList]
GO
	-- Author		:	Tejashree Patil.
-- Create date	:	9 Oct 2013.
-- Description	:	This SP used to get list of all campaigns.
-- [TC_GetCampaignList] 3692,4693,0,NULL,NULL,NULL,NULL,NULL,NULL,1,50
-- [TC_GetCampaignList] NULL,9,1,NULL,'05-16-2014',NULL,NULL,NULL,NULL,1,50
-- Modified By  :	Tejashree Patil on 19 May 2014, Added @CampaignFromDate and @CampaignToDate condition in WHERE clause.
-- =============================================    
CREATE PROCEDURE [dbo].[TC_GetCampaignList]
 -- Add the parameters for the stored procedure here    
 @BranchId BIGINT,
 @UserId BIGINT,
 @IsSpecialUser BIT,
 @CampaignName VARCHAR(150),
 @CampaignFromDate DATETIME,
 @CampaignToDate DATETIME,
 @RegionId INT,
 @AMId INT,
 @DealerId INT,--Id of dealer of which want to search.
 @FromIndex TINYINT = NULL,
 @ToIndex TINYINT = NULL
AS    
BEGIN	
	
	DECLARE @SpecialUserId INT = @UserId
	IF(@IsSpecialUser=0)
	BEGIN
		SET @IsSpecialUser = NULL
		SET @SpecialUserId = NULL
	END
	
	DECLARE @tblDealers TABLE(Id INT)
	INSERT INTO @tblDealers(Id)
	SELECT	ID 
	FROM	Dealers WITH(NOLOCK)
	WHERE	(@AMId IS NULL OR TC_AMId=@AMId)
			AND (@RegionId IS NULL OR TC_BrandZoneId=@RegionId)
			AND (@DealerId IS NULL OR Id=@DealerId)

	BEGIN
		WITH cte AS (
			SELECT  TC_CampaignSchedulingId,CS.TC_MainCampaignId,CS.TC_SubCampaignId,CS.CampaignName,CampaignFromDate,
					CampaignToDate,CS.CityId,Amount,LeadTarget,CS.BranchId,UserId,IsSpecialUser,CS.IsActive,
					VC.MainCampaignName,VC.SubCampaignName, VC.SubMainCampaignName,
					CASE 
					WHEN IsSpecialUser=1 THEN SPU.UserName
					ELSE U.UserName
					END AS UserName,
					D.Organization,
					--ROW_NUMBER() OVER( ORDER BY CS.CampaignToDate DESC )AS RowNo
					ROW_NUMBER() OVER( ORDER BY CS.EntryDate DESC )AS RowNo
			FROM	TC_CampaignScheduling CS WITH(NOLOCK)
					INNER JOIN TC_vwCampaignMaster  AS VC ON VC.TC_SubCampaignId=CS.TC_SubCampaignId
					--LEFT JOIN TC_vwAllUsers U  WITH(NOLOCK) ON U.ID=CS.UserId
					LEFT JOIN TC_SpecialUsers SPU WITH(NOLOCK) ON SPU.TC_SpecialUsersId=CS.UserId AND IsSpecialUser=1
					LEFT JOIN TC_Users U WITH(NOLOCK) ON U.Id=CS.UserId AND IsSpecialUser=0
					LEFT JOIN Dealers D WITH(NOLOCK) ON D.Id = CS.BranchId
			WHERE	(@CampaignName IS NULL OR CS.CampaignName LIKE '%'+@CampaignName+'%')
					AND 
					   (   (        (@CampaignFromDate IS NULL OR CONVERT(DATE,CS.CampaignFromDate)=@CampaignFromDate)
					           AND  (@CampaignToDate IS NULL OR CONVERT(DATE,CS.CampaignToDate)=@CampaignToDate)
					        )
						  OR-- Modified By  :	Tejashree Patil on 19 May 2014,
                           (		(@CampaignFromDate IS NOT NULL AND  CONVERT(DATE,CS.CampaignFromDate)>=@CampaignFromDate) 
						       AND  (@CampaignToDate IS NOT NULL AND  CONVERT(DATE,CS.CampaignToDate)<=@CampaignToDate)  
						   )
					   )
			        AND (@DealerId IS NULL OR CS.BranchId = @DealerId)
					AND (
							((@SpecialUserId IS NOT NULL) AND (CS.BranchId IS NULL OR CS.BRANCHID IN (SELECT Id FROM  @tblDealers))) 
							OR  
							((@IsSpecialUser IS NULL) AND (CS.BRANCHID = @BranchId))
						)
					AND CS.IsActive=1
					)
					
				
											
		SELECT * 
		INTO   #TblTemp 
		FROM   cte		

		SELECT * 
		FROM   #TblTemp
		WHERE  RowNo BETWEEN @FromIndex AND @ToIndex 
		ORDER BY RowNo 
	      

		SELECT COUNT(*) AS RecordCount 
		FROM   #TblTemp
				
		DROP TABLE #TblTemp 	
	END	
END

