IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetLocalTaxData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetLocalTaxData]
GO

	
-- =============================================
-- Author:		<Prashant Vishe>
-- Create date: <16 Oct 2013>
-- Description:	<For retrieving local tax data.>
-- =============================================
CREATE PROCEDURE [dbo].[GetLocalTaxData]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT PL.Id
		,PL.Rate
		,PL.Description
		,PL.IsTaxOnTax
		,PCI.CategoryName AS TaxType
		,C.NAME AS City
	FROM PriceQuote_LocalTax PL
		,PQ_CategoryItems PCI
		,Cities C
	WHERE C.Id = PL.CityId
		AND PL.CategoryItemid = PCI.Id
	ORDER BY C.NAME
END
