IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetInsuranceLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetInsuranceLeads]
GO

	-- =============================================
-- Author:		Manish Chourasiya
-- Create date: 20-03-2014
-- Description:	To send insurance leads to the client
--  [dbo].[GetInsuranceLeads] 0
-- =============================================
CREATE PROCEDURE [dbo].[GetInsuranceLeads] 
    @ClientId INT
	AS
BEGIN

	SELECT  IP.Id,
			Ct.Name AS City,
			vw.Make,
			vw.Model,
			Vw.Version,
			IP.InsTypeNew,
			IP.MakeYear,
			IP.Price,
			IP.Displacement,
			IP.Premium,
			IP.RequestDateTime,
			IP.Name,
			IP.Email,
			IP.Mobile,
			PushStatus,
			C.Name As Name1,
			C.email Email1,
			C.Mobile Mobile1
	FROM INS_PremiumLeads as IP with (nolock)
	JOIN vwMMV as vw on vw.VersionId=IP.VersionId
	JOIN Cities as Ct with (nolock) on CT.Id=IP.CityId
	LEFT JOIN customers as C with (nolock) on C.Id=IP.CustomerID
	WHERE ClientId = @ClientId
	AND IP.ID>(SELECT MAX(LastLeadIdSent) FROM CarInsuranceLeadSentLogs with (nolock) WHERE ClientId=@ClientId)
	ORDER BY IP.Id

	INSERT INTO CarInsuranceLeadSentLogs (ClientId,LastLeadIdSent)
	SELECT  @ClientId,
	        MAX(IP.Id)
	FROM INS_PremiumLeads as IP with (nolock)
	JOIN vwMMV as vw on vw.VersionId=IP.VersionId
	JOIN Cities as Ct with (nolock) on CT.Id=IP.CityId
	LEFT JOIN customers as C with (nolock) on C.Id=IP.CustomerID
	WHERE ClientId = @ClientId
	AND IP.ID>(SELECT MAX(LastLeadIdSent) FROM CarInsuranceLeadSentLogs with (nolock) WHERE ClientId=@ClientId)

END 