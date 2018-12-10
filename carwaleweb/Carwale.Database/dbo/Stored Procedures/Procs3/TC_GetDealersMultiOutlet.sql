IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealersMultiOutlet]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealersMultiOutlet]
GO
	
-- =============================================
-- Author:		<vivek rajak>
-- Create date: <20/05/2015>
-- Description:	<To insert distinct dropdown delaers>
-- Modifier    : Ajay Singh (2 feb 2016)
-- Description : To add a condition of group and multioutlet
-- Modifier : Amit Yadav (12the Feb 2016)
--Purpose : Added distinct and condition to check if the dealer is inacitve in TC_dealerAdmin
-- EXEC [TC_GetDealersMultiOutlet] 1,1
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealersMultiOutlet]
@CityId VARCHAR(20)	,
@ApplicationId VARCHAR(20)
AS
BEGIN

	SELECT DISTINCT D.ID AS Value, (D.Organization + '-' + CONVERT(VARCHAR, D.ID)) AS Text FROM Dealers D WITH(NOLOCK)
			LEFT JOIN TC_DealerAdmin TD WITH(NOLOCK) ON TD.DealerId = D.ID
    WHERE CityID = @CityId AND D.Organization IS NOT NULL AND D.Organization <> '' 
		--AND TD.DealerId IS NULL
		and IsDealerActive = 1 and IsTCDealer = 1
	 	  AND D.ApplicationId = @ApplicationId
		  AND D.IsMultiOutlet = 0
		  AND D.IsGroup=0
		  AND D.ID NOT IN(SELECT MP.DealerId from  TC_DealerAdminMapping MP WITH(NOLOCK) JOIN TC_DealerAdmin TAD WITH(NOLOCK) ON MP.DealerAdminId = TAD.Id AND ISNULL(TAD.IsActive,0) = 1)		  
	ORDER BY Text 

END

 
--------------------------------------------------------------------------------------------------


