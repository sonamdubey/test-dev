IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CancelDealerMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CancelDealerMapping]
GO

	-- =============================================
-- Author:		Tejashree Patil.
-- Create date: 10 Dec 2013.
-- Description:	This sp is used to cancel dealer mapping specially done for Inventory.
-- EXEC TC_CancelDealerMapping 6703,1,3
-- =============================================
CREATE PROCEDURE [dbo].[TC_CancelDealerMapping]
	@TC_DealerMappingId  INT,
	@UserId INT
AS
BEGIN
	
	--For Admin dealer	
	UPDATE	TC_DealerMapping 	
	SET		IsActive = 0, ModifiedBy = @UserId, ModifiedDate = GETDATE()
	WHERE	TC_DealerMappingId = @TC_DealerMappingId

	--For Sub dealers
	UPDATE	TC_SubDealers 	
	SET		IsActive = 0, ModifiedDate = GETDATE()
	WHERE	TC_DealerMappingId = @TC_DealerMappingId


END

