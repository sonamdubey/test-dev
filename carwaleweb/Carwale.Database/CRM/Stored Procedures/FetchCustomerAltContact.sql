IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[FetchCustomerAltContact]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[FetchCustomerAltContact]
GO

	
--Summary	: Fetch Alternate Contact Details
--Author	: Dilip V. 08-Aug-2012

CREATE PROCEDURE [CRM].[FetchCustomerAltContact]
	@CutomerId NUMERIC(18,0)
AS
BEGIN

SET NOCOUNT ON
	SELECT CCA.FirstName,CCA.Email,CCA.MobileNo,CCA.CreatedOn
	FROM CRM_Customers AS CC WITH (NOLOCK)
		INNER JOIN CRM_CustomerAliases CCA WITH (NOLOCK) ON CC.Id = CCA.CRM_CustomerId
	WHERE CC.ID = @CutomerId 
	ORDER BY CCA.CreatedOn DESC

END

