IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetLisitingCountByMobile]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetLisitingCountByMobile]
GO

	CREATE PROCEDURE [dbo].[GetLisitingCountByMobile]
	-- Add the parameters for the stored procedure here
	@CarId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Mobile VARCHAR(20) = (SELECT CustomerMobile FROM CustomerSellInquiries WITH(NOLOCK) WHERE ID = @CarId)

    SELECT PC.Id PkType, COUNT(T.PackageType) [Count] FROM
	InquiryPointCategory PC WITH(NOLOCK)
	LEFT JOIN (SELECT CS.PackageType FROM CustomerSellInquiries CS WITH(NOLOCK)
			  INNER JOIN livelistings LL WITH(NOLOCK) ON LL.Inquiryid = CS.ID AND LL.SellerType=2
			  WHERE CS.CustomerMobile = @Mobile AND IsArchived = 0) T ON PC.Id=T.PackageType  -- Added isArchived = 0 by Aditi on 4/08/2014
	WHERE PC.Id IN (30,31)
	GROUP BY PC.Id
END
