IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_UpdateDealerModelsCategoryMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_UpdateDealerModelsCategoryMapping]
GO

	-- =============================================
-- Author:		Mihir A.Chheda
-- Create date: 27-05-2015
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_UpdateDealerModelsCategoryMapping]
@RowId INT,
@StatusCode INT,
@Status INT  OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF(@StatusCode=1)
	BEGIN
	   UPDATE Microsite_DWModelFeatureCategories 
	   SET    IsActive=0,ModifiedDate=GETDATE()
	   WHERE  ID=@RowId
	   SET    @Status=0
	END
	ELSE
	BEGIN
		UPDATE Microsite_DWModelFeatureCategories 
		SET    IsActive=1,ModifiedDate=GETDATE()
		WHERE  ID=@RowId
		SET    @Status=1
	END

END
