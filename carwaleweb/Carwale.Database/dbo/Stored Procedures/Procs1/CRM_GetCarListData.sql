IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetCarListData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetCarListData]
GO

	-- Modifier : Chetan Navin - 15th June 2014 (Added CarDealerAssignment date)
CREATE PROC [dbo].[CRM_GetCarListData]        
@LeadId AS NUMERIC,
@ProductTypeId AS INT,
@dealerId AS Numeric
        
AS        
BEGIN   
     
    SELECT DISTINCT CII.Id AS InterestedInId, CBD.ID CarBasicDataId,  CSD.SubDisposition, VM.Car AS CarName,CBD.IsDeleted As IsDeleted,
		CBD.CreatedOn, CFM.Message,VM.ModelId ModelId, CBD.IsProductExplained, CBD.IsPQMailed, CBD.IsFinalized,
		0 AS BookingStatus, GETDATE() BookingDate, 0 AS DeliveryStatus, GETDATE() ExpectedDeliveryDate, GETDATE() ActualDeliveryDate, 
		0 TDStatus, GETDATE() TDRequestDate, GETDATE() TDCompletedDate,  0 PQRequested ,
		ISNULL(CDA.Id, -1) AS CDAStatus, 0 IsPDRequired, ISNULL(CBD.DeleteReasonId, -1) AS DeleteReason,CDA.CreatedOn AS DealerAssignmentDate
	--CBA.Name AS BookingStatus, 
	--CB.BookingDate, CIA.Name AS DeliveryStatus, CDD.ExpectedDeliveryDate, CDD.ActualDeliveryDate, 
	--TDS.Name AS TDStatus, CTD.TDRequestDate, CTD.TDCompletedDate, CBD.CreatedOn, CBD.IsPQMailExternalReq PQRequested ,
	--ISNULL(CDA.Id, -1) AS CDAStatus, CVL.IsPDRequired, CFM.Message,ISNULL(CBD.DeleteReasonId, -1) AS DeleteReason,
	
	FROM CRM_InterestedIn CII WITH (NOLOCK) 
	INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CII.LeadId = CBD.LeadId 
	INNER JOIN  vwMMV VM ON CBD.VersionId = VM.VersionId 
	LEFT JOIN CRM_CarDealerAssignment AS CDA WITH (NOLOCK) ON CBD.ID = CDA.CBDId --AND CDA.DealerId = @dealerId
	LEFT JOIN CRM_FLCMessage CFM WITH (NOLOCK) ON CFM.ModelId = VM.ModelId AND CFM.CityId = CBD.CityId
	LEFT JOIN CRM_SubDisposition CSD ON CDA.LatestStatus = CSD.Id
	--LEFT JOIN CRM_CarBookingData AS CB WITH (NOLOCK) ON CBD.Id = CB.CarBasicDataId 
	--LEFT JOIN CRM_EventTypes AS CBA WITH (NOLOCK) ON CB.BookingStatusId = CBA.Id 
	--LEFT JOIN CRM_CarDeliveryData AS CDD WITH (NOLOCK) ON CBD.Id = CDD.CarBasicDataId 
	--LEFT JOIN CRM_EventTypes AS CIA WITH (NOLOCK) ON CDD.DeliveryStatusId = CIA.Id 
	--LEFT JOIN CRM_CarTestDriveData AS CTD WITH (NOLOCK) ON CBD.Id = CTD.CarBasicDataId 
	--LEFT JOIN CRM_EventTypes AS TDS WITH (NOLOCK) ON CTD.TDStatusId = TDS.Id
	--LEFT JOIN CRM_VerificationLog CVL WITH (NOLOCK) ON CBD.ID = CVL.CBDId
	
	WHERE CII.LeadId = @LeadId AND CII.ProductTypeId =@ProductTypeId  
	ORDER BY CBD.CreatedOn DESC
		
	-- IF @dealerId <> -1 AND @dealerId <> 0
	--	BEGIN
	--		SELECT DISTINCT CII.Id AS InterestedInId, CBD.ID CarBasicDataId, CBD.IsProductExplained, CBD.IsPQMailed, 
	--		CBD.IsFinalized, (CMA.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName, CBA.Name AS BookingStatus, 
	--		CB.BookingDate, CIA.Name AS DeliveryStatus, CDD.ExpectedDeliveryDate, CDD.ActualDeliveryDate, 
	--		TDS.Name AS TDStatus, CTD.TDRequestDate, CTD.TDCompletedDate, CBD.CreatedOn, CBD.IsPQMailExternalReq PQRequested ,
	--		ISNULL(CDA.Id, -1) AS CDAStatus, CVL.IsPDRequired, CFM.Message,ISNULL(CBD.DeleteReasonId, -1) AS DeleteReason,
	--		CSD.SubDisposition
	--		FROM CRM_InterestedIn CII WITH (NOLOCK) 
	--		INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CII.LeadId = CBD.LeadId 
	--		INNER JOIN  CarVersions CV WITH (NOLOCK) ON CBD.VersionId = CV.ID 
	--		INNER JOIN  CarModels CMO WITH (NOLOCK) ON CV.CarModelId = CMO.ID 
	--		INNER JOIN  CarMakes CMA WITH (NOLOCK) ON CMO.CarMakeId = CMA.ID 
	--		LEFT JOIN CRM_CarBookingData AS CB WITH (NOLOCK) ON CBD.Id = CB.CarBasicDataId 
	--		LEFT JOIN CRM_EventTypes AS CBA WITH (NOLOCK) ON CB.BookingStatusId = CBA.Id 
	--		LEFT JOIN CRM_CarDeliveryData AS CDD WITH (NOLOCK) ON CBD.Id = CDD.CarBasicDataId 
	--		LEFT JOIN CRM_EventTypes AS CIA WITH (NOLOCK) ON CDD.DeliveryStatusId = CIA.Id 
	--		LEFT JOIN CRM_CarTestDriveData AS CTD WITH (NOLOCK) ON CBD.Id = CTD.CarBasicDataId 
	--		LEFT JOIN CRM_EventTypes AS TDS WITH (NOLOCK) ON CTD.TDStatusId = TDS.Id
	--		LEFT JOIN CRM_CarDealerAssignment AS CDA WITH (NOLOCK) ON CBD.ID = CDA.CBDId --AND CDA.DealerId = @dealerId
	--		LEFT JOIN CRM_VerificationLog CVL WITH (NOLOCK) ON CBD.ID = CVL.CBDId
	--		LEFT JOIN CRM_FLCMessage CFM WITH (NOLOCK) ON CFM.ModelId = CMO.ID AND CFM.CityId = CBD.CityId
	--		LEFT JOIN CRM_SubDisposition CSD ON CDA.LatestStatus = CSD.Id
	--		WHERE CII.LeadId = @LeadId AND CII.ProductTypeId =@ProductTypeId  
	--		ORDER BY CBD.CreatedOn DESC
	--	END
	--ELSE
	--	BEGIN
	--		SELECT DISTINCT CII.Id AS InterestedInId, CBD.ID CarBasicDataId, CBD.IsProductExplained, CBD.IsPQMailed, 
	--		CBD.IsFinalized, (CMA.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName, CBA.Name AS BookingStatus, 
	--		CB.BookingDate, CIA.Name AS DeliveryStatus, CDD.ExpectedDeliveryDate, CDD.ActualDeliveryDate, 
	--		TDS.Name AS TDStatus, CTD.TDRequestDate, CTD.TDCompletedDate, CBD.CreatedOn, CBD.IsPQMailExternalReq PQRequested ,
	--		ISNULL(CDA.Id, -1) AS CDAStatus, CVL.IsPDRequired, CMO.ID ModelId,CBD.CityId,CFM.Message,ISNULL(CBD.DeleteReasonId, -1) AS DeleteReason,
	--		CSD.SubDisposition
	--		FROM CRM_InterestedIn CII WITH (NOLOCK)
	--		INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CII.LeadId = CBD.LeadId 
	--		INNER JOIN  CarVersions CV WITH (NOLOCK) ON CBD.VersionId = CV.ID 
	--		INNER JOIN  CarModels CMO WITH (NOLOCK) ON CV.CarModelId = CMO.ID 
	--		INNER JOIN  CarMakes CMA WITH (NOLOCK) ON CMO.CarMakeId = CMA.ID 
	--		LEFT JOIN CRM_CarBookingData AS CB WITH (NOLOCK) ON CBD.Id = CB.CarBasicDataId 
	--		LEFT JOIN CRM_EventTypes AS CBA WITH (NOLOCK) ON CB.BookingStatusId = CBA.Id 
	--		LEFT JOIN CRM_CarDeliveryData AS CDD WITH (NOLOCK) ON CBD.Id = CDD.CarBasicDataId 
	--		LEFT JOIN CRM_EventTypes AS CIA WITH (NOLOCK) ON CDD.DeliveryStatusId = CIA.Id 
	--		LEFT JOIN CRM_CarTestDriveData AS CTD WITH (NOLOCK) ON CBD.Id = CTD.CarBasicDataId 
	--		LEFT JOIN CRM_EventTypes AS TDS WITH (NOLOCK) ON CTD.TDStatusId = TDS.Id
	--		LEFT JOIN CRM_CarDealerAssignment AS CDA WITH (NOLOCK) ON CBD.ID = CDA.CBDId
	--		LEFT JOIN CRM_SubDisposition CSD ON CDA.LatestStatus = CSD.Id
	--		LEFT JOIN CRM_VerificationLog CVL WITH (NOLOCK) ON CBD.ID = CVL.CBDId
	--		LEFT JOIN CRM_FLCMessage CFM WITH (NOLOCK) ON CFM.ModelId = CMO.ID AND CFM.CityId = CBD.CityId
	--		WHERE CII.LeadId = @LeadId AND CII.ProductTypeId =@ProductTypeId  
	--		ORDER BY CBD.CreatedOn DESC
 --        END 
END  


