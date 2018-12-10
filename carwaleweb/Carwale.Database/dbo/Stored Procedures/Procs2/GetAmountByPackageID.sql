IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAmountByPackageID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAmountByPackageID]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 06/05/2013
-- Description:	get amount for a package
/*
	Declare @PackageID int = 3
	declare @Amount int
	exec GetAmountByPackageID @PackageID,@Amount out
	select @Amount
*/
-- =============================================
CREATE PROCEDURE [dbo].[GetAmountByPackageID] 
	-- Add the parameters for the stored procedure here
	@PackageID int,
	@Amount int out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @Amount = Amount
	FROM Packages WITH (NOLOCK)
	WHERE Id = @PackageID
	    AND InqPtCategoryId = 2
		AND isActive = 1
		AND ForIndividual = 1
		AND IsInternal = 0

END
