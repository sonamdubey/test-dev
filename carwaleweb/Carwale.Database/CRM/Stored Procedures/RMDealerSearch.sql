IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[RMDealerSearch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[RMDealerSearch]
GO

	
CREATE PROCEDURE [CRM].[RMDealerSearch]	
	@DealerId	VARCHAR(MAX),
	@Name		VARCHAR(15) = NULL,
	@Email		VARCHAR(15) = NULL,
	@PhoneL		VARCHAR(15) = NULL,
	@PhoneM		VARCHAR(15) = NULL

AS

BEGIN
	SET NOCOUNT ON
	DECLARE	@varsql				VARCHAR(MAX),
			@SingleQuotesTwice	VARCHAR(2) = '''''',
			@SingleQuotes		VARCHAR(1) = ''''
			
	SET @varsql =  'SELECT DISTINCT CC.ID, (FirstName +'+@SingleQuotes+' '+@SingleQuotes+'+ LastName) AS CustomerName,CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
	(CMA.Name + '+@SingleQuotes+' '+@SingleQuotes+' + CMO.Name + '+@SingleQuotes+' '+@SingleQuotes+' + CV.Name) AS CarName,'+@SingleQuotesTwice+' NextCallDate,CDA.CreatedOn, CL.ID LeadId, CDA.DealerId,'+@SingleQuotesTwice+' CP, 
	CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName,
	ND.Name Dealer,CATM.TeamId,CBD.ID CBDID 
	FROM Cities AS C, (((((((CRM_CarDealerAssignment AS CDA WITH (NOLOCK) 
	INNER JOIN NCS_Dealers ND WITH (NOLOCK) ON ND.ID = CDA.DealerId 
	LEFT JOIN CRM_CarBasicData AS CBD WITH (NOLOCK) ON CDA.CBDId = CBD.Id) 
	LEFT JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CBD.LeadId = CLS.LeadId 
	LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId 
	LEFT JOIN CRM_Leads AS CL WITH (NOLOCK) ON CBD.LeadId = CL.Id) 
	LEFT JOIN CRM_ADM_TeamMembers AS CATM WITH (NOLOCK) ON CL.Owner = CATM.TeamId) 
	LEFT JOIN CRM_Customers AS CC WITH (NOLOCK) ON CL.CNS_CustId = CC.Id) 
	LEFT JOIN CarVersions AS CV WITH (NOLOCK) ON CBD.VersionId = CV.Id) 
	LEFT JOIN CarModels AS CMO WITH (NOLOCK) ON CV.CarModelId = CMO.Id) 
	LEFT JOIN CarMakes AS CMA WITH (NOLOCK) ON CMO.CarMakeId = CMA.Id) 
	WHERE CC.CityId = C.Id 
	AND CDA.DealerId IN ('+ @DealerId +')'
	IF(@Name IS NOT NULL)
		SET @varsql += ' AND (CC.FirstName +'+@SingleQuotes+' '+@SingleQuotes+'+ CC.LastName) LIKE '+ @SingleQuotes+'%'+ @Name+'%'+ @SingleQuotes+''
	IF(@Email IS NOT NULL)
		SET @varsql += ' AND CC.Email LIKE '+ @SingleQuotes+'%'+ @Email+'%'+ @SingleQuotes+''
	IF((@PhoneL IS NOT NULL) OR (@PhoneM IS NOT NULL))
		SET @varsql += ' AND ( CC.Landline LIKE '+ @SingleQuotes+'%'+ @PhoneL+'%'+ @SingleQuotes+' OR CC.Mobile LIKE '+ @SingleQuotes+'%'+ @PhoneM+'%'+ @SingleQuotes+')'

	PRINT(@varsql)
	EXEC(@varsql)
END
