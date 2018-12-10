IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_CancelWarranty]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_CancelWarranty]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 12th Feb 2015
-- Description : To update cancel warranty reqests in database.
-- =============================================
CREATE PROCEDURE [dbo].[Absure_CancelWarranty] 
	-- Add the parameters for the stored procedure here
	@AbsureCarDetailsId BIGINT,
	@StatusId           TINYINT,
	@Reason				VARCHAR(1000),
	@CancelledBy        BIGINT	
AS
BEGIN
	
	--Removed Status and added IsCancelled = 1 by Deepak on 13th March 2015
	UPDATE AbSure_CarDetails SET CancelReason = @Reason,CancelledBy = @CancelledBy,CancelledOn = GETDATE(), IsCancelled = 1
	WHERE Id = @AbsureCarDetailsId 

END
