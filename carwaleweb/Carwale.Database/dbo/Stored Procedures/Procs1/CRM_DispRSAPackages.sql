IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DispRSAPackages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DispRSAPackages]
GO

	-- ===============================================
-- Author:		Yuga Hatolkar
-- Create date: 13/11/14
-- Description:	Display Data From TC_SoldRSAPackages
-- EXEC [CRM_DispRSAPackages] '4/1/2014 12:00:00 AM','4/1/2015 12:00:00 AM',0,1,0,NULL,NULL
-- ===============================================
CREATE PROCEDURE [dbo].[CRM_DispRSAPackages]
@From DATETIME,
@To DATETIME,
@Pending BIT,
@Approved BIT,
@Rejected BIT,
@FromIndex INT = NULL, 
@ToIndex INT = NULL

AS
	BEGIN	
		 WITH CTE AS
		(
			SELECT DISTINCT VW.Make AS CarMake, VW.MakeId AS CarMakeId, VW.Model AS CarModel, VW.ModelId AS CarModelId,SP.Id AS RSAId,
			VW.Version AS CarVersion, VW.VersionId AS CarVersionId,
			SP.RegistrationNo AS RegistrationNo, DL.Organization AS Dealer, SP.Id AS TC_SoldRSAId,
			SP.Name AS Name, SP.MobileNo AS MobileNo, SP.Email AS Email,SP.EntryDate AS EntryDate,
			SP.CBDId AS CBDId, SP.IsAccepted AS IsAccepted,SP.MakeYear AS MakeYear, 
			SP.StartDate AS StartDate,SP.EndDate AS EndDate, SP.Comments AS Comments, 
			SP.UpdatedOn AS UpdatedOn, SP.UpdatedBy AS UpdatedBy, PK.Name AS PackageName, PK.Amount AS Amount
			FROM TC_SoldRSAPackages SP WITH(NOLOCK)		
			LEFT JOIN Dealers DL WITH(NOLOCK) ON	SP.BranchId = DL.ID
			LEFT JOIN TC_AvailableRSAPackages AP WITH(NOLOCK) ON SP.TC_AvailableRSAPackagesId = AP.Id
			LEFT JOIN Packages PK WITH(NOLOCK) ON AP.PackageId = PK.Id	
			LEFT JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON SP.CBDId = CBD.ID 		
			LEFT JOIN vwMMV VW WITH(NOLOCK) ON SP.VersionId = VW.VersionId		
			--LEFT JOIN vwMMV as Vw1 WITH(NOLOCK) ON CBD.VersionId =  VW.VersionId 
			WHERE SP.EntryDate BETWEEN @from AND @to AND ISNULL(SP.IsActivated,0) <>1
		)
		SELECT * INTO #TblTemp
		FROM CTE

		IF(@Pending = 1)
		BEGIN		
			SELECT * 
			INTO #TblPageTemp
			FROM #TblTemp
			WHERE IsAccepted IS NULL

			SELECT *,ROW_NUMBER() OVER(ORDER BY EntryDate) AS NumberForPaging
			INTO #TblPagePending
			FROM #TblPageTemp

			SELECT * FROM #TblPagePending
			WHERE  
			(@FromIndex IS NULL AND @ToIndex IS NULL)
			OR
			(NumberForPaging  BETWEEN @FromIndex AND @ToIndex ) 

			SELECT COUNT(RSAId) AS RecordCount
			FROM   #TblPagePending  
			
			DROP TABLE #TblPagePending	
			DROP TABLE #TblPageTemp				
		END

		ELSE IF(@Approved = 1)
		BEGIN		
			SELECT * 
			INTO #TblPagerApproval
			FROM #TblTemp
			WHERE IsAccepted = 1

			SELECT *,ROW_NUMBER() OVER(ORDER BY EntryDate) AS NumberForPaging
			INTO #TblPageApproval
			FROM #TblPagerApproval

			SELECT * FROM #TblPageApproval
			WHERE  
			(@FromIndex IS NULL AND @ToIndex IS NULL)
			OR
			(NumberForPaging  BETWEEN @FromIndex AND @ToIndex ) 

			SELECT COUNT(RSAId) AS RecordCount
			FROM   #TblPageApproval  
			
			DROP TABLE #TblPageApproval	
			DROP TABLE #TblPagerApproval		
		END

		ELSE
		BEGIN	
			SELECT * 
			INTO #TblPager
			FROM #TblTemp
			WHERE IsAccepted <> 1

			SELECT *,ROW_NUMBER() OVER(ORDER BY EntryDate) AS NumberForPaging
			INTO #TblPageRejected
			FROM #TblPager

			SELECT * FROM #TblPageRejected
			WHERE  
			(@FromIndex IS NULL AND @ToIndex IS NULL)
			OR
			(NumberForPaging  BETWEEN @FromIndex AND @ToIndex ) 

			SELECT COUNT(RSAId) AS RecordCount
			FROM   #TblPageRejected  
			
			DROP TABLE #TblPageRejected	
			DROP TABLE #TblPager		
		END		

		DROP TABLE #TblTemp 
END
