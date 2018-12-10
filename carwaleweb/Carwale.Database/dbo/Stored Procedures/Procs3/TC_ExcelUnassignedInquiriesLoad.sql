IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ExcelUnassignedInquiriesLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ExcelUnassignedInquiriesLoad]
GO
	-- ================================================================================================
-- Author:		Tejashree Patil
-- Create date: 15 May,2013
-- Description:	This Proc Returns unassigned inquiries depend on type imported from excel.
-- Modified By:	Tejashree Patil on 14 July 2013, Fetched details of new car inquiry based VWFormat.
-- TC_ExcelUnassignedInquiriesLoad 1,5,1,0,1,100
-- TC_ExcelUnassignedInquiriesLoad 203,1028,3,1,1,1,100
-- EXEC TC_ExcelUnassignedInquiriesLoad 243,5,3,0,0,1,100
-- Modified By : Tejashree Patil on 29 July 2013, Fetched BookingAmount of New Car inquiry.
-- Modified By : Tejashree Patil on 19 Feb 2014,Fetched details like DealerCode,LastName like.
-- Mofified By : Afrose on 11th November 2015, changed input parameter branchid and userid from bigint to int
-- ================================================================================================
CREATE PROCEDURE [dbo].[TC_ExcelUnassignedInquiriesLoad]
	@UserId    INT,
	@BranchId  INT,
	@InquiryType TINYINT,
	@ReqFromUnassignedPage BIT=0,
	@IsSpecialUser BIT=0,
	@FromIndex  INT=NULL, 
	@ToIndex  INT=NULL
