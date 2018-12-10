IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[RMDealerFollowUp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[RMDealerFollowUp]
GO

	
--Name of SP/Function				: CarWale.[CRM].[RMDealerFollowUp]
--Applications using SP				: RM and Dealer Panel
--Modules using the SP				: FollowUp.cs
--Technical department				: Database
--Summary							: TD details
--Author							: Dilip V. 11-Jul-2012
--Modification history				: 1.

CREATE PROCEDURE [CRM].[RMDealerFollowUp]
	@Values		TINYINT,
	@PanelType	TINYINT,
	@DealerId	VARCHAR(MAX)=NULL

AS

BEGIN
	SET NOCOUNT ON
	DECLARE	@varsql				VARCHAR(MAX),
			@SingleQuotesTwice	VARCHAR(2) = '''''',
			@SingleQuotes		VARCHAR(1) = '''',
			@TDate				DATETIME
	SET @TDate = CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),120)+ ' 23:59:59');
	
	SET @varsql =  'SELECT DISTINCT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, CC.Email, 
	CC.Mobile, CC.Landline,  C.Name CityName, (CMA.Name + '+@SingleQuotes+' '+@SingleQuotes+' + CMO.Name + '+@SingleQuotes+' '+@SingleQuotes+' + CV.Name) CarName, 
	CDA.CreatedOn,CL.ID LeadId,CDA.DealerId,FU.NextCallDate, 
	CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName,ND.Name Dealer, 
	(CASE CII.ClosingProbability WHEN 1 THEN '+@SingleQuotes+'Very Hot'+@SingleQuotes+' WHEN 2 THEN '+@SingleQuotes+'Hot'+@SingleQuotes+' WHEN 3 THEN '+@SingleQuotes+'Normal'+@SingleQuotes+' ELSE '+@SingleQuotes+'Cold'+@SingleQuotes+' END) AS CP,
	CATM.TeamId,CBD.ID CBDID FROM CRM_CarBasicData CBD WITH (NOLOCK) 
	LEFT JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CBD.LeadId = CLS.LeadId 
	LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId 
	INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CBD.LeadId = CL.Id 
	INNER JOIN CRM_ADM_TeamMembers AS CATM WITH (NOLOCK) ON CL.Owner = CATM.TeamId 
	LEFT JOIN CRM_DealerFollowUp FU WITH (NOLOCK) ON CL.ID = FU.LeadId 
	LEFT JOIN NCS_Dealers ND WITH (NOLOCK) ON ND.ID = FU.DealerId 
	INNER JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CBD.Id 
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CBD.VersionId = CV.Id 
	INNER JOIN CarModels CMO WITH (NOLOCK) ON CV.CarModelId = CMO.Id 
	INNER JOIN CarMakes CMA WITH (NOLOCK) ON CMO.CarMakeId = CMA.Id 
	INNER JOIN CRM_Customers CC WITH (NOLOCK) ON CL.CNS_CustId = CC.Id 
	LEFT JOIN Cities C WITH (NOLOCK) ON CC.CityId = C.Id 
	INNER JOIN CRM_InterestedIn CII WITH (NOLOCK) ON CL.Id = CII.LeadId 
	WHERE CL.LeadStageId <> 3 AND (CDA.Status = -1 OR CDA.Status IS NULL) 
	AND CII.ProductTypeId = 1 AND FU.PanelType = ' + CONVERT(CHAR(1), @PanelType, 101)
	IF (@Values = 7)
		SET @varsql += ' AND FU.Eagerness IS NOT NULL AND FU.ProductStatus IN (4,8) AND FU.DealerId = CDA.DealerId AND CDA.DealerId IN ('+@DealerId+')'
	ELSE IF(@Values = 5)
		SET @varsql += ' AND FU.Eagerness IS NOT NULL AND FU.ProductStatus = 3 AND FU.DealerId = CDA.DealerId AND CDA.DealerId IN ('+@DealerId+')'
	ELSE
		BEGIN
		SET @varsql += ' AND FU.Eagerness = ' + CONVERT(CHAR(1), @Values, 101) + '
		AND FU.NextCallDate <= ' +@SingleQuotes+ CONVERT(CHAR(19), @TDate, 120) +@SingleQuotes +'
		AND FU.Eagerness IS NOT NULL 
		AND FU.ProductStatus = 2 
		AND FU.DealerId = CDA.DealerId 
		AND CDA.DealerId IN ('+@DealerId+')'
		END
	SET @varsql += ' ORDER BY NextCallDate DESC, CP'
	PRINT(@varsql)
	EXEC(@varsql)
		
 END
