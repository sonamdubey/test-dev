IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetDealersList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetDealersList]
GO
	
-- =============================================
-- Author:		Kartik Rathod
-- Create date: 23 Mar 2016
-- Description:	Gives the Name and Id of dealer on the basis of ApplicationId eg.Carwale and CityId
-- Modified By : Mihir Chheda On 20th June 2016 , get dealer list based on dealerId and dealerName.
-- Modified By : Mihir Chheda On 23rd June 2016 , left join on Areas table. 
-- Modified By : Komal Manjare On 23rd June 2016,isdeleted condition for cities and areas table
-- Modified By:Komal Manjare on 25 July 2016 ,Isdeleted condition for dealers insteadt of status check
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetDealersList]
	@ApplicationId SMALLINT = NULL,
	@CityId INT = NULL,
	@DealerType INT = NULL,
	@DealerId INT = NULL,			-- Mihir Chheda On 20th June 2016 
	@DealerName VARCHAR(250)=NULL	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	D.ID AS DealerId,
			D.Organization +' ('+CONVERT(VARCHAR,D.ID)+')' AS DealerName,
			D.ApplicationId,
			D.TC_DealerTypeId AS DealerType,	
			C.Name 	AS City	,
			A.Name  AS Area ,
			D.PhoneNo AS PhoneNumber,
			D.MobileNo AS MobileNumber,
			OU.UserName AS SalesFieldExecutiveName,
			D.PanNumber,D.TanNumber,D.LegalName,
			CASE WHEN (D.TanNumber IS NULL OR D.PanNumber IS NULL OR D.LegalName IS NULL)  THEN 0 -- only for NCD and NCD -UCD
			ELSE 1 END AS IsLegal
	FROM Dealers D (NOLOCK)
	JOIN Cities  C (NOLOCK) ON C.Id=D.CityId AND C.IsDeleted=0					-- Komal Manjare On 23rd June 2016
	LEFT JOIN Areas   A (NOLOCK) ON A.Id=D.AreaId AND A.IsDeleted=0				-- Mihir Chheda On 23rd June 2016
	JOIN DCRM_ADM_UserDealers DAUD  (NOLOCK) ON D.Id=DAUD.DealerId
	JOIN OprUsers OU (NOLOCK) ON OU.Id=DAUD.UserId
	WHERE 
		--D.Status = 0 
		D.IsDealerDeleted=0   --Komal Manjare on 25 July 2016 ,Isdeleted condition for dealers insteadt of status check
		AND (@ApplicationId IS NULL OR D.ApplicationId = @ApplicationId) 
		AND (@CityId IS NULL OR D.CityId = @CityId )
		AND (@DealerType IS NULL OR D.TC_DealerTypeId = @DealerType )
		AND (@DealerId IS NULL OR D.ID =@DealerId)
		AND (@DealerName IS NULL OR D.Organization LIKE ('%' + @DealerName + '%'))					-- Mihir Chheda On 20th June 2016 
		AND  DAUD.RoleId=3
	ORDER BY Organization
END

