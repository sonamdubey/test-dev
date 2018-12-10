IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPackageDetails_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPackageDetails_SP]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 17 May 2013
-- Description:	Proc to get package details of customers
-- =============================================
create PROCEDURE [dbo].[GetPackageDetails_SP]
	@PackageId INT,
	@PackageName VARCHAR(50) OUTPUT,
	@PackageValidity INT OUTPUT,
	@PackageAmount INT OUTPUT,
	@PackageDescription VARCHAR(1000) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
		@PackageName = P.Name, @PackageValidity = P.Validity, 
		@PackageAmount = P.Amount, @PackageDescription = P.Description		
    FROM Packages P
    WHERE P.Id = @PackageId AND P.isActive = 1
	
END
