IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UndoDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UndoDelete]
GO

	CREATE PROCEDURE [dbo].[CRM_UndoDelete]
@iiID NUMERIC (18,0),
@cbdId NUMERIC (18,0)
AS
BEGIN
	INSERT INTO CRM_ActiveItems (InterestedInId,ItemId,Priority) VALUES (@iiID,@cbdId,5) 
	UPDATE CRM_CarBasicData SET DeleteReasonId = null , IsDeleted  = 0 WHERE ID = @cbdId
END 