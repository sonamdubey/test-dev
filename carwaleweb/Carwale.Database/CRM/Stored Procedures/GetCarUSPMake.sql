IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetCarUSPMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetCarUSPMake]
GO

	



--Summary	: SELECE Active MAKE of CarUSP
--Author	: Dilip V. 01-Aug-2012

CREATE PROCEDURE [CRM].[GetCarUSPMake]
	
 AS
	
BEGIN
	SET NOCOUNT ON	
		
		SELECT DISTINCT CMA.ID,CMA.Name
		FROM CRM.CarUSP	CCU WITH(NOLOCK)
		INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMA.ID = CCU.MakeId
		WHERE CCU.IsActive = 1
		ORDER BY CMA.Name
END






