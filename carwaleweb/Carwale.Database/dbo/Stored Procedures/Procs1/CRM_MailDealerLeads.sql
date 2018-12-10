IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_MailDealerLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_MailDealerLeads]
GO

	
-- Author	:	Sachin Bharti(17th May 2013)
CREATE Procedure [dbo].[CRM_MailDealerLeads]
@DealerId	NUMERIC(18,0),
@Date		VARCHAR(15)
AS
BEGIN

	SELECT DISTINCT CDA.DealerId, ND.Name AS DealerName, ND.Email AS DealerEMail, NR.Email AS RMMail,
	CMA.Name AS Make, CMO.Name AS Model, CV.Name AS Version, C.Name AS City,
	S.Name AS State, CC.FirstName, CC.LastName, CC.EMail AS CEMail, CC.Mobile,
	CC.Landline, OU.LoginId, CDA.Comments, 'Y' AS DealerAssign,
	CTD.TDStatusId, CBD.IsPQMailExternalReq, ND.ContactPerson, CCBD.BookingStatusId,
	CASE CII.ClosingProbability WHEN 1 THEN 'Very High' WHEN 2 THEN 'High'
	WHEN 3 THEN 'Normal' WHEN 4 THEN 'Low' ELSE 'None' END AS LeadEagerness

	FROM NCS_Dealers AS ND, CRM_Customers AS CC, CarMakes AS CMA, CarModels AS CMO,
	CarVersions AS CV, Cities AS C, States AS S, 
	CRM_Leads AS CL, CRM_CarDealerAssignment AS CDA
	INNER JOIN CRM_ADM_DCDealers AS CAD WITH (NOLOCK) ON CDA.DealerId = CAD.DealerId 
	INNER JOIN OPRUsers AS OU WITH (NOLOCK) ON CAD.DCID = OU.Id
	INNER JOIN CRM_CarBasicData AS CBD ON CDA.CBDId = CBD.Id
	LEFT JOIN CRM_CarTestDriveData AS CTD ON CTD.CarBasicDataId = CBD.Id
	LEFT JOIN CRM_CarBookingData AS CCBD ON CBD.Id = CCBD.CarBasicDataId
	LEFT JOIN NCS_RMDealers NRD ON CDA.DealerId = NRD.DealerId AND NRD.Type = 0
	LEFT JOIN NCS_RManagers AS NR ON NRD.RMId = NR.Id
	LEFT JOIN CRM_InterestedIn AS CII ON CBD.LeadId = CII.LeadId AND CII.ProductTypeId = 1

	WHERE CDA.DealerId = ND.Id 
	AND CBD.VersionId = CV.Id
	AND CV.CarModelId = CMO.Id AND CMO.CarMakeId = CMA.Id
	AND CBD.LeadId = CL.Id AND CL.CNS_CustId = CC.Id
	AND CC.CityId = C.Id AND C.StateId = S.Id
	AND CONVERT(VARCHAR(10), CDA.CreatedOn, 103) = @Date
	AND ND.ID = @DealerId
END