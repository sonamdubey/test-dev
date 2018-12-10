IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetTDAvailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetTDAvailability]
GO

	
--Summary	: Get Test drive availability
--Author	: Vinay Kumar Prajapati 7th feb 2014
--Modifier	: Vaibhav K 1 July 2014 (Added constraint for model condition as parameter was already passed)

CREATE PROCEDURE [dbo].[CRM_GetTDAvailability]
	@MakeId		       INT,
	@ModelId           INT = NULL,
	@StateId	       INT = NULL,
	@CityId		       INT = NULL,
	@DealerId          INT = NULL,
	@FuelType	       INT = NULL,
	@TransmissionType  INT = NULL
 AS
	
BEGIN	
	SELECT DISTINCT CDT.Id, ND.Name AS DealerName, CMO.Name AS Model,C.Name AS City,CFT.FuelType,CTT.Descr AS TransmissionType,CDT.CreatedOn 
	FROM 
		CRM_DLS_TDCarAvailability AS CDT WITH(NOLOCK) 
		INNER JOIN NCS_Dealers AS ND WITH(NOLOCK)  ON CDT.DealerId = ND.ID
		INNER JOIN NCS_DealerMakes AS NDM WITH(NOLOCK) ON ND.ID = NDM.DealerId
		INNER JOIN CarModels AS CMO WITH(NOLOCK) ON CDT.ModelId = CMO.ID AND NDM.MakeId = CMO.CarMakeId
		INNER JOIN Cities AS C WITH(NOLOCK) ON C.ID=Nd.CityId
		INNER JOIN CarFuelType AS CFT WITH(NOLOCK) ON CFT.FuelTypeId=CDT.FuelType
		INNER JOIN CarTransmission AS CTT WITH(NOLOCK) ON  CTT.Id=CDT.TransmissionType

	WHERE (@DealerId IS NULL OR CDT.DealerId = @DealerId)  AND 
		  (@CityId IS NULL OR ND.CityId = @CityId) AND
		  (@StateId IS NULL OR C.StateId = @StateId) AND
		  (@MakeId IS NULL OR NDM.MakeId = @MakeId) AND
		  (@ModelId IS NULL OR CMO.ID = @ModelId) AND
		  (@FuelType IS NULL OR CDT.FuelType = @FuelType) AND
		  (@TransmissionType IS NULL OR CDT.TransmissionType = @TransmissionType)

	ORDER BY ND.Name

END

