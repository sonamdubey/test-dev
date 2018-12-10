IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetWarrentyReqDealerList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetWarrentyReqDealerList]
GO

	-- =============================================
-- Author:		Tejashree Patil	
-- Create date: 12/12/2014
-- Description:	Get dealer list warranty requested.
--[AbSure_GetWarrentyReqDealerList] 13191 --23283
-- Modified By : Tejashree Patil on 5 Jan 2014, Commented Stock StatusId condition.
-- Modified By Tejashree Patil on 13 March 2015, Selected only IsActive = 1 cars.
-- Modified By Ruchira Patil on 17 March 2015, Selected only IsRejected = 0 cars.
-- Modified By Vinay Kumar Prajapati 12th May 2015 Append Name of carwale  Sales Field with DealerName  
-- Modified By  : Ashwini Dhamankar on June 23,2015, Fetched Latest App Version
-- Modified By Ruchira Patil on 31st Aug 2015, fetched emailid , areaId and cityId of dealer
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetWarrentyReqDealerList]-- 13191,91
@UserId INT,
@ApplicationType SMALLINT = NULL
AS
BEGIN

	SELECT DISTINCT  D.Id DealerId, 
	D.Organization +  (CASE WHEN OU.UserName IS NULL THEN ' ' ELSE ' : ' END) + ISNULL(OU.UserName,'') 
	                 +  (CASE WHEN OU.PhoneNo IS NULL THEN ' ' ELSE ' : ' END) + ISNULL(OU.PhoneNo,'') DealerName , -- Modified By Vinay Kumar Prajapati 
		--	D.Organization AS DealerName,
		    C.Name City, COUNT(CD.Id) CarCount, 
			D.Longitude, D.Lattitude, D.MobileNo, ISNULL(D.Address1,D.Address2) Address,ISNULL(D.RCNotMandatory,0) IsRCNotMandatory,
			D.EmailId EmailId,D.AreaId AreaId,D.CityId CityId
						
	FROM	Dealers D WITH(NOLOCK)
			INNER JOIN AbSure_CarDetails CD			WITH(NOLOCK)	ON CD.DealerId = d.ID
			INNER JOIN Cities C						WITH(NOLOCK)	ON C.ID = D.CityId			
			INNER JOIN AbSure_CarSurveyorMapping CS WITH(NOLOCK)	ON CS.AbSure_CarDetailsId = cd.Id
			LEFT JOIN DCRM_ADM_UserDealers AS DAUD  WITH(NOLOCK)    ON DAUD.DealerId=D.ID  AND DAUD.RoleId IN(3)
			LEFT JOIN OprUsers AS OU                WITH(NOLOCK)    ON  OU.Id=DAUD.UserId

	WHERE	CS.TC_UserId = @UserId
			AND ISNULL(CD.IsSurveyDone, 0) = 0
			AND ISNULL(CD.Status,0) <> 3
			AND ISNULL(CD.IsRejected,0) <> 1 -- Modified By Ruchira Patil on 17 March 2015
			AND ISNULL(CD.IsActive,1) = 1 -- Modified By Tejashree Patil on 13 March 2015
	GROUP BY D.Organization,C.Name, D.Id, D.Longitude, D.Lattitude, D.MobileNo, ISNULL(D.Address1,D.Address2),OU.RoleIds,OU.UserName,OU.PhoneNo,ISNULL(D.RCNotMandatory,0),
			 D.EmailId,D.AreaId,D.CityId

	SELECT	TOP 1 VersionId
	FROM	WA_AndroidAppVersions WITH(NOLOCK)
	WHERE	ApplicationType = @ApplicationType
	AND IsLatest = 1
	ORDER BY Id DESC

END