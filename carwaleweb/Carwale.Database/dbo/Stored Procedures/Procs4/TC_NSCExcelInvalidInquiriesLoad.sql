IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_NSCExcelInvalidInquiriesLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_NSCExcelInvalidInquiriesLoad]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 19 Feb 2014
-- Description:	This Proc Returns invalid inquiries of nsc imported excel.
-- =============================================
CREATE PROCEDURE [dbo].[TC_NSCExcelInvalidInquiriesLoad]
	@UserId    BIGINT,
	@FromIndex  INT, 
	@ToIndex  INT
AS
BEGIN
	WITH cte1 AS (
		SELECT
					E.Id, 
					E.BranchId, 
					E.Name as Name, 
					E.LastName as LastName,-- Modified By : Tejashree Patil on 19 Feb 2014
					E.Salutation,
					E.Email,
					E.Mobile,
					E.City AS City,
					E.CarMake,
					E.CarModel,
					E.VersionId,
					E.CarVersion,
					( ISNULL(E.CarMake,'') + ' ' + ISNULL(E.CarModel,'')+ ' ' 
					+ ISNULL(E.CarVersion,'')) AS CarDetails,
					E.InquirySource,
					E.TC_InquirySourceId AS Source,
					E.IsValid ,
					E.DealerCode,
					E.ExcelSheetId,
				ROW_NUMBER() OVER( ORDER BY E.ExcelSheetId DESC, E.EntryDate  )AS RowNo 
					FROM TC_ExcelInquiries E WITH(NOLOCK)
					LEFT JOIN TC_Users U WITH (NOLOCK) 
														ON E.UserId = U.Id -- Modified By : Tejashree Patil on 19 Feb 2014, LEFT JOIN instead of INNER JOIN.
					WHERE  E.IsValid=0
					AND E.IsDeleted=0
					AND E.UserId=@UserId
					AND E.TC_NewCarInquiriesId IS NULL)

			--SELECT * FROM cte1 WHERE RowNo BETWEEN @FromIndex AND @ToIndex;   
			SELECT * 
			INTO   #TblTemp2 
			FROM   cte1 		

			SELECT * 
			FROM   #TblTemp2 
			WHERE  RowNo BETWEEN @FromIndex AND @ToIndex 	
			ORDER BY RowNo 
	      

			SELECT COUNT(*) AS RecordCount 
			FROM   #TblTemp2 	
				
			DROP TABLE #TblTemp2            
	
END

