IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[TDRequest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[TDRequest]
GO

	



--Name of SP/Function				: CarWale.[CRM].[TDRequest]
--Applications using SP				: CRM
--Modules using the SP				: TDRequest.cs
--Technical department				: Database
--Summary							: TD details
--Author							: Dilip V. 06-Jul-2012
--Modification history				: 1.Amit Kumar on 11th jan 2013 removed INNER JOIN CRM_EventLogs CEL WITH(NOLOCK) ON CEL.ItemId = CCTL.CBDId and put ISNULL(CCTL.TDCompletedEventOn,'''') 
--                                  : 2.Vinay Kumar Prajapati 14th May (Added ND.DealerCode)
CREATE PROCEDURE [CRM].[TDRequest]
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
		SELECT DISTINCT CBD.ID BasicDataId, CC.Id, CONVERT(CHAR(19), CCA.CreatedOn, 106) AS DADate,
		ROW_NUMBER() OVER (PARTITION BY CCTL.Id ORDER BY CCTL.TDCompletedEventOn DESC) AS RowNumber,
		(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, CC.Email,
		CC.Mobile , CC.Landline, (CMA.Name + '+@SingleQuotes+' '+@SingleQuotes+' + CMO.Name + '+@SingleQuotes+' '+@SingleQuotes+' + CV.Name) CarName, C.Name CityName,
		OU.UserName,CCTL.ISTDNotPossible, CCTL.TDRequestDate,CCTL.IsTDCompleted,CCTL.TDCompleteDate,
		CASE CCTL.IsTDCompleted WHEN 1 THEN CONVERT(VARCHAR,ISNULL(CCTL.TDCompletedEventOn,'''')) ELSE CONVERT(VARCHAR,ISNULL(CCTL.CreatedOn,'''')) END EventOn,
		CASE CCTL.IsTDCompleted WHEN 1 THEN DATEDIFF(DD,CCTL.TDRequestDate,CCTL.TDCompleteDate) ELSE '''' END DayDifference,
		CASE  WHEN CCTL.IsTDCompleted = 1 OR CCTL.ISTDNotPossible = 1 THEN
	CONVERT(VARCHAR,DATEDIFF(hh,CCTL.TDRequestDate,CCTL.TDCompleteDate)) ELSE '''' END / 24 DayDiff,
	 CASE WHEN CCTL.IsTDCompleted = 1 OR CCTL.ISTDNotPossible = 1 THEN 
	 CONVERT(VARCHAR,DATEDIFF(hh,CCTL.TDRequestDate,CCTL.TDCompleteDate)) ELSE '''' END % 24 HourDiff,				
		CCTL.CreatedOn,CBD.LeadId, CL.CreatedOn LeadDate,CED.Name Dispositions,CAT.Name TeamName, CDA.ContactPerson, CDA.Contact,
		ND.Id DealerId, ND.Name Dealer,ND.DealerCode,NRM.Name DealerTL,NRM.HierId.GetAncestor(1).ToString() PLName, NRM4.Name AS CWExec, CII.ClosingProbability               
		, NRM.HierId.GetAncestor(2).ToString() AS RM1, NRM.HierId.GetAncestor(3).ToString() AS RM2, NRM.HierId.GetAncestor(4).ToString() AS RM3 
		, OUD.UserName AS DCName, OUC.UserName CreatedBy, 
		CASE LS.CategoryId WHEN 1 THEN '+@SingleQuotes+'CarWale'+@SingleQuotes+' WHEN 2 THEN '+@SingleQuotes+'CarWale'+@SingleQuotes+' WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS LeadSource
		FROM CRM_CarTDLog CCTL WITH(NOLOCK)
		INNER JOIN OprUsers OUC WITH(NOLOCK) ON OUC.Id = CCTL.CreatedBy
		
		LEFT JOIN CRM_ETDispositions CED WITH(NOLOCK) ON CCTL.TDNPDispositionId = CED.Id AND CED.IsActive = 1,
		CRM_CarBasicData CBD WITH(NOLOCK),CRM_Leads CL WITH(NOLOCK)
		LEFT JOIN CRM_LeadSource AS LS WITH(NOLOCK) ON LS.LeadId = CL.ID
		LEFT JOIN LA_Agencies AS LA WITH(NOLOCK) ON LA.Id = LS.SourceId
		LEFT JOIN CRM_ADM_Teams CAT WITH(NOLOCK) ON CL.Owner = CAT.Id
		,CarVersions CV WITH(NOLOCK), CarModels CMO WITH(NOLOCK), CarMakes CMA WITH(NOLOCK),
		CRM_Customers CC WITH(NOLOCK), Cities C WITH(NOLOCK),OprUsers OU WITH(NOLOCK),
		CRM_CarDealerAssignment CDA WITH(NOLOCK), NCS_Dealers ND WITH(NOLOCK)
		LEFT JOIN NCS_RMDealers NRD WITH(NOLOCK) ON ND.ID = NRD.DealerId AND NRD.Type = 0
		LEFT JOIN NCS_RManagers NRM WITH(NOLOCK) ON NRD.RMId = NRM.Id 
		LEFT JOIN NCS_RMDealers NRD1 WITH(NOLOCK) ON ND.ID = NRD1.DealerId AND NRD1.Type = 1
		LEFT JOIN NCS_RManagers NRM4 WITH(NOLOCK) ON NRD1.RMId = NRM4.Id
		LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON ND.ID = CAD.DealerId
		LEFT JOIN OprUsers OUD WITH(NOLOCK) ON CAD.DCID = OUD.Id
		,CRM_InterestedIn CII WITH(NOLOCK),CRM_CarDealerAssignment CCA WITH(NOLOCK)
		WHERE CCA.CBDId=CBD.Id AND CCTL.CBDId = CBD.ID AND CBD.LeadId = CL.ID AND CL.CNS_CustId = CC.ID AND CBD.VersionId = CV.ID
		AND CV.CarModelId = CMO.ID AND CMO.CarMakeId = CMA.ID AND CBD.CityId = C.Id AND CCTL.CreatedBy = OU.Id
		AND CBD.ID = CDA.CBDId AND CDA.DealerId = ND.ID
		AND CL.Id = CII.LeadId AND CII.ProductTypeId = 1 AND CCTL.IsTDRequested = 1
		AND CCTL.TDRequestDate BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @frmDate, 120) +@SingleQuotes+' 
	AND ' +@SingleQuotes+ CONVERT(CHAR(19), @TDate, 120) +@SingleQuotes +''
	IF(@Make IS NOT NULL)
	BEGIN
		SET @varsql += ' AND CMA.Id IN ('+@Make+')'
	END
	SET @varsql += ' ) SELECT BasicDataId,Id,CustomerName, DADate, Email, Mobile, Landline, CarName,CityName,UserName, ISTDNotPossible, TDRequestDate,IsTDCompleted,DayDifference,DayDiff,HourDiff,
	TDCompleteDate,EventOn,CreatedOn,LeadId,LeadDate,TeamName,ContactPerson, Contact, Dispositions,DealerId, Dealer,DealerCode,DealerTL,PLName,ClosingProbability,RM1,RM2,RM3,CWExec,DCName,CreatedBy,LeadSource
	FROM CP 
	WHERE RowNumber = 1 
	ORDER BY Dealer'
	
	--PRINT (@varsql)
	EXEC (@varsql)

END



