IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SyndicationXMLFeedTATACapital]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SyndicationXMLFeedTATACapital]
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
-- =============================================
CREATE PROCEDURE [dbo].[TC_SyndicationXMLFeedTATACapital]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--Table[1]: Retrives All Syndication Websites Available/ Only TATA Capital
    SELECT S.TC_SyndicationWebsiteId,S.WebsiteFileName FROM TC_SyndicationWebsite S WITH(NOLOCK) WHERE TC_SyndicationWebsiteId = 11
    
	--Table[2]: Retrives All Dealer Details Modified By: Nilesh Utture on 17 Sept 2012 on 6pm
	SELECT Lst.DealerId, Lst.ProfileId, Lst.MakeId, MakeName, Lst.ModelId, ModelName, Lst.VersionId, VersionName, lst.CityId, lst.CityName, lst.CityName, 
			lst.StateId, lst.StateName, Lst.MakeYear, Lst.Price, Lst.Kilometers, Lst.Color, lst.LastUpdated, Lst.Comments, lst.PhotoCount, lst.CertificationId,
			lst.CertifiedLogoUrl, lst.Owners  AS NoOwners, lst.SortScore, lst.VideoCount, lst.AdditionalFuel, 
			CASE lst.OriginalImgPath WHEN NULL THEN '' ELSE (lst.HostURL + '310X174' + lst.OriginalImgPath) END AS ImageUrlMedium, 
			CASE lst.OriginalImgPath WHEN NULL THEN '' ELSE (lst.HostURL + '110X61' + lst.OriginalImgPath) END AS FrontImagePath,
		   CASE Vs.CarFuelType WHEN 1 THEN 'Petrol' WHEN 2 THEN 'Diesel' WHEN 3 THEN 'CNG' WHEN 4 THEN 'LPG' WHEN 5 THEN 'Electric' END AS CarFuelType,
		   Lst.RootId, Lst.RootName     
    FROM	LiveListings Lst WITH(NOLOCK) 
			INNER JOIN CarVersions Vs WITH(NOLOCK)ON Lst.VersionId=Vs.ID
			  
       WHERE Lst.SellerType=1 AND lst.DealerId NOT IN(SELECT BranchId FROM TC_SyndicationDealer WHERE TC_SyndicationWebsiteId = 11 AND IsActive = 1)
		AND DATEDIFF(MM,Lst.MakeYear, GETDATE()) <= 96 --Should not me more than 8 year old
		AND lst.CityId IN(SELECT ID FROM Cities WHERE Name IN ('Agra','Dehradun','New Delhi','Durgapur','Guwahati','Kanpur','Karnal','Kolkata','Lucknow','Amritsar','Chandigarh','Jaipur','Jalandhar','Jodhpur','Kota','Ludhiana','Patiala','Udaipur','Bangalore','Kozhikode','Chennai','Coimbatore','Hubli','Hyderabad','Kochi','Madurai','Mangalore','Pondicherry','Salem','Trichy','Vishakhapatnam','Bhubaneshwar','Bilaspur','Goa','Indore','Jamshedpur','Mumbai and Thane','Raipur','Ahmedabad','Anand','Aurangabad','Kolhapur','Nagpur','Nashik','Pune','Rajkot','Surat','Vapi','Kottayam','Vadodara','mumbai','thane', 'noida', 'gurgaon') AND IsDeleted=0)
	
END


