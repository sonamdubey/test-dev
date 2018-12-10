IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_CarDealerAssignmentSP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_CarDealerAssignmentSP]
GO

	
--Name of SP/Function                    : CarWale.CRM_CarDealerAssignmentTest
--Applications using SP                  : DealerAssignment
--Modules using the SP                   : DealerAssignment.cs
--Technical department                   : Database
--Summary                                : DealerAssignment
--Author                                 : AMIT Kumar 06-Aug-2013
--Modification history                   : 

CREATE PROCEDURE [dbo].[CRM_CarDealerAssignmentSP]
@To		DATETIME,
@From	DATETIME,
@Make	NUMERIC(18,0),
@strt 	INT,
@end	INT
AS
BEGIN
	WITH CTE AS 
	(
	  SELECT DISTINCT CC.Id,(CC.FirstName + ' ' + CC.LastName) AS CustomerName, CC.Email, (CC.Mobile + ',' + CC.Landline) AS ContactNo,
		  C.Name AS CityName,S.Name AS State,c.DefaultPinCode AS Pincode, ND.Name AS DealerName,ND.DealerCode AS DealerCode,OUDC.UserName DCName, C1.Name AS DealerCity, CMA.Name AS Make, CMO.Name AS Model, CV.Name AS Version,
		  CDA.Comments, OU.UserName, CDA.CreatedOn AS EventOn, CL.CreatedOn AS LDDate,LA.UserName SrcName, (CASE CII.ClosingProbability
		  WHEN 1 THEN 'Very High' WHEN 2 THEN 'High' WHEN 3 THEN 'Normal' WHEN 4 THEN 'Low' ELSE 'NA' END) AS CP,
		  NRM.Name AS DealerTL, NRR.Name AS PLName, CAC.Name AS CarCity,
		  NM1.Name AS RM1, NM2.Name AS RM2, NM3.Name AS RM3
		  , NDM.Name AS CRepresentative, CL.ID AS LeadId,
		  CAT.Name AS TeamName
      
      FROM (((((((((((((((((((((((((CRM_CarDealerAssignment AS CDA WITH(NOLOCK)

		  INNER JOIN CRM_CarBasicData AS CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id)
		  INNER JOIN CRM_Leads AS CL WITH(NOLOCK) ON CBD.LeadId = CL.Id)
		  INNER JOIN CRM_Customers AS CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id)
		  INNER JOIN NCS_Dealers AS ND WITH(NOLOCK) ON CDA.DealerId = ND.Id)
		  LEFT JOIN CRM_ADM_DCDealers AS CADD WITH(NOLOCK) ON ND.ID = CADD.DealerId)
		  LEFT JOIN OprUsers AS OUDC WITH(NOLOCK) ON CADD.DCID = OUDC.Id)
		  LEFT JOIN Cities AS C WITH(NOLOCK) ON CC.CityId = C.Id)
		  LEFT JOIN States AS S WITH(NOLOCK) ON S.ID = C.StateId)
		  LEFT JOIN CRM_ADM_Teams AS CAT WITH(NOLOCK) ON CAT.ID  = CL.Owner)
		  LEFT JOIN Cities AS C1 WITH(NOLOCK) ON ND.CityId = C1.Id)
		  LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON CDA.CreatedBy = OU.Id)
		  INNER JOIN CarVersions AS CV WITH(NOLOCK) ON CBD.VersionId = CV.Id)
		  INNER JOIN CarModels AS CMO WITH(NOLOCK) ON CV.CarModelId = CMO.Id)
		  INNER JOIN CarMakes AS CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.Id)
		  INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.Id = CII.LeadId)
		  LEFT JOIN NCS_RMDealers NRD WITH(NOLOCK) ON ND.ID = NRD.DealerId AND NRD.Type = 0)
		  LEFT JOIN NCS_RManagers NRM WITH(NOLOCK) ON NRD.RMId = NRM.Id)
		  LEFT JOIN NCS_RManagers NRR WITH(NOLOCK) ON NRM.ReportTo = NRR.Id)

		  LEFT JOIN NCS_RManagers NM1 WITH(NOLOCK) ON NRR.ReportTo = NM1.Id)
		  LEFT JOIN NCS_RManagers NM2 WITH(NOLOCK) ON NM1.ReportTo = NM2.Id)
		  LEFT JOIN NCS_RManagers NM3 WITH(NOLOCK) ON NM2.ReportTo = NM3.Id)
		  INNER JOIN Cities CAC WITH(NOLOCK) ON CBD.CityId =  CAC.ID) 
		  LEFT JOIN LA_Agencies AS LA WITH(NOLOCK) ON CC.Source = CAST(LA.Id AS VarChar))

		  LEFT JOIN NCS_RMDealers NDR WITH(NOLOCK) ON ND.ID = NDR.DealerId AND NDR.Type = 1) 
		  LEFT JOIN NCS_RManagers NDM WITH(NOLOCK) ON NDR.RMId = NDM.Id)  
      WHERE CDA.CreatedOn BETWEEN @From AND @To AND (@Make IS NULL OR CMA.Id = @Make )
    ) SELECT *,ROW_NUMBER()Over (Order by CustomerName) AS RowId INTO #MyTemp FROM CTE;
                   
    SELECT * FROM #MyTemp WHERE RowId BETWEEN @strt AND @end
    SELECT COUNT(RowId) AS TotalCount FROM #MyTemp
    
    DROP TABLE #MyTemp

END
