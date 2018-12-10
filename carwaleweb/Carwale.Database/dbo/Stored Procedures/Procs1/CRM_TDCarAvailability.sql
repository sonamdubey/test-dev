IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_TDCarAvailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_TDCarAvailability]
GO

	CREATE PROCEDURE [dbo].[CRM_TDCarAvailability]
--Name of SP/Function                    : CRM_TDCarAvailability
--Applications using SP                  : DealerPanel
--Modules using the SP                   : TDCarAvailability.cs
--Technical department                   : Database
--Summary                                : TD Car Availability
--Author                                 : AMIT Kumar 28th jan 2014
--Modification history                   : 1. Added @TdAt
--										 : 2. Amit Kumar (14 feb 2014) Added removed blocklistd car

@dealerId		VARCHAR(MAX)=NULL,
@type			INT,
@ModelId		VarChar(150) =NULL,
@FuelType		VarChar(150)= NULL,
@TransmissionType VarChar(150)=NULL,
@TdAt			Varchar (50)=NULL

AS
BEGIN
	IF(@type=1)-- for select statement
		BEGIN
			SELECT DISTINCT CM.id AS CarModelId,CM.Name AS CarModelName,CMK.Name AS MakeName,CV.CarFuelType ,CV.CarTransmission,ND.Name AS DealerShip,
				CFT.FuelType AS FuelType,CT.Descr AS Transmission, NDM.DealerId,CONVERT(VARCHAR(20),CDTA.CreatedOn,100) AS LastUpdated, DateDiff(dd,CDTA.CreatedOn,getdate()) AS UpdatedSince
			FROM CarModels CM WITH(NOLOCK) 
				INNER JOIN CarMakes CMK WITH(NOLOCK) ON CMK.Id=CM.CarMakeId --AND CM.Id IN(SELECT CDAC.AvaliableModelId FROM CRM_DLS_AvailableCars CDAC WITH(NOLOCK))
				INNER JOIN CarVersions CV WITH(NOLOCK) ON CM.ID = CV.CarModelId AND CV.New=1 
				INNER JOIN CarFuelType CFT WITH(NOLOCK) ON CFT.FuelTypeId=CV.CarFuelType
				INNER JOIN NCS_DealerMakes NDM WITH(NOLOCK) ON CMK.ID = NDM.MakeId
				INNER JOIN CarTransmission CT WITH(NOLOCK) ON CT.Id = CV.CarTransmission
				INNER JOIN NCS_Dealers ND WITH(NOLOCK) ON ND.ID = NDM.DealerId
				LEFT JOIN CRM_DLS_TDCarAvailability CDTA WITH(NOLOCK) ON  CDTA.DealerId = NDM.DealerId 
				AND CDTA.ModelId = CM.Id AND CDTA.FuelType=CFT.FuelTypeId AND CDTA.TransmissionType = CT.Id
			WHERE NDM.DealerId IN (SELECT ListMember FROM  fnSplitCSV(@dealerId)) AND CM.New = 1 
			ORDER BY CMK.Name, LastUpdated DESC, CM.Name
		END
	IF(@type=2)-- for insert statement
		BEGIN
			INSERT INTO CRM_DLS_TDCarAvailability(ModelId,DealerId,FuelType,TransmissionType,DesiredPlace)
			VALUES(@ModelId,@dealerId,@FuelType,@TransmissionType,@TdAt)

			IF(@@ROWCOUNT <> 0)
				BEGIN
					INSERT INTO CRM_DLS_TDCarAvailabilityLog(CreatedByDealerId,IsInsert,ModelId,FuelType,TransmissionType)
					VALUES(@dealerId,1,@ModelId,@FuelType,@TransmissionType)
				END

		END

	IF(@type=3)-- for DELETE statement
		BEGIN
			DELETE FROM CRM_DLS_TDCarAvailability WHERE ModelId=@ModelId AND DealerId=@dealerId AND FuelType=@FuelType AND TransmissionType = @TransmissionType
			
			IF(@@ROWCOUNT <> 0)
				BEGIN
					INSERT INTO CRM_DLS_TDCarAvailabilityLog(CreatedByDealerId,IsDelete,ModelId,FuelType,TransmissionType)
					VALUES(@dealerId,1,@ModelId,@FuelType,@TransmissionType)
				END 
		END

	IF(@type=4)-- Get Data to prefill the checkbox
		BEGIN
			SELECT * FROM CRM_DLS_TDCarAvailability WITH(NOLOCK) WHERE DealerId IN (SELECT ListMember FROM  fnSplitCSV(@dealerId))
		END
END
