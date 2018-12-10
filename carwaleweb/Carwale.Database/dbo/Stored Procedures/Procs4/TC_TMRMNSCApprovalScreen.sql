IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMRMNSCApprovalScreen]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMRMNSCApprovalScreen]
GO

	-- =================================================================
-- Author	    :	Vinayk Patil
-- Create date	:	27-11-2013
-- Description	:	Get Model & Dealer wise data For RM for Approval
-- ==================================================================

CREATE PROCEDURE [dbo].[TC_TMRMNSCApprovalScreen]
@TC_AMId INT=NULL 

AS 
 
 BEGIN

    SELECT      D.Id  FieldId ,D.Organization FieldName ,M.Id  CarId ,M.Name  CarName , 
	            ROUND(SUM (Target),0) AS Target
				FROM [TC_TMAMTargetChangeApprovalReq] AS I WITH(NOLOCK)
				JOIN Dealers           AS D WITH(NOLOCK) ON I.DealerId=D.Id
				JOIN CarVersions       AS V WITH(NOLOCK) ON V.Id=I.CarVersionId
				JOIN TC_SpecialUsers   AS S WITH(NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
				JOIN TC_BrandZone      AS TCB WITH(NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				JOIN CarModels         AS M WITH(NOLOCK) ON M.Id=V.CarModelId
				WHERE 
					I.TC_AMId = @TC_AMId 
				GROUP BY D.Id,D.Organization,M.Id,M.Name
				ORDER BY D.ID,M.ID

 END
