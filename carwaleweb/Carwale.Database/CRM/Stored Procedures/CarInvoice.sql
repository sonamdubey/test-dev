IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[CarInvoice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[CarInvoice]
GO

	


-- Description	:	Get Report of Car Invoice (RMPanel and can be used for CRM)
-- Author		:	Dilip V. 24-Mar-2012
-- Modifier		:	

CREATE PROCEDURE [CRM].[CarInvoice]	
	@InvoiceId	NUMERIC(18,0),
	@Status		SMALLINT,
	@DealerId	VARCHAR(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE	@varsql				VARCHAR(MAX),
			@SingleQuotes		VARCHAR(1) = ''''
			
	BEGIN		
		
		
SET @varsql =  'SELECT DISTINCT CI.CBDId,CBD.LeadId AS LeadId,CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+'+ CC.LastName AS Customer, CC.Mobile, CC.Email, CC.Id AS CustomerId,
		VW.Model + '+@SingleQuotes+' '+@SingleQuotes+'+ VW.Version AS CarName, CB.BookingDate AS Bookingcompleteddate, CDD.ActualDeliveryDate,
		CB.RegisterPersonName, CA.CreatedOn AS InvoiceCreatedOn, CL.CreatedOn AS LDDate,
		CDD.EngineNumber, CDD.ChasisNumber, CDD.RegistrationNumber,  ND.Name AS Dealer, CI.IsActive, 		
		(SELECT Top 1 CEL.EventOn FROM CRM_EventLogs CEL WITH (NOLOCK) WHERE CEL.ItemId = CI.CBDId AND CEL.EventType = 16 ORDER BY ID DESC) CarBookedOn, 
		(SELECT Top 1 CEL.EventOn FROM CRM_EventLogs CEL WITH (NOLOCK) WHERE CEL.ItemId = CI.CBDId AND CEL.EventType = 45 ORDER BY ID DESC) UpdatedOn 
		,VWC.City AS CustCity, VWC.State AS CustState, VWB.City AS CarCity, VWB.State AS CarState, NRM.Name AS CWExec  
		FROM CRM_ADM_Invoices CA WITH (NOLOCK)
		INNER JOIN CRM_CarInvoices CI WITH (NOLOCK) ON CI.InvoiceId = CA.Id
		INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID = CI.CBDId 
		LEFT JOIN CRM_CarBookingData CB WITH (NOLOCK) ON CB.CarBasicDataId = CI.CBDId
		LEFT JOIN CRM_CarDeliveryData CDD WITH (NOLOCK) ON CDD.CarBasicDataId = CI.CBDId
		INNER JOIN vwCity VWB WITH (NOLOCK) ON VWB.CityId = CBD.CityId
		INNER JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CBD.Id
		INNER JOIN NCS_Dealers ND WITH (NOLOCK) ON ND.Id = CDA.DealerId
		INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CL.ID = CBD.LeadId
		INNER JOIN CRM_Customers CC WITH (NOLOCK) ON CC.ID = CL.CNS_CustId 
		INNER JOIN vwCity VWC WITH (NOLOCK) ON VWC.CityId = CC.CityId
		INNER JOIN vwMMV VW WITH (NOLOCK) ON CBD.VersionId = VW.VersionId
		LEFT JOIN NCS_RMDealers NRD WITH(NOLOCK) ON ND.ID = NRD.DealerId AND NRD.IsExecutive = 1 
		LEFT JOIN NCS_RManagers NRM WITH(NOLOCK) ON NRD.RMId = NRM.Id
		WHERE 
		CA.ID = ' + CONVERT(CHAR(10), @InvoiceId, 101)
		
		IF(@DealerId IS NOT NULL)
			SET @varsql += ' AND ND.Id IN ('+@DealerId+')'
		
		IF(@Status = -1)
			SET @varsql += ' AND CI.IsActive IS NULL'
		ELSE
			SET @varsql += ' AND CI.IsActive = ' + CONVERT(CHAR(10), @Status, 101)		
		
		SET @varsql += ' ORDER BY CarName'
		PRINT(@varsql)
		EXEC(@varsql)
	END
	
END








