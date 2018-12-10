IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUnlistedCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUnlistedCars]
GO

	

-- =============================================
-- Author:		Amit Verma
-- Create date: 20 May 2013
-- Description:	Get Unlisted Cars
-- EXEC GetUnlistedCars 1805,44
-- =============================================
CREATE PROCEDURE [dbo].[GetUnlistedCars] 
	-- Add the parameters for the stored procedure here
	@CustomerId int,
	@DefaultPackageId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Select Ma.Name + ' ' + Mo.Name + ' ' + Cv.Name AS Car, SI.ID AS SellInquiryId, IsNull(SI.ViewCount, 0) AS Viewed, 
	(Select Count(Distinct CustomerId) From ClassifiedRequests Where SellInquiryId = SI.ID) AS PurReq
	,(CASE when SI.PackageId is null then @DefaultPackageId else SI.PackageId end) as PackageId
	,(CASE when P.Amount is null then (select Amount from Packages where ID = @DefaultPackageId) else P.Amount end) as PackageAmount
	From CarMakes AS Ma, CarModels AS Mo, CarVersions AS Cv, CustomerSellInquiries SI
	left join Packages P on SI.PackageId = P.ID
	Where SI.CustomerId = @CustomerId AND SI.IsApproved = 0 AND SI.IsFake = 0 AND 
	Cv.ID = SI.CarVersionId AND Mo.ID = Cv.CarModelId AND Ma.ID = Mo.CarMakeId AND SI.StatusId = 1 
	UNION ALL 
	Select Ma.Name + ' ' + Mo.Name + ' ' + Cv.Name AS Car, SI.ID AS SellInquiryId, IsNull(SI.ViewCount, 0) AS Viewed, 
	(Select Count(Distinct CustomerId) From ClassifiedRequests Where SellInquiryId = SI.ID) AS PurReq
	,(CASE when SI.PackageId is null then @DefaultPackageId else SI.PackageId end) as PackageId
	,(CASE when P.Amount is null then (select Amount from Packages where ID = @DefaultPackageId) else P.Amount end) as PackageAmount
	From CarMakes AS Ma, CarModels AS Mo, CarVersions AS Cv, LiveListings AS LL , CustomerSellInquiries SI
	left join Packages P on SI.PackageId = P.ID
	Where SI.CustomerId = @CustomerId AND LL.InquiryId = SI.ID AND LL.SellerType = 2 AND 
	Cv.ID = SI.CarVersionId AND Mo.ID = Cv.CarModelId AND Ma.ID = Mo.CarMakeId AND SI.PackageType = 1 AND SI.StatusId = 1 
	ORDER BY Viewed Desc 
END


