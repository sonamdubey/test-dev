IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ImportSellerInqValidInvalid]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ImportSellerInqValidInvalid]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 10th April,2013
-- Description:	This Proc Returns valid and invalid table for seller inquiries 
-- TC_ImportSellerInqValidInvalid 400 ,1265
-- =============================================
CREATE PROCEDURE [dbo].[TC_ImportSellerInqValidInvalid]
@UserId    BIGINT,
@BranchId  BIGINT
AS
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
			 S.Eagerness,
			 S.CarColor,
			 S.CarMileage, 
			 S.RegistrationNo,
			 S.TC_InquiryOtherSourceId AS OtherSource,
			 S.TC_InquirySourceId AS Source,
			 S.IsValid ,
			 CMO.CarMakeId AS MakeID, 
			 CMO.ID AS ModelId, 
			 S.IsNew 
				 FROM TC_ImportSellerInquiries S WITH(NOLOCK)
				 JOIN CarModels CMO WITH (NOLOCK) 
													 ON S.CarModel = CMO.Name 
				 INNER JOIN TC_Users U WITH (NOLOCK) 
													 ON S.UserId = U.Id 
				 WHERE  S.IsValid=1 
					AND S.IsDeleted=0 
					AND S.UserId=@UserId 
					AND S.BranchId=@BranchId   
		                            
		                   
		 SELECT 
			B.TC_ImportSellerInquiriesId, 
			B.Name, 
			B.Email,
			B.Mobile,
			B.Location,
			B.CarMake,
			B.CarModel,
			B.CarVersion, 
			B.Comments,
			B.Price,
			B.CarYear,
			B.TC_InquiryOtherSourceId AS OtherSource,
			B.TC_InquirySourceId AS Source,
			B.IsValid 
				  FROM TC_ImportSellerInquiries B WITH(NOLOCK) 
				  WHERE (B.IsValid=0 OR B.IsValid IS NULL)
				  AND    B.IsDeleted=0 
				  AND    B.UserId=@UserId 
				  AND    B.BranchId=@BranchId
END
