IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealersCities_ByMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealersCities_ByMake]
GO

	
-- =============================================          
-- Author:  Kritika Choudhary on 19th June 2015
         
-- =============================================          
CREATE PROCEDURE [dbo].[GetDealersCities_ByMake] (
	@MakeId INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT C.Id AS Value, 
	C.Name AS Text, COUNT(DNC.Id) AS TotalBranches, 
	S.Name AS [State], S.ID AS StateId,
	ROW_NUMBER() Over(Partition By StateId Order by StateId) AS StateRank 
	FROM Dealer_NewCar AS DNC, Cities AS C, States AS S 
	WHERE DNC.CityId = C.Id AND C.StateId = S.ID AND DNC.IsActive = 1 AND DNC.IsNewDealer = 1
	AND C.IsDeleted = 0 AND DNC.MakeId =@MakeId 
	GROUP By C.Id, C.Name, S.Name, S.ID, StateId 
	Order By Text

END


