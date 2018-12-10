IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckIndividualPackageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckIndividualPackageDetails]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <9/12/2013>
-- Description:	<Check if the combination of Package type,Package Id and Source Id for Individual listers is correct>
-- =============================================
CREATE PROCEDURE [dbo].[CheckIndividualPackageDetails]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SourceId,csi.PackageType,csi.PackageId,ll.PackageType AS LLPackageType,COUNT(CSI.ID) AS TotalCount
	FROM CustomerSellInquiries CSI WITH(NOLOCK)
	INNER JOIN LiveListings LL WITH(NOLOCK) ON LL.Inquiryid=CSI.ID and SellerType=2
	WHERE (CSI.PackageType=31 and (PackageId<>49 OR PackageId IS NULL))
	OR (CSI.PackageType=30 and (PackageId<>48 OR PackageId IS NULL))
	OR (CSI.PackageType<>LL.PackageType)
	OR (CSI.PackageType=31 and SourceId=36 and CSI.ID<>380337)
	OR (LL.PackageType=2)
	GROUP BY SourceId,csi.PackageType,csi.PackageId,ll.PackageType

	--SELECT CSI.Id,CustomerId,SourceId,csi.PackageType,csi.PackageId,ll.PackageType AS LLPackageType
	--FROM CustomerSellInquiries CSI WITH(NOLOCK)
	--INNER JOIN LiveListings LL WITH(NOLOCK) ON LL.Inquiryid=CSI.ID and SellerType=2
	--WHERE (CSI.PackageType=31 and (PackageId<>49 OR PackageId IS NULL))
	--OR (CSI.PackageType=30 and (PackageId<>48 OR PackageId IS NULL))
	--OR (CSI.PackageType<>LL.PackageType)
	--OR (CSI.PackageType=31 and SourceId=36 and CSI.ID<>380337)
	--OR (LL.PackageType=2)
END
