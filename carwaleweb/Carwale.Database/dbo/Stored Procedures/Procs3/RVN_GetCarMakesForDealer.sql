IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetCarMakesForDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetCarMakesForDealer]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(9th Oct 2014)
-- Description	:	Get all car makes on which Dealer is working
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetCarMakesForDealer] 
	
	@DealerId	INT
AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT 
			DISTINCT CM.ID AS Value,
			CM.Name AS Text
    FROM	
			CarMakes AS CM
			LEFT JOIN  TC_DealerMakes AS DM ON DM.MakeId=CM.ID AND CM.IsDeleted=0 AND DM.DealerId = @DealerId
			
    ORDER BY Text
END