AS
BEGIN
	IF(@InquiryType=1)--Buyer
	BEGIN
		WITH cte1 AS (
			SELECT
				 B.TC_ImportBuyerInquiriesId AS Id, 
				 B.Name, 
				 B.Email,
				 B.Mobile,
				 B.Location,
				 ( ISNULL(B.CarMake,'') +' '+ ISNULL(B.CarModel,'') + ' '+ISNULL(B.CarVersion,'')) AS CarDetails, 
				 '' AS AlternateCarDetails,
				 B.Comments, 
				 B.Price, 
				 B.CarYear, 
				 B.Eagerness,
				 B.TC_InquiryOtherSourceId AS OtherSource,
				 B.TC_InquirySourceId AS Source,
				 B.IsValid ,
				 B.IsNew ,
				 B.ExcelSheetId,
				 ROW_NUMBER() OVER( ORDER BY B.ExcelSheetId DESC, B.EntryDate  )AS RowNo
					 FROM TC_ImportBuyerInquiries B WITH(NOLOCK)
					 --JOIN CarModels CMO WITH (NOLOCK) 
					 --								 ON B.CarModel = CMO.Name 
					 INNER JOIN TC_Users U WITH (NOLOCK) 
														 ON B.UserId = U.Id 
					 WHERE  B.IsValid=ISNULL(@ReqFromUnassignedPage,0)
						AND B.IsDeleted=0 
						AND B.UserId=@UserId 
						AND B.BranchId=@BranchId 
						AND B.TC_BuyerInquiriesId IS NULL)
				--SELECT * FROM cte1 WHERE RowNo BETWEEN @FromIndex AND @ToIndex;
				SELECT * 
				INTO   #TblTemp 
				FROM   cte1 		

				SELECT * 
				FROM   #TblTemp 
				WHERE  RowNo BETWEEN @FromIndex AND @ToIndex 
				ORDER BY RowNo 
	      

				SELECT COUNT(1) AS RecordCount 
				FROM   #TblTemp 
				
				DROP TABLE #TblTemp 		
	END
	IF(@InquiryType=2)--Seller
	BEGIN
		WITH cte1 AS (
			SELECT
				 S.TC_ImportSellerInquiriesId AS Id, 
				 S.Name, 
				 S.Email,
				 S.Mobile,
				 S.Location,
				 S.CarMake,
				 S.CarModel,
				 S.CarVersion,
				 ( ISNULL(S.CarMake,'') + ' ' + ISNULL(S.CarModel,'') + ' ' + ISNULL(S.CarVersion,'')) AS CarDetails, 
				 '' AS AlternateCarDetails,
				 S.Comments, 
				 S.Price, 
				 S.CarYear, 
				 S.Eagerness,
				 S.CarColor,
				 S.CarMileage, 
				 S.RegistrationNo,
				 S.TC_InquiryOtherSourceId AS OtherSource,
				 S.TC_InquirySourceId AS Source,
				 S.IsValid ,
				 --CMO.CarMakeId AS MakeID, 
				 --CMO.ID AS ModelId, 
				 S.IsNew ,
				 S.ExcelSheetId,
				 ROW_NUMBER() OVER( ORDER BY S.ExcelSheetId DESC, S.EntryDate  )AS RowNo
					 FROM TC_ImportSellerInquiries S WITH(NOLOCK)
					 --JOIN CarModels CMO WITH (NOLOCK) 
						--								 ON S.CarModel = CMO.Name 
					 INNER JOIN TC_Users U WITH (NOLOCK) 
														 ON S.UserId = U.Id 
					 WHERE  S.IsValid=ISNULL(@ReqFromUnassignedPage,0)
						AND S.IsDeleted=0 
						AND S.UserId=@UserId 
						AND S.BranchId=@BranchId 
						AND S.TC_SellerInquiriesId IS NULL)  
				--SELECT * FROM cte1 WHERE RowNo BETWEEN @FromIndex AND @ToIndex;    
				SELECT * 
				INTO   #TblTemp1 
				FROM   cte1 	

				SELECT * 
				FROM   #TblTemp1
				WHERE  RowNo BETWEEN @FromIndex AND @ToIndex
				ORDER BY RowNo  		      

				SELECT COUNT(1) AS RecordCount 
				FROM   #TblTemp1 		
				
				DROP TABLE #TblTemp1             
	END
	IF(@InquiryType=3)--New Car
	BEGIN
		IF @IsSpecialUser = 0
			SET @IsSpecialUser = NULL;

		WITH cte1 AS (
			SELECT
					 E.Id, 
					 E.Name as Name, 
					 E.Name as FirstName,
					 E.LastName as LastName,-- Modified By : Tejashree Patil on 19 Feb 2014
					 E.Email as Email,
					 E.Mobile,
					 E.City AS Location,
					 E.City AS City,
					 E.CarMake,
					 E.CarModel,
					 E.VersionId,
					 E.CarVersion,
					 ( ISNULL(E.CarMake,'') + ' ' + ISNULL(E.CarModel,'')+ ' ' 
					 + ISNULL(E.CarVersion,'')) AS CarDetails,
					 ( ISNULL(E.CarMake,'') + ' ' + ISNULL(E.AlternateModel,'')+ ' ' 
					 + ISNULL(E.AlternateCarVersion,'')) AS AlternateCarDetails,
					 E.TC_InquirySourceId AS OtherSource,
					 E.TC_InquirySourceId AS Source,
					 E.IsValid ,
					 '--' AS Price,
					 '--' AS CarYear,
					 E.Comments AS Comments,
					 --CMO.CarMakeId AS MakeID, 
					 --CMO.ID AS ModelId, 
					 E.IsNew,
					 E.SalesConsultant,
					 E.SalesConsultantId,
					 E.InquirySource,
					 E.InquiryDate,
					 E.Eagerness,
					 E.BuyingTime,
					 E.IsEligibleForCorporate,
					 E.CompanyName,
					 E.CompanyId,
					 E.AlternateModel,
					 E.AlternateCarVersion,
					 E.AlternateVersionId,
					 E.AlternateVersionColour,
					 E.AlternateVersionColourIds,
					 E.PreferedVersionColour,
					 E.PreferedVersionColourIds,
					 E.IsTestDriveRequested,
					 E.TestDriveDate,
					 E.TDStatus,
					 E.TDStatusId,
					 E.IsCarBooked,
					 E.BookingDate,
					 E.BookingAmount,-- Modified By : Tejashree Patil on 29 July 2013,
					 E.IsCarDelivered,
					 E.TentativeDeliveryDate,
					 E.DeliveryDate,
					 E.IsLeadLost,
					 E.LeadLostReason,
					 E.LostDispositionId,
					 E.NextFollowUpDate,
					 E.DealerCode,
					 E.RecentComment,
					 E.ActivityFeed,
					 E.ExcelSheetId,
				 ROW_NUMBER() OVER( ORDER BY E.ExcelSheetId DESC, E.EntryDate  )AS RowNo 
					 FROM TC_ExcelInquiries E WITH(NOLOCK)
					 LEFT JOIN TC_Users U WITH (NOLOCK) 
														 ON E.UserId = U.Id -- Modified By : Tejashree Patil on 19 Feb 2014, LEFT JOIN instead of INNER JOIN.
					 WHERE  E.IsValid=ISNULL(@ReqFromUnassignedPage,0)
						AND E.IsDeleted=0 
						--AND (E.UserId=@UserId)-- Modified By : Tejashree Patil on 19 Feb 2014, Commented
						AND ((@IsSpecialUser IS NULL AND  E.BranchId=@BranchId) OR (@IsSpecialUser IS NOT NULL AND E.IsSpecialUser=@IsSpecialUser))
						AND E.TC_NewCarInquiriesId IS NULL)

				--SELECT * FROM cte1 WHERE RowNo BETWEEN @FromIndex AND @ToIndex;   
				SELECT * 
				INTO   #TblTemp2 
				FROM   cte1 		

				SELECT * 
				FROM   #TblTemp2 
				WHERE  RowNo BETWEEN @FromIndex AND @ToIndex 	
				ORDER BY RowNo 
	      

				SELECT COUNT(1) AS RecordCount 
				FROM   #TblTemp2 	
				
				DROP TABLE #TblTemp2            
	END	
	
END
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

