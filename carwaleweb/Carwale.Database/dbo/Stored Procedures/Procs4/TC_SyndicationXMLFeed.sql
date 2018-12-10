IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SyndicationXMLFeed]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SyndicationXMLFeed]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 14 Aug 2012
-- Description:	To feed XML data for Syndication
-- Execute TC_SyndicationXMLFeed
-- Modified By: Tejashree Patil on 12 Sept 2012 on 5pm
-- Description: Retrive CarFuelType
-- Modified By: Nilesh Utture on 17 Sept 2012 on 6pm
-- Description: Made Changes So that only dealer stock is stored in XMl
-- Modified By :Surendra On 25/02/2013 Desc: removed underscore replace in thumburl
-- Modified By :Tejashree Patil on 23 Dec 2013 Desc: Commented WHERE clause to get individuals inquiry also for syndication.
-- Modified By :Supriya Bhide on 10 Sept 2015 Desc: Added RootName and AreaName for trovitIndia.
-- Modified By :Supriya Bhide on 06/04/2016, Added packagetype in select query.
-- Modified By :Supriya Bhide on 23/06/2016, Corrected image path of ModelXLargePic using OriginalImgPath
-- =============================================
CREATE PROCEDURE [dbo].[TC_SyndicationXMLFeed]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--Table[1]: Retrives All Syndication Websites Available
    SELECT S.TC_SyndicationWebsiteId,S.WebsiteFileName FROM TC_SyndicationWebsite S WITH(NOLOCK) WHERE S.IsActive=1 AND IsTesting = 0
	
	--Table[2]: Retrives All Dealer Ids 
	--SELECT DISTINCT(S.DealerId) FROM SellInquiries S WITH(NOLOCK) 
	--INNER JOIN LiveListings L WITH(NOLOCK) ON S.ID=L.Inquiryid AND L.SellerType=1
	
	--Table[3]: Retrives All Dealer Ids And Syndicate Websites  
	--SELECT S.TC_SyndicationWebsiteId,S.BranchId FROM TC_SyndicationDealer S WITH(NOLOCK) WHERE S.IsActive=1

	--Table[4]: Retrives All Dealer Details Modified By: Nilesh Utture on 17 Sept 2012 on 6pm
	SELECT Lst.PackageType	-- Modified By :Supriya Bhide on 06/04/2016
			,Lst.DealerId, Lst.ProfileId, MakeName, ModelName, VersionName, lst.CityName, lst.StateName, Lst.MakeYear,
		   Lst.Kilometers, Lst.Color, Lst.Price, Lst.RootName, Lst.AreaName,	-- Modified By :Supriya Bhide on 10 Sept 2015
		   CASE lst.OriginalImgPath WHEN NULL THEN '' ELSE (lst.HostURL + '160X89' + lst.OriginalImgPath) END thumbUrl, Lst.Comments,
		   --CASE Lst.SellerType WHEN 1 THEN SD.RegistrationPlace WHEN 2 THEN CSD.RegistrationPlace END AS RegPlace,
		   --CASE Lst.SellerType WHEN 1 THEN SD.Insurance WHEN 2 THEN CSD.Insurance END AS Ins,
		   --CASE Lst.SellerType WHEN 1 THEN SD.InsuranceExpiry WHEN 2 THEN CSD.InsuranceExpiry END AS InsDate,
		   lst.Owners  AS NoOwners, Lst.SellerType, '' AS RegPlace, '' AS Ins, '' AS InsDate,
		   CASE Vs.CarFuelType WHEN 1 THEN 'Petrol' WHEN 2 THEN 'Diesel' WHEN 3 THEN 'CNG' WHEN 4 THEN 'LPG' WHEN 5 THEN 'Electric' END AS CarFuelType,
		   CASE Lst.SellerType WHEN 1 THEN 'Dealer' WHEN 2 THEN 'Individual' END AS Seller, 
		   Ct.Descr AS Transmission, CB.Name AS BodyStyle, lst.CertificationId, lst.CertifiedLogoUrl, lst.AdditionalFuel, 
		   CASE lst.OriginalImgPath WHEN NULL THEN '' ELSE (lst.HostURL + '310X174' + lst.OriginalImgPath) END AS ImageUrlMedium, 
		   CASE lst.OriginalImgPath WHEN NULL THEN '' ELSE(lst.HostURL + '762X429' + lst.OriginalImgPath) END AS ImageUrlBig, Lst.LastUpdated,
		   Lst.SortScore, 
		   (SELECT TOP 1 CASE OriginalImgPath WHEN NULL THEN '' ELSE (HostURL + '0X0' + OriginalImgPath) END --Modified By Supriya Bhide on 23/06/2016
			FROM CarModels WITH(NOLOCK) WHERE RootId = Cm.RootId AND XLargePic IS NOT NULL  ORDER BY ID DESC) AS ModelXLargePic   
    FROM	LiveListings Lst WITH(NOLOCK) 
			INNER JOIN CarVersions Vs WITH(NOLOCK)ON Lst.VersionId=Vs.ID
			INNER JOIN CarModels Cm WITH(NOLOCK)ON Vs.CarModelId = Cm.ID
			INNER JOIN CarTransmission CT WITH(NOLOCK)ON CT.Id = vs.CarTransmission
			INNER JOIN CarBodyStyles CB WITH(NOLOCK)ON CB.ID = vs.BodyStyleId
			
			--LEFT JOIN	SellInquiries S WITH(NOLOCK) ON Lst.Inquiryid=S.ID AND Lst.SellerType = 1
			--LEFT JOIN	SellInquiriesDetails AS SD WITH(NOLOCK) ON Lst.Inquiryid = SD.SellInquiryId AND Lst.SellerType = 1   
			--LEFT JOIN	CustomerSellInquiries CS WITH(NOLOCK) ON Lst.Inquiryid=S.ID AND Lst.SellerType = 2
			--LEFT JOIN	CustomerSellInquiryDetails AS CSD WITH(NOLOCK) ON Lst.Inquiryid = SD.SellInquiryId AND Lst.SellerType = 2   
			  
      -- WHERE Lst.SellerType=1
	  -- Modified By :Tejashree Patil on 23 Dec 2013 Desc: Commented WHERE Clause to get individuals inquiry also for syndication.
	  -- Modified By :Deepak Tripathi on 17th Apr 2014 Desc: Commented Left Join, Stopped Sending Ins and Reg details
	
END

