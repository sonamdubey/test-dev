IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQFromExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQFromExcel]
GO

	-- Created By:	Tejashree Patil
-- Create date: 18 May 2013
-- Description:	Get unassigned and invalid inquiries from excel.
-- =============================================
CREATE  PROCEDURE       [dbo].[TC_INQFromExcel]
( 
@BranchId BIGINT,
@UserId BIGINT,
@InquiryType SMALLINT,
@IsInvalidInquiry BIT
)		
AS 	
BEGIN
	IF (@InquiryType=1)
	BEGIN		
		IF(@IsInvalidInquiry=1)
		BEGIN
			SELECT
			 B.TC_ImportBuyerInquiriesId, 
			 B.Name, 
			 B.Email,
			 B.Mobile,
			 B.Location,
			 B.CarDetails, 
			 B.Comments, 
			 B.Price, 
			 B.CarYear
				 FROM TC_ImportBuyerInquiries B WITH(NOLOCK)
				 INNER JOIN TC_Users U WITH (NOLOCK) 
													 ON B.UserId = U.Id 
				 WHERE  B.IsValid=1 
					AND B.IsDeleted=0 
					AND B.UserId=@UserId 
					AND B.BranchId=@BranchId 
					AND B.TC_BuyerInquiriesId IS NULL
					AND B.LeadOwnerId IS NULL 
		END
		ELSE
		BEGIN
			SELECT
			 B.TC_ImportBuyerInquiriesId, 
			 B.Name, 
			 B.Email,
			 B.Mobile,
			 B.Location,
			 B.CarDetails, 
			 B.Comments, 
			 B.Price, 
			 B.CarYear
				 FROM TC_ImportBuyerInquiries B WITH(NOLOCK)
				 INNER JOIN TC_Users U WITH (NOLOCK) 
													 ON B.UserId = U.Id 
				 WHERE  (B.IsValid IS NULL OR B.IsValid=0) 
					AND B.IsDeleted=0 
					AND B.UserId=@UserId 
					AND B.BranchId=@BranchId 
					AND B.TC_BuyerInquiriesId IS NULL
					AND B.LeadOwnerId IS NULL 
		END
	END	    
	ELSE 
	IF (@InquiryType=2)
	BEGIN
		IF(@IsInvalidInquiry=1)
		BEGIN
			SELECT
			 S.TC_ImportSellerInquiriesId, 
			 S.Name, 
			 S.Email,
			 S.Mobile,
			 S.Location,
			 ( ISNULL(S.CarMake,'') + ISNULL(S.CarModel,'') + ISNULL(S.CarVersion,'')) AS CarDetails, 
			 S.Comments, 
			 S.Price, 
			 S.CarYear,
			 CMO.CarMakeId AS MakeID, 
			 CMO.ID AS ModelId
				 FROM TC_ImportSellerInquiries S WITH(NOLOCK)
				 JOIN CarModels CMO WITH (NOLOCK) 
													 ON S.CarModel = CMO.Name 
				 INNER JOIN TC_Users U WITH (NOLOCK) 
													 ON S.UserId = U.Id 
				 WHERE  S.IsValid=1 
					AND S.IsDeleted=0 
					AND S.UserId=@UserId 
					AND S.BranchId=@BranchId 
					AND S.TC_SellerInquiriesId IS NULL
					AND S.LeadOwnerId IS NULL 
		END
		ELSE
		BEGIN
			SELECT
			 S.TC_ImportSellerInquiriesId, 
			 S.Name, 
			 S.Email,
			 S.Mobile,
			 S.Location,
			 ( ISNULL(S.CarMake,'') + ISNULL(S.CarModel,'') + ISNULL(S.CarVersion,'')) AS CarDetails, 
			 S.Comments, 
			 S.Price, 
			 S.CarYear,
			 CMO.CarMakeId AS MakeID, 
			 CMO.ID AS ModelId
				 FROM TC_ImportSellerInquiries S WITH(NOLOCK)
				 JOIN CarModels CMO WITH (NOLOCK) 
													 ON S.CarModel = CMO.Name 
				 INNER JOIN TC_Users U WITH (NOLOCK) 
													 ON S.UserId = U.Id 
				 WHERE  (S.IsValid IS NULL OR S.IsValid=0)  
					AND S.IsDeleted=0 
					AND S.UserId=@UserId 
					AND S.BranchId=@BranchId 
					AND S.TC_SellerInquiriesId IS NULL
					AND S.LeadOwnerId IS NULL 
			END		
		END
	ELSE 
	IF (@InquiryType=3)
	BEGIN			
		IF(@IsInvalidInquiry=1)
		BEGIN
			SELECT
			 E.Id, 
			 E.Name, 
			 E.Email,
			 E.Mobile,
			 E.City,
			 ( ISNULL(E.CarMake,'') + ISNULL(E.CarModel,'')) AS CarDetails, 
			 E.Comments,
			 CMO.CarMakeId AS MakeID, 
			 CMO.ID AS ModelId
				 FROM TC_ExcelInquiries E WITH(NOLOCK)
				 JOIN CarModels CMO WITH (NOLOCK) 
													 ON E.CarModel = CMO.Name 
				 INNER JOIN TC_Users U WITH (NOLOCK) 
													 ON E.UserId = U.Id 
				 WHERE  E.IsValid=1 
					AND E.IsDeleted=0 
					AND E.UserId=@UserId 
					AND E.BranchId=@BranchId 
					AND E.TC_NewCarInquiriesId IS NULL
					AND LeadOwnerId IS NULL 
		
		END
		ELSE
		BEGIN
			SELECT
			 E.Id, 
			 E.Name, 
			 E.Email,
			 E.Mobile,
			 E.City,
			 ( ISNULL(E.CarMake,'') + ISNULL(E.CarModel,'')) AS CarDetails, 
			 E.Comments,
			 CMO.CarMakeId AS MakeID, 
			 CMO.ID AS ModelId
				 FROM TC_ExcelInquiries E WITH(NOLOCK)
				 JOIN CarModels CMO WITH (NOLOCK) 
													 ON E.CarModel = CMO.Name 
				 INNER JOIN TC_Users U WITH (NOLOCK) 
													 ON E.UserId = U.Id 
				 WHERE  (E.IsValid IS NULL OR E.IsValid=0) 
					AND E.IsDeleted=0 
					AND E.UserId=@UserId 
					AND E.BranchId=@BranchId 
					AND E.TC_NewCarInquiriesId IS NULL
					AND LeadOwnerId IS NULL 
		
		END
	END		  			
END
