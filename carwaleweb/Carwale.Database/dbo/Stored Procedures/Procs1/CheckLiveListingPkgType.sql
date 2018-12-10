IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckLiveListingPkgType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckLiveListingPkgType]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 6/1/2014
-- Description:	Check if there are Free plans with IsPremium=1 and PackageTypes other than Free or Premium
-- =============================================
CREATE PROCEDURE [dbo].[CheckLiveListingPkgType]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT DISTINCT IP.Name PackageType,LL.IsPremium,COUNT(DISTINCT InquiryId) Count
	--FROM Livelistings LL WITH (NOLOCK)
	--INNER JOIN InquiryPointCategory IP WITH (NOLOCK) ON IP.Id=LL.PackageType
	--WHERE SellerType=2 AND ((PackageType=30 and  ll.IsPremium=1 ) OR PackageType NOT IN(30,31))
	--GROUP BY IP.Name,LL.IsPremium
	SELECT DISTINCT CASE SellerType WHEN 1 THEN 'Dealer' ELSE 'Individual' END Seller,IP.NAME PackageType
		,IP.IsPremium PackageTypeIsPremium
		,LL.IsPremium LivelistingsIsPremium
		,COUNT(DISTINCT InquiryId) Count
	FROM Livelistings LL WITH (NOLOCK)
	INNER JOIN InquiryPointCategory IP WITH (NOLOCK) ON IP.Id = LL.PackageType
	WHERE  
		(SellerType=1 AND PackageType = 28 AND ll.IsPremium = 1)
			OR
			(SellerType=2 AND ((PackageType = 30 AND ll.IsPremium = 1) OR (PackageType NOT IN (30,31))))
		
	GROUP BY SellerType
		,IP.NAME
		,LL.IsPremium
		,IP.IsPremium

END
