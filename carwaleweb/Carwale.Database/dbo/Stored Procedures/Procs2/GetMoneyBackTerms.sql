IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMoneyBackTerms]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMoneyBackTerms]
GO

	

-- =============================================
-- Author:		Amit Verma
-- Create date: 06/05/2013
-- Description:	get amount for a package
/*
	Declare @PackageID int = 44
	declare @Terms VARCHAR(MAX)
	exec GetMoneyBackTerms @PackageID,@Terms out
	select @Terms
*/
-- =============================================
CREATE PROCEDURE [dbo].[GetMoneyBackTerms] 
	-- Add the parameters for the stored procedure here
	@PackageID int,
	@Terms VARCHAR(MAX) out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

     --Insert statements for procedure here
	SELECT @Terms = TermsAndConditions FROM Packages WHERE Id = @PackageID

END


