IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateDealerModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateDealerModels]
GO

	-- =============================================
-- Author:		Mihir A.Chheda
-- Create date: 27-05-2015
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateDealerModels]
@RowId INT,
@StatusCode INT,
@Status INT  OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF(@StatusCode=0)
	BEGIN
	   UPDATE TC_DealerModels 
	   SET    IsDeleted=1,ModifiedDate=GETDATE()
	   WHERE  ID=@RowId
	   SET    @Status=1
	END
	ELSE
	BEGIN
		UPDATE TC_DealerModels 
		SET    IsDeleted=0,ModifiedDate=GETDATE()
		WHERE  ID=@RowId
		SET    @Status=0
	END

END
