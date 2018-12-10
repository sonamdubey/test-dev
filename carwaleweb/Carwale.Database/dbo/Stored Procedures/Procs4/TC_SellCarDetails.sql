IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SellCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SellCarDetails]
GO

	-- =============================================
-- Created By  :  Tejashree Patil on 10 Oct 2012 at 4 pm
-- Description :  Add sell Car details
-- EXEC [TC_SellCarDetails] 4197,1266389
-- Modified By : Tejashree Patil on 6 Nov 2012 at 12 pm Description : Added SINQ.IsPurchased in SELECT CLAUSE
-- Modified By : Tejashree Patil on 28 Nov 2012 Description: Removed IsParkNSale column from SELECT clause
-- Modified By : Tejashree Patil on 31 Jan 2013 Description: Join with TC_InquiriesLead
-- Modified By : Ruchira Patil on 27th Apr 2016 Description: Join with CustomerSellInquiryDetails to fetch owners
-- =============================================
CREATE PROCEDURE [dbo].[TC_SellCarDetails]
	@BranchId BIGINT,
	@SellerInquiryId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- Modified By : Tejashree Patil on 28 Nov 2012 
	SELECT	SINQ.TC_SellerInquiriesId, CM.ID AS MakeId, CMO.ID AS ModelId, CV.ID AS VersionId, 
			CM.Name AS Make, CM.LogoUrl AS LogoUrl, CMO.Name AS Model, 
			CV.Name AS Version, CV.largePic AS CarLargePicUrl, SINQ.Price AS Price, 
			SINQ.Kms AS Kilometers, SINQ.MakeYear AS MakeYear, SINQ.Colour, SINQ.LastUpdatedDate, SINQ.RegNo,/* SINQ.IsParkNSale , */
			ISNULL(CP.ImageUrlThumb, '') As ImageUrlThumb,ISNULL(CP.ImageUrlFull, '') As ImageUrlFull, 
			
			
			CASE WHEN SINQ.CWInquiryId IS NOT NULL THEN ISNULL(CSD.Owners,0) ELSE ISNULL(SINQ.Owners,0)END AS Owners,
			SINQ.Insurance, SINQ.InsuranceExpiry, 
			SINQ.Tax Tax, SINQ.RegistrationPlace, 
			SINQ.InteriorColor, SINQ.InteriorColor, SINQ.CityMileage, 
			SINQ.AdditionalFuel, SINQ.CarDriven, SINQ.Accidental, SINQ.FloodAffected, 
			SINQ.Warranties, SINQ.Modifications, SINQ.Comments, SINQ.BatteryCondition, 
			SINQ.BrakesCondition, SINQ.ElectricalsCondition, 
			SINQ.EngineCondition, SINQ.ExteriorCondition, 
			SINQ.SeatsCondition, SINQ.SuspensionsCondition, 
			SINQ.TyresCondition, SINQ.OverallCondition, 
			SINQ.Features_SafetySecurity, SINQ.Features_Comfort, SINQ.Features_Others, 
			SINQ.InteriorCondition, SINQ.ACCondition, SINQ.IsPurchased,
			--Dealer details
			D.ContactEmail, D.Organization,D.MobileNo, D.WebsiteUrl,CP.DirectoryPath,CP.HostUrl,
			--Customer Details
			CD.CustomerName, CD.Email, CD.Mobile, CD.Location

	FROM	TC_SellerInquiries SINQ  WITH(NOLOCK)
			INNER JOIN TC_InquiriesLead INQL WITH(NOLOCK) ON SINQ.TC_InquiriesLeadId = INQL.TC_InquiriesLeadId
			INNER JOIN Dealers D WITH(NOLOCK) ON INQL.BranchId = D.Id 
			INNER JOIN TC_CustomerDetails CD WITH(NOLOCK) ON CD.Id = INQL.TC_CustomerId
			LEFT JOIN  TC_SellCarPhotos CP WITH(NOLOCK) ON CP.TC_SellerInquiriesId = SINQ.TC_SellerInquiriesId And CP.IsMain = 1 And CP.IsActive = 1 
			INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.Id = SINQ.CarVersionId 
			INNER JOIN CarModels CMO WITH(NOLOCK) ON CMO.Id = CV.CarModelId 
			INNER JOIN CarMakes CM WITH(NOLOCK) ON CM.Id = CMO.CarMakeId 
			LEFT JOIN CustomerSellInquiryDetails CSD WITH(NOLOCK) ON CSD.InquiryId = SINQ.CWInquiryId -- Ruchira Patil on 27th Apr 2016 Description: Join with CustomerSellInquiryDetails to fetch owners

	WHERE	SINQ.TC_SellerInquiriesId = @SellerInquiryId 
			AND INQL.BranchId=@BranchId    
END

--------------------------------------------------------------------------------------------------------------------------------------------------------

