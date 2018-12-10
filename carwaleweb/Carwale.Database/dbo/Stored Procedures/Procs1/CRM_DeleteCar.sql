IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DeleteCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DeleteCar]
GO

	-- =============================================
-- Author:		Amit Kumar
-- Create date: 7th feb 2013
-- Description:	Used to delete the car from CRM_ActiveItems and update CRM_CarbasicData (isDeleted = 1)
-- EXEC CRM_DeleteCar 869,'10650,10651',7
-- =============================================
CREATE PROCEDURE [dbo].[CRM_DeleteCar] 
	@InterestedInId NUMERIC(18,0),
	@ItemId			VARCHAR(500),
	@deleteReason	INT,
	@IsUpdated      TINYINT = 0 OUTPUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE FROM CRM_ActiveItems
	WHERE InterestedInId = @InterestedInId AND ItemId IN (SELECT ListMember FROM fnSplitCSV(@ItemId))
	
	UPDATE CRM_CarBasicData SET IsDeleted = 1,DeleteReasonId=@deleteReason WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@ItemId))

	SET @IsUpdated = 1
END
