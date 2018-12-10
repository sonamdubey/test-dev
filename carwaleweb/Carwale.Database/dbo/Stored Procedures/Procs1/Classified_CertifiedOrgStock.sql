IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_CertifiedOrgStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_CertifiedOrgStock]
GO

	CREATE Procedure [dbo].[Classified_CertifiedOrgStock]
(
	@CertifiedOrgId INT,
	@CityId INT = NULL
)
AS
Begin
	IF(@CityId IS NOT NULL)
		Begin
			SELECT ProfileId,(L.MakeName + ' ' + L.ModelName + ' ' + L.VersionName) 'Car', DATEPART(YEAR,L.MakeYear)'Year', L.CityName, L.Price, L.Kilometers, L.CertificationId		
			FROM LiveListings L
					INNER JOIN SellInquiries Si ON Si.ID = L.Inquiryid AND Si.DealerId IN(SELECT DealerId FROM Classified_CertifiedOrgSub WHERE CertifiedOrgId=@CertifiedOrgId)
			WHERE CityId=@CityID AND L.SellerType = 1 
			ORDER BY L.Price
		End
	ELSE
		Begin
			SELECT ProfileId,(L.MakeName + ' ' + L.ModelName + ' ' + L.VersionName) 'Car', DATEPART(YEAR,L.MakeYear)'Year', L.CityName, L.Price, L.Kilometers, L.CertificationId		
			FROM LiveListings L
					INNER JOIN SellInquiries Si ON Si.ID = L.Inquiryid AND Si.DealerId IN(SELECT DealerId FROM Classified_CertifiedOrgSub WHERE CertifiedOrgId=@CertifiedOrgId)
			WHERE L.SellerType = 1
			ORDER BY L.Price
		End
End
