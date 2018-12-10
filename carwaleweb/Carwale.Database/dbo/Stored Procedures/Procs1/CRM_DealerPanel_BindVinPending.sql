IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DealerPanel_BindVinPending]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DealerPanel_BindVinPending]
GO

	CREATE PROCEDURE [dbo].[CRM_DealerPanel_BindVinPending]
@dealerIds			VARCHAR(MAX),	
@closingProbability VARCHAR(50)
AS
BEGIN
	SELECT DISTINCT CC.Id CustomerId, (CC.FirstName + ' ' + CC.LastName) AS CustomerName,
		CL.Id AS LeadId,(VM.Model + ' ' + VM.Version) AS CarName, CL.ID LeadId, CDA.DealerId,
		CASE WHEN CCDD.ExpectedDeliveryDate IS NULL
		THEN DATEDIFF(day, CCBD.BookingDate, GETDATE())
		ELSE DATEDIFF(day, CCDD.ExpectedDeliveryDate, GETDATE()) END AS OverdueDays,
		CCDD.RegPersonName, CCDD.ExpectedDeliveryDate,CCBD.BookingDate, CDA.CBDId AS CBDID,
		CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE 'CarWale' END AS SourceName,
		(SELECT TOP 1 CPA.IsApproved FROM CRM_CientPendingApprovals CPA WHERE CL.ID = CPA.LeadId
		AND CPA.CurrentEventType = 46 AND CBD.ID = CPA.CBDId AND CPA.IsApproved = 0) AS IsApproved, CAC.Name As CarCity

	FROM CRM_CarDealerAssignment AS CDA WITH(NOLOCK)
		LEFT JOIN CRM_CarDeliveryData AS CCDD WITH(NOLOCK) ON CDA.CBDId = CCDD.CarBasicDataId
		INNER JOIN CRM_CarBookingData AS CCBD ON CDA.CBDId = CCBD.CarBasicDataId
		INNER JOIN CRM_CarBasicData AS CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id
		LEFT JOIN Cities CAC WITH (NOLOCK) ON CBD.CityId = CAC.ID
		LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId
		INNER JOIN CRM_InterestedIn CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId
		INNER JOIN vwMMV AS VM WITH(NOLOCK) ON CBD.VersionId = VM.VersionId
		INNER JOIN CRM_Leads AS CL WITH(NOLOCK) ON CBD.LeadId = CL.Id
		INNER JOIN CRM_Customers AS CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id    
                        
	WHERE CDA.Status NOT IN (40,41,42,50,60,61)
		AND ISNULL(CCDD.ChasisNumber, '') = ''
		AND ISNULL(CCDD.EngineNumber, '') = ''
		AND ISNULL(CCDD.RegistrationNumber,'') = ''
		AND CDA.DealerId IN ( SELECT ListMember FROM [dbo].[fnSplitCSV] (@dealerIds) )
		AND CII.ClosingProbability IN ( SELECT ListMember FROM [dbo].[fnSplitCSV] (@closingProbability)  )
		AND CDA.CBDId NOT IN
		(SELECT CBDId FROM CRM_CarInvoices WHERE InvoiceId IN (SELECT id FROM CRM_ADM_Invoices WHERE MakeId = 15))
		AND (CCBD.BookingStatusId = 16 OR CCDD.DeliveryStatusId = 20)
		AND CII.ProductTypeId = 1
	ORDER BY OverdueDays Desc
END
