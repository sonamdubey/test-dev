IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[PQRequest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[PQRequest]
GO

	


--Name of SP/Function				: CarWale.[CRM].[PQRequest]
--Applications using SP				: CRM
--Modules using the SP				: PQRequest.cs
--Technical department				: Database
--Summary							: PQ details
--Author							: Dilip V. 06-Jul-2012
--Modification history				: 1 AMIT Kumar 7 sept 2012(added PQEvent date  in select part).
--									: 2 Amit Kumar 11th Jan 2013 (added CCPL.PQCompletedEventOn )
--                                  : 3 Vinay Kumar Prajapati 14th May 2013 (added ND.DealerCode)
--									: 4 Amit Kumar 22nd July 2013(changed NRD.IsExecutive = 0  to NRD.Type=0 AND NRD1.IsExecutive = 1 To NRD1.type =1)


CREATE PROCEDURE [CRM].[PQRequest]
	@FromDate	DATETIME,
	@ToDate		DATETIME,
	@Make		VARCHAR(MAX)=NULL

AS

BEGIN
	SET NOCOUNT ON
	DECLARE	@varsql				VARCHAR(MAX),
			@SingleQuotesTwice	VARCHAR(2) = '''''',
			@SingleQuotes		VARCHAR(1) = '''',
			@frmDate			DATETIME, 
			@TDate				DATETIME
    SET @frmDate = CONVERT(DATETIME,CONVERT(VARCHAR(10),@FromDate,120)+ ' 00:00:00')	
	SET @TDate = CONVERT(DATETIME,CONVERT(VARCHAR(10),@ToDate,120)+ ' 23:59:59');
	SELECT Name,HierId.ToString() HierId from NCS_RManagers;
	
	SET @varsql =  'WITH CP AS 
	(
	SELECT CC.Id,
	ROW_NUMBER() OVER (PARTITION BY CCPL.Id ORDER BY CCPL.PQCompletedEventOn DESC) AS RowNumber,CONVERT(CHAR(19), CCA.CreatedOn, 106)AS DADate,
	(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
	CC.Email, CC.Mobile, CC.Landline, (CMA.Name + '+@SingleQuotes+' '+@SingleQuotes+' + CMO.Name + '+@SingleQuotes+' '+@SingleQuotes+' + CV.Name) CarName, 
	S.Name +'+@SingleQuotes+' /'+@SingleQuotes+'+ C.Name CityName, OU.UserName, CCPL.IsPQNotRequired,CCPL.PQRequestDate,CCPL.IsPQCompleted,CCPL.PQCompleteDate, 
	CASE CCPL.IsPQCompleted WHEN 1 THEN CONVERT(VARCHAR,ISNULL(CCPL.PQCompletedEventOn,'''')) ELSE '''' END EventOn, 
	CASE CCPL.IsPQCompleted WHEN 1 THEN DATEDIFF(DD,CCPL.PQRequestDate,CCPL.PQCompleteDate) ELSE '''' END DayDifference,
	CASE  WHEN CCPL.IsPQCompleted = 1 OR CCPL.IsPQNotRequired = 1 THEN
	CONVERT(VARCHAR,DATEDIFF(hh,CCPL.PQRequestDate,CCPL.PQCompleteDate)) ELSE '''' END / 24 DayDiff,
	 CASE  WHEN CCPL.IsPQCompleted = 1 OR CCPL.IsPQNotRequired = 1 THEN 
	 CONVERT(VARCHAR,DATEDIFF(hh,CCPL.PQRequestDate,CCPL.PQCompleteDate)) ELSE '''' END % 24 HourDiff,
	CL.CreatedOn LeadDate,CAT.Name TeamName, CDA.ContactPerson, CDA.Contact, CED.Name Dispositions, 
	ND.Id DealerId, ND.Name Dealer,ND.DealerCode, NRM.Name DealerTL, NRM.HierId.GetAncestor(1).ToString() PLName,CII.ClosingProbability, 
	CASE CLS.CategoryId WHEN 1 THEN '+@SingleQuotes+'CarWale'+@SingleQuotes+' WHEN 2 THEN '+@SingleQuotes+'CarWale'+@SingleQuotes+' WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName, 
	NRM.HierId.GetAncestor(2).ToString() AS RM1, NRM.HierId.GetAncestor(3).ToString() AS RM2, NRM.HierId.GetAncestor(4).ToString() AS RM3, NRM4.Name AS CWExec, CAS.Name +'+@SingleQuotes+' /'+@SingleQuotes+'+ CAC.Name AS CarCity  , 
	OUD.UserName AS DCName, OUC.UserName CreatedBy ,CCPL.CreatedOn AS PQEventDate 
	FROM CRM_CarPQLog CCPL WITH(NOLOCK) 
	INNER JOIN OprUsers OUC WITH(NOLOCK) ON OUC.Id = CCPL.CreatedBy 
	LEFT JOIN CRM_EventLogs CEL WITH(NOLOCK) ON CEL.ItemId = CCPL.CBDId AND CEL.EventType = 36
	LEFT JOIN CRM_ETDispositions CED WITH(NOLOCK) ON CCPL.PQNRDispositionId = CED.Id AND CED.IsActive = 1 
	INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CCPL.CBDId = CBD.ID 
	LEFT JOIN Cities CAC WITH(NOLOCK) ON CBD.CityId = CAC.ID 
	INNER JOIN States CAS WITH(NOLOCK) ON CAC.StateId = CAS.ID AND CAS.IsDeleted = 0  
	INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.ID 
	LEFT JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId 
	LEFT JOIN LA_Agencies AS LA WITH(NOLOCK) ON LA.Id = CLS.SourceId 
	INNER JOIN CarVersions CV WITH(NOLOCK) ON CBD.VersionId = CV.ID 
	INNER JOIN CarModels CMO WITH(NOLOCK) ON CV.CarModelId = CMO.ID 
	INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID 
	INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.ID 
	INNER JOIN Cities C WITH(NOLOCK) ON CBD.CityId = C.Id 
	INNER JOIN States AS S WITH(NOLOCK) ON C.StateId = S.ID AND S.IsDeleted = 0  
	INNER JOIN OprUsers OU WITH(NOLOCK) ON CCPL.CreatedBy = OU.Id 
	LEFT JOIN CRM_ADM_Teams CAT WITH(NOLOCK) ON CL.Owner = CAT.Id 
	INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CBD.ID = CDA.CBDId 
	INNER JOIN NCS_Dealers ND WITH(NOLOCK) ON CDA.DealerId = ND.ID 
	LEFT JOIN NCS_RMDealers NRD WITH(NOLOCK) ON ND.ID = NRD.DealerId AND NRD.Type = 0 
	LEFT JOIN NCS_RManagers NRM WITH(NOLOCK) ON NRD.RMId = NRM.Id 	
	LEFT JOIN NCS_RMDealers NRD1 WITH(NOLOCK) ON ND.ID = NRD1.DealerId AND NRD1.Type = 1 
	LEFT JOIN NCS_RManagers NRM4 WITH(NOLOCK) ON NRD1.RMId = NRM4.Id 
	INNER JOIN CRM_InterestedIn CII WITH(NOLOCK) ON CL.Id = CII.LeadId 
	LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON ND.ID = CAD.DealerId  
	LEFT JOIN OprUsers OUD WITH(NOLOCK) ON CAD.DCID = OUD.Id,
	CRM_CarDealerAssignment CCA WITH(NOLOCK)  
	WHERE CCA.CBDId=CBD.Id AND CII.ProductTypeId = 1 AND CCPL.PQRequestDate BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @frmDate, 120) +@SingleQuotes+' 
	AND ' +@SingleQuotes+ CONVERT(CHAR(19), @TDate, 120) +@SingleQuotes +''
	IF(@Make IS NOT NULL)
	BEGIN
		SET @varsql += ' AND CMA.Id IN ('+@Make+')'
	END
	SET @varsql += ' ) SELECT Id,CustomerName, DADate,Email, Mobile, Landline, CarName,CityName,UserName, IsPQNotRequired, PQRequestDate,IsPQCompleted,DayDifference ,DayDiff,HourDiff,
	PQCompleteDate,EventOn,LeadDate,TeamName,ContactPerson, Contact, Dispositions,DealerId, Dealer,DealerCode,DealerTL,PLName,ClosingProbability,SourceName,RM1,RM2,RM3,CWExec,CarCity,DCName,CreatedBy
	,PQEventDate
	FROM CP 
	WHERE RowNumber = 1 
	ORDER BY Dealer'
	
   -- PRINT (@varsql)
	EXEC (@varsql)

END


