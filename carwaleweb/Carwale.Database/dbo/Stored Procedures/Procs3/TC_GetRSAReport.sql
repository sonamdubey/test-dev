IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetRSAReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetRSAReport]
GO
	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: 24-Sep-2014
-- Description: Get data to show on RSAReport page.
-- Modified By: Tejashree Patil on 2 Dec 2014, Changed condition of JOIN with TC_SoldRSAPackages.
-- Modifided By: Ashwini Dhamankar on Dec 9,2014 Fetched StartDate,EndDate,EntryDate and IsAccepted from TC_SoldRSAPackages
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetRSAReport]
	@BranchId		 INT,
	@FromIndex       INT, 
	@ToIndex         INT
AS
BEGIN
		
	WITH Cte1 
           AS (
				SELECT	P.Name AS PackageName,S.Name AS CustomerName,
				CASE WHEN S.IsAccepted = 1 THEN S.StartDate ELSE NULL END	AS ActivationDate,
				CASE WHEN S.IsAccepted = 1 THEN S.EndDate	ELSE NULL END	AS Expirydate,
				S.EntryDate,S.IsAccepted,     -- Modifided By: Ashwini Dhamankar on Dec 9,2014 Fetched StartDate,EndDate,EntryDate and IsAccepted from TC_SoldRSAPackages
				         
                      ROW_NUMBER() 
                        OVER (                                
                               ORDER BY 
							   S.EntryDate DESC  
							 ) RowNumber
				FROM	TC_AvailableRSAPackages RSA		WITH(NOLOCK)
						INNER JOIN Packages P			WITH(NOLOCK) ON P.Id=RSA.PackageId
						INNER JOIN TC_SoldRSAPackages S WITH(NOLOCK) ON S.TC_AvailableRSAPackagesId=RSA.Id AND S.BranchId= @BranchId
				WHERE	RSA.BranchId = @BranchId 
			   )
         
              SELECT *, ROW_NUMBER() OVER (ORDER BY EntryDate DESC) NumberForPaging
              INTO   #TblTemp 
              FROM   Cte1 
                
		      SELECT * 
		      FROM   #TblTemp 
		      WHERE  
			  (@FromIndex IS NULL AND @ToIndex IS NULL)
			  OR
			  (NumberForPaging  BETWEEN @FromIndex AND @ToIndex )

		      SELECT COUNT(*) AS RecordCount 
		      FROM   #TblTemp 

		      DROP TABLE #TblTemp 
END