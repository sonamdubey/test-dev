IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarTrade_GetCertificationRequestDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarTrade_GetCertificationRequestDetails]
GO

	-- =============================================================================
-- Author		: Suresh Prajapati
-- Created Date : 15th Dec, 2015
-- Description	: To get CarTrade Certification Request from ListingId(StockId)
-- Modified By  : Chetan Navin on 3/3/2016 (Added dealer contact person,email and pincode columns to fetch)
-- EXEC TC_CarTrade_GetCertificationRequestDetails 611965
-- =============================================================================
CREATE PROCEDURE [dbo].[TC_CarTrade_GetCertificationRequestDetails] @ListingId INT
AS
BEGIN
	SELECT TOP 1 TC.TC_CarTradeCertificationRequestId
		,TC.RegistrationNo
		,TC.DealerId
		,TC.DealerName
		,TC.DealerMobile
		,TC.DealerAddress
		,TC.Make
		,TC.Model
		,TC.Variant
		,TC.Color
		,TC.ManufacturingYear
		,TC.CarTradeCertificationId
		,TC.CertificationStatus
		,TC.DealerEmail
		,TC.DealerContactPerson
		,TC.DealerPinCode
		,TC.DealerCity
	FROM TC_CarTradeCertificationRequests TC WITH (NOLOCK)
	WHERE TC.ListingId = @ListingId
	ORDER BY TC_CarTradeCertificationRequestId  DESC
END