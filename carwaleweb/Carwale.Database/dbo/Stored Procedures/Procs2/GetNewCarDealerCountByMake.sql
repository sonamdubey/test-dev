IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerCountByMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerCountByMake]
GO

	
-- =============================================          
-- Author:  <Vinayak>          
-- Description: <Get the count of new car dealers>
-- =============================================          
CREATE PROCEDURE [dbo].[GetNewCarDealerCountByMake]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT CM.ID AS MakeId,CM.Name AS MakeName,COUNT(DN.Id) AS [DealerCount] FROM CarMakes CM WITH (NOLOCK)
	INNER JOIN  Dealer_NewCar DN WITH (NOLOCK)
	ON CM.ID = DN.MakeId
	WHERE CM.IsDeleted=0
	AND DN.IsActive=1
	GROUP BY CM.Name,CM.ID
END



