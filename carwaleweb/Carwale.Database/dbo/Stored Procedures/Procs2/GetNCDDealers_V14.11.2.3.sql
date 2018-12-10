IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCDDealers_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCDDealers_V14]
GO

	-- =============================================          
-- Author:  <Raghu>          
-- Description: <Get the car dealers>          
-- Tables Used : Dealer_NewCar,NCD_Dealers,Cities,States,CarMakes          
-- Create By: Raghu on <01/08/2013>    
-- Modified by Raghu : <20/11/2013> used mobile column instead of dealermobno column 
-- Modified by Raghu : <5/12/2013> commented out Nd.IsActive Condition
-- Modified by Raghu : <5/12/2013> Sorting by IsPriority too Added IsPriority Desc  
-- Modified by Supriya : <10/6/2014> Added column IsPremium in select clause  
-- Modified by Vikas J : <27/06/2014> added order by NEWID() to randomise the dealer list    
-- Approved by: Manish Chourasiya on 01-07-2014 5:50 pm , With (NoLock) is used.
-- Avishkar Added 10-07-2014 DNC.IsNewDealer=1 
-- Vinayak Modified Contact details for dealers adding primary, secondary and landline number instead of earlier contact number field
-- =============================================          
CREATE PROCEDURE [dbo].[GetNCDDealers_V14.11.2.3] (
	@CityId SMALLINT
	,@MakeId SMALLINT
	)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DNC.TcDealerId AS Id
		,DNC.NAME AS DealerName
		,DNC.Address
		,DNC.PinCode
		--,DNC.ContactNo --modified by vinayak 5/11/2014 to replace the contact info with seperate primob,secmob,landline
		,DNC.PrimaryMobileNo
		,DNC.SecondaryMobileNo
		,DNC.LandLineNo
		,DNC.FaxNo
		,DNC.EMailId
		,DNC.WebSite
		,DNC.WorkingHours
		,'+91 ' + DNC.Mobile AS MobileNo
		,
		--Nd.IsPremium,      
		CMA.NAME AS CarMake
		,C.NAME AS City
		,S.NAME AS STATE
		,NCD_Website
		,Nd.Lattitude AS Latitude
		,Nd.Longitude AS Longitude
		,1 AS IsPremium
	FROM Dealer_NewCar AS DNC WITH (NOLOCK)
	LEFT JOIN NCD_Dealers Nd WITH (NOLOCK) ON DNC.Id = Nd.DealerId
	--AND Nd.IsActive =1   Commented by Raghu   
	JOIN Cities AS C  WITH (NOLOCK) ON DNC.CityId = C.ID
	JOIN States AS S WITH (NOLOCK) ON C.StateId = S.Id
	JOIN CarMakes AS CMA  WITH (NOLOCK) ON DNC.MakeId = CMA.ID
	WHERE DNC.CityId = @CityId
		AND DNC.MakeId = @MakeId
		AND DNC.IsActive = 1
		AND C.IsDeleted = 0
		AND S.IsDeleted = 0
		AND CMA.IsDeleted = 0
		--AND Nd.IsActive =1 AND Nd.IsPanelOnly = 0
		AND Nd.IsPremium = 1
		AND DNC.IsNewDealer=1 -- Avishkar Modified 10-07-2014
	--ORDER BY DealerName 
	ORDER BY NEWID() -- modified by vikas on 27-Jun-2014 to randomise the dealer list

	SELECT DNC.TcDealerId AS Id
		,DNC.NAME AS DealerName
		,DNC.Address
		,DNC.PinCode
		--,DNC.ContactNo --modified by vinayak 5/11/2014 to replace the contact info with seperate primob,secmob,landline
		,DNC.PrimaryMobileNo
		,DNC.SecondaryMobileNo
		,DNC.LandLineNo
		,DNC.FaxNo
		,DNC.EMailId
		,DNC.WebSite
		,DNC.WorkingHours
		,DNC.DealerMobileNo AS MobileNo
		,CMA.NAME AS CarMake
		,C.NAME AS City
		,S.NAME AS STATE
		,NCD_Website
		,Nd.Lattitude AS Latitude
		,Nd.Longitude AS Longitude
		,0 AS IsPremium
	FROM Dealer_NewCar AS DNC WITH (NOLOCK)
	LEFT JOIN NCD_Dealers Nd WITH (NOLOCK) ON DNC.Id = Nd.DealerId
		AND Nd.IsActive = 1
	JOIN Cities AS C WITH (NOLOCK) ON DNC.CityId = C.ID
	JOIN States AS S WITH (NOLOCK)  ON C.StateId = S.Id
	JOIN CarMakes AS CMA  WITH (NOLOCK) ON DNC.MakeId = CMA.ID
	WHERE DNC.CityId = @CityId
		AND DNC.MakeId = @MakeId
		AND DNC.IsActive = 1
		AND C.IsDeleted = 0
		AND S.IsDeleted = 0
		AND CMA.IsDeleted = 0
		AND DNC.IsNewDealer=1 -- Avishkar Modified 10-07-2014
		--AND Nd.IsPanelOnly = 1
		AND (
			Nd.IsPremium IS NULL
			OR Nd.IsPremium = 0
			)
	ORDER BY DNC.IsPriority DESC
		,DealerName
END

