IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveLocalTaxData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveLocalTaxData]
GO

	
-- =============================================
-- Author:		<Prashant Vishe>
-- Create date: <16 Oct 2013>
-- Description:	<For retrieving local tax data>
-- =============================================
CREATE PROCEDURE [dbo].[RetrieveLocalTaxData]
	-- Add the parameters for the stored procedure here
	@Id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT PL.*
		,C.StateId
	FROM PriceQuote_LocalTax PL
		,Cities C
	WHERE PL.CityId = C.Id
		AND PL.Id = @Id
END
