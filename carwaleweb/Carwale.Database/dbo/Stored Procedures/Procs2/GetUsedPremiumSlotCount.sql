IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUsedPremiumSlotCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUsedPremiumSlotCount]
GO

	-- =============================================
-- Created date : 19 Feb, 2016
-- Description   : To Get Slot Count From City id
-- Created By	:	Sahil Sharma
-- EXEC [dbo].[GetUsedPremiumSlotCount] 1
-- =============================================
create PROCEDURE [dbo].[GetUsedPremiumSlotCount] 
	@CityID INT
AS
BEGIN
SELECT [Count]
FROM UsedPremiumSlotCount WITH(NOLOCK)
where CityId = @CityID
END