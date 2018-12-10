IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetStateAndAllCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetStateAndAllCities]
GO

	-- =============================================
-- Author	:	Sachin Bharti on 11/03/16
-- Description	:	Get state and all cities of that state based on cityId
-- execute [dbo].[GetStateAndAllCities] 1
-- =============================================
CREATE PROCEDURE [dbo].[GetStateAndAllCities]
	@CityId INT
AS
BEGIN
SET NOCOUNT  ON 
	DECLARE @StateId INT
	SELECT @StateId=StateId FROM Cities(NOLOCK) where ID=@CityId

	SELECT DISTINCT ISNULL(S.ID,0) AS Id ,S.Name
	From States S(NOLOCK) 
	WHERE S.Id = @StateId AND S.IsDeleted=0

	SELECT ISNULL(C.ID,0) AS Id,C.Name 
	From Cities C(NOLOCK) 
	WHERE C.StateId = @StateId AND C.IsDeleted=0
	ORDER BY  C.Name
END
