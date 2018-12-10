IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMUpdateDelearPoint]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMUpdateDelearPoint]
GO

	-- =============================================
-- Author:		<Ranjeet kumar>
-- Create date: <31/10/2013>
-- Description:	< for Validate dealer points and update for MIX n Match buy >
-- =============================================
CREATE PROCEDURE [dbo].[TC_MMUpdateDelearPoint] 
	@DealerId int
	
	AS
BEGIN
	SET NOCOUNT OFF;
	
	UPDATE TC_MMDealersPoint  
	SET CurrentPoint = (CurrentPoint - 5)  
	WHERE CurrentPoint >= 5 and DealerId = @DealerId ;

END
