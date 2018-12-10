IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ChangeListingPackage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ChangeListingPackage]
GO

	-- =============================================
-- Author:		AMIT VERMA
-- Create date: 10/31/2013
-- Description:	Change package for a listing
-- =============================================
CREATE PROCEDURE [dbo].[ChangeListingPackage]
	-- Add the parameters for the stored procedure here
    @CarId NUMERIC(18,0),
    @ConsumerId NUMERIC(18,0),
	@PackageID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CustomerSellInquiries
	SET PackageId = @PackageID, PackageType = 1
	WHERE ID = @CarId AND CustomerId = @ConsumerId
	--EXEC [dbo].[UpgradePackageTypeToListingType] 2,@CarId,@ConsumerId,0
END

