IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerMixMatchPoints]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerMixMatchPoints]
GO

	--========================================================
-- Author		: Suresh Prajapati
-- Created Date : 1st Oct, 2015
-- Description	: To Get Dealer's MixMatch Points
-- exec TC_GetDealerMixMatchPoints 1
--========================================================
CREATE PROCEDURE TC_GetDealerMixMatchPoints @DealerId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CurrentPoint AS CurrentPoints
	FROM TC_MMDealersPoint WITH (NOLOCK)
	WHERE DealerId = @DealerId
END


