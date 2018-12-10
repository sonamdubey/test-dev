IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetUsedCityWDealerList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetUsedCityWDealerList]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:Get the State Wise Dealer List
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetUsedCityWDealerList]
	@CityId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT(D.FirstName + ' ' +D.LastName) AS LastName, D.Address1 as Address, D.ID FROM Dealers as D WITH (NOLOCK) where D.TC_DealerTypeId in (1,3) AND D.IsDealerActive = 1 AND D.IsDealerDeleted = 0 AND D.CityId = @CityId 
END
