IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_CertifiedOrgCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_CertifiedOrgCities]
GO

	
CREATE procedure [dbo].[Classified_CertifiedOrgCities]
(
	@CertifiedOrgId INT	
)
as
Begin
	select distinct CityId,CityName FROM LiveListings L
			INNER JOIN SellInquiries Si ON Si.ID = L.Inquiryid AND Si.DealerId IN(SELECT DealerId FROM Classified_CertifiedOrgSub WHERE CertifiedOrgId=@CertifiedOrgId)
	WHERE L.SellerType = 1
	ORDER BY CityName
End
