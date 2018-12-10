IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUsedCarDealersForState]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUsedCarDealersForState]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(10th Sept 2014)
-- Description	:	Get dealers belong to that state only
-- =============================================
CREATE PROCEDURE [dbo].[GetUsedCarDealersForState]
	
	@StateId	INT

AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT DISTINCT D.ID AS Value , D.Organization + ' ( '+CAST(D.ID AS varchar(10)) + ' ) 'AS Text 
	FROM Dealers D(NOLOCK) WHERE StateId = @StateId ORDER BY Text
	
END

