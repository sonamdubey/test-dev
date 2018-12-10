IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_UpdateEventDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_UpdateEventDetails]
GO

	-- =============================================
-- Author:	Komal Manjare
-- Create date: 26-October-2016
-- Description:	delete or activate event details
-- Modifier : Kartik Rathod on 26 oct 2016, update isactive and isdeleted for events
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_UpdateEventDetails]
	@EventId INT=NULL,
	@EventImageId INT=NULL,
	@IsActive BIT = NULL,
	@IsDeleted BIT = NULL
AS
BEGIN
	If(ISNULL(@EventId,0) > 0)	-- for events
	BEGIN
		UPDATE	Microsite_DealerEvents
		SET		IsActive = ISNULL(@IsActive,IsActive), IsDeleted = ISNULL(@IsDeleted,IsDeleted),UpdatedOn=getdate()
		WHERE	Id=@EventId
		
	END

	IF(ISNULL(@EventImageId,0) > 0)-- for images
	BEGIN
		UPDATE Microsite_EventImages
		SET IsActive = 0,UpdatedOn = getdate()
		WHERE ID = @EventImageId 

	END
END



