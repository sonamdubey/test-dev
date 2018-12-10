IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealers_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealers_V15]
GO

	-- =============================================          
-- Author:  <Vikas Jyoty>          
-- Description: <Get the car dealers>          
-- Tables Used : Dealer_NewCar,NCD_Dealers,Cities,States,CarMakes          
-- Create By: Vikas J on <01/08/2013>          
-- Modified By: Avishkar on  <01/09/2013> to use WITH(NOLOCK)     
-- Modified By:Prashant Vishe on <22/04/2013> to  add condition for retrieving top 5 records  from query   
-- Modifiled By : Raghu on <16/7/2013> to get the DealerId      
-- Modifiled By : Raghu on <3/13/2014> Sorting by Ispriority Column too
-- Modified By : Vicky Lund, 05/11/2015, Removed dealer_newcar dependency
-- =============================================          
CREATE PROCEDURE [dbo].[GetNewCarDealers_V15.11.3] (
	@CityId SMALLINT
	,@MakeId SMALLINT
	,@Choice SMALLINT = 1 --TO ELIMINATE TOP 5 ON QUERY      
	)
AS
BEGIN
	SET NOCOUNT ON;

	IF (@Choice = 1)
	BEGIN
		SELECT D.ID AS Id
			,D.FirstName AS DealerName
			,D.Address1 AS [Address]
			,D.Pincode
			,D.MobileNo AS ContactNo
			,D.FaxNo
			,D.EmailId AS EMailId
			,D.WebsiteUrl
			,D.ContactHours AS WorkingHours
			,CMA.NAME AS CarMake
			,C.NAME AS City
			,S.NAME AS STATE
			,D.WebsiteUrl
		FROM Dealers D WITH (NOLOCK) --Modified By: Avishkar on  <01/09/2013> to use WITH(NOLOCK)
		INNER JOIN DealerLocatorConfiguration DLC WITH (NOLOCK) ON D.ID = DLC.DealerId
			AND DLC.IsLocatorActive = 1
		INNER JOIN Cities AS C WITH (NOLOCK) ON D.CityId = C.ID
		INNER JOIN States AS S WITH (NOLOCK) ON C.StateId = S.ID
		INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON D.ID = TDM.DealerId
		INNER JOIN CarMakes AS CMA WITH (NOLOCK) ON TDM.MakeId = CMA.ID
		WHERE D.CityId = @CityId
			AND TDM.MakeId = @MakeId
			AND D.IsDealerActive = 1
			--AND D.IsDealerDeleted = 0
			AND D.TC_DealerTypeId = 2
			AND C.IsDeleted = 0
			AND S.IsDeleted = 0
			AND CMA.IsDeleted = 0
		ORDER BY D.WebsiteUrl DESC
			,DealerName;
	END
	ELSE
	BEGIN
		SELECT TOP 5 D.ID AS Id
			,D.FirstName AS DealerName
			,D.Address1 AS [Address]
			,D.Pincode
			,D.MobileNo AS ContactNo
			,D.FaxNo
			,D.EmailId AS EMailId
			,D.WebsiteUrl
			,D.ContactHours AS WorkingHours
			,CMA.NAME AS CarMake
			,C.NAME AS City
			,S.NAME AS STATE
			,D.WebsiteUrl
		FROM Dealers D WITH (NOLOCK) --Modified By: Avishkar on  <01/09/2013> to use WITH(NOLOCK)          
		INNER JOIN DealerLocatorConfiguration DLC WITH (NOLOCK) ON D.ID = DLC.DealerId
			AND DLC.IsLocatorActive = 1
		INNER JOIN Cities AS C WITH (NOLOCK) ON D.CityId = C.ID
		INNER JOIN States AS S WITH (NOLOCK) ON C.StateId = S.ID
		INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON D.ID = TDM.DealerId
		INNER JOIN CarMakes AS CMA WITH (NOLOCK) ON TDM.MakeId = CMA.ID
		WHERE D.CityId = @CityId
			AND TDM.MakeId = @MakeId
			AND D.IsDealerActive = 1
			AND D.IsDealerDeleted = 0
			AND D.TC_DealerTypeId = 2
			AND C.IsDeleted = 0
			AND S.IsDeleted = 0
			AND CMA.IsDeleted = 0
		ORDER BY D.WebsiteUrl DESC
			,DealerName;
	END
END

