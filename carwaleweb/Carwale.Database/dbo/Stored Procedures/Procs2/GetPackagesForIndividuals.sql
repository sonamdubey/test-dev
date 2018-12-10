IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPackagesForIndividuals]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPackagesForIndividuals]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 06/05/2013
-- Description:	Get available packages for individuals
-- modificatio: by amit to include InqPtCategoryId = 30
-- =============================================
CREATE PROCEDURE [dbo].[GetPackagesForIndividuals] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT Id,
		NAME,
		Validity,
		Amount,
		Description
	FROM Packages WITH(NOLOCK)
	WHERE InqPtCategoryId in (31,30)
		AND isActive = 1
		AND ForIndividual = 1
		AND IsInternal = 0

END

