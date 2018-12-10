IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetVerificationDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetVerificationDetails]
GO

	
-- Modified by Amit kumar on 4th jan 2013 in type=2.( added left join on CRM_CarCustomerServiceRequests )
-- Modified by Chetan Navin 24th May 2013 in type = 1(included HeadAgencyId)
-- Modified by Chetan Navin 18 Sep 2013 in type = 3 ( included salutation and current car)
-- Modified by Chetan Navin 21 Nov 2013 in type = 1 ( included customer city name , state name and areaId ) 
-- Modified by Ruchira Patil 24 Jan 2014 in type = 1 ( included ismulitiplebooking , booking count)
-- Modified by Chetan Navin 27 Jan 2014 (included stateId in type 2)
CREATE Proc [dbo].[CRM_GetVerificationDetails]        
	@LeadId AS NUMERIC,
	@Type AS SMALLINT
        
AS        
BEGIN   
     --Customer Details
     IF @Type = 1
		BEGIN
			SELECT CC.Salutation,CC.FirstName,CC.LastName, CC.Mobile, CC.Email, CC.Landline, CI.ID AS CityId,CII.ID AS IIId, CLS.SourceId,CI.Name AS CityName,S.Name AS StateName,
				S.ID AS StateId, CLS.SourceName AS SourceName, CL.LeadStageId, CL.ID AS LeadId,CII.IsMultipleBooking,CII.BookingCount, 
				CC.CWCustId, CL.Owner, LA.BColor, LA.FColor, CLS.CategoryId, CL.IsVisitedDealer, CFG.Name AS FLCGroupName, CFG.GroupType, CC.AreaId AS AreaId,ISNULL(LA.HeadAgencyId,1) AS HeadAgencyId
			FROM CRM_Customers AS CC WITH(NOLOCK), Cities AS CI WITH(NOLOCK), States AS S WITH(NOLOCK), CRM_InterestedIn CII WITH(NOLOCK), 
				CRM_Leads AS CL WITH(NOLOCK) LEFT JOIN CRM_LeadSource AS CLS WITH(NOLOCK) ON CL.ID = CLS.LeadId
				LEFT JOIN LA_Agencies AS LA WITH(NOLOCK) ON CLS.SourceId = LA.Id
				LEFT JOIN CRM_ADM_FLCGroups CFG WITH(NOLOCK) ON CL.GroupId = CFG.Id
			WHERE CC.CityId = CI.ID AND CI.StateId = S.ID AND CC.ID = CL.CNS_CustId
				AND CL.ID = @LeadId AND CL.Id = CII.LeadId AND CII.ProductTypeId = 1
		END
	ELSE --CarDetails
		IF @Type = 2
			BEGIN
				SELECT ISNULL(CVL.id, -1) AS Id, (CMA.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName,CMO.Name AS CarModel,
					CV.ID AS CarVersionId, CBD.ID AS CBDId, CVL.IsPDRequired, CBD.CreatedOn, CI.Name AS City,
					CVL.IsTDRequired, CVL.IsPQRequired, CVL.DealerId, CBD.CityId, CVL.Comments, CI.Id AS CityId,
					CMA.ID AS MakeId, CVL.DealerId, CV.ID AS VersionId, CMA.Name AS CarMake,CMO.MaskingName AS MaskingName,CV.Name as VersionName,
					NCSP.Price AS ExShowroom, NCSP.Insurance AS Insurance, NCSP.RTO AS RTO, 
					(ISNULL(NCSP.Price, 0) + ISNULL(NCSP.Insurance, 0) + ISNULL(NCSP.RTO, 0)) AS OnRoadPrice,
					CMO.ID AS ModelId, CBD.IsVisitedDealer, CCSR.TDRequest,CCSR.TDLocation,CCSR.TDDate ,CBD.ID,
					(CASE CBD.SourceCategory WHEN 3 THEN (CASE WHEN ISNULL(CBD.SourceId, 0) > 0 THEN CBD.SourceId ELSE 1 END) ELSE 1 END) AS SourceId,
					DATEDIFF(DD, CBD.CreatedOn, CBD.ExpectedBuyingDate) AS ExpBuyDay,CBD.IsDealerAssigned,CI.StateId AS StateId,CVL.IsOffer,CVL.IsFinance,CVL.IsUrgent
				FROM   
					CRM_CarBasicData AS CBD WITH(NOLOCK) 
					INNER JOIN CarVersions AS CV WITH(NOLOCK) ON CBD.VersionId = CV.ID
					INNER JOIN CarModels AS CMO WITH(NOLOCK) ON CV.CarModelId = CMO.ID	
					INNER JOIN CarMakes AS CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
					INNER JOIN CRM_ActiveItems AS CA WITH(NOLOCK) ON CBD.ID = CA.ItemId
					INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CII.LeadId = CBD.LeadId  
					LEFT JOIN CRM_VerificationLog AS CVL WITH(NOLOCK) ON CBD.ID = CVL.CBDId
					LEFT JOIN Cities AS CI WITH(NOLOCK) ON CBD.CityId = CI.ID
					LEFT JOIN CRM_CarCustomerServiceRequests CCSR WITH(NOLOCK) ON CCSR.CarBasicDataId = CBD.ID
					LEFT JOIN NewCarShowroomPrices NCSP WITH(NOLOCK) ON NCSP.CarVersionId = CBD.VersionId AND NCSP.CityId = CBD.CityId
				WHERE CBD.LeadId = @LeadId AND CA.InterestedInId = CII.ID
				ORDER BY CMO.ID,CVL.DealerId  DESC
				
			END
	ELSE
		IF @Type = 3
			BEGIN
				SELECT LeadId, DealerId, DealerName, PurchaseTime, Eagerness, IsPEDone, CPL.PurchaseOnName, 
						CPL.PurchaseOnNameType, CPL.PurchaseMode,CPL.CurrentCarOwned AS CurrentCar
				FROM CRM_VerificationOthersLog CPL WITH(NOLOCK)
				WHERE LeadId = @LeadId
			END
END  



