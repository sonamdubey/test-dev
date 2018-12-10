IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveEventLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveEventLog]
GO

	--EXEC CRM_SaveEventLog -1,4,'10656,10657','2014-01-29 14:57:34.270',3,1
--Modified By : Chetan Navin - 29 Jan 2014 (Multiple itemId insertion)
CREATE PROCEDURE [dbo].[CRM_SaveEventLog]
	
	@Id				Numeric,
	@EventType		SmallInt,
	@ItemId			VARCHAR(500),
	@EventOn		DateTime,
	@EventBy		Numeric,
	@currentId		INT OUTPUT
				
 AS

BEGIN
	IF @Id = -1
	BEGIN
		INSERT INTO CRM_EventLogs
		(
			EventType, ItemId, EventOn, EventBy
		)
		SELECT @EventType, ListMember, GETDATE(), @EventBy
		FROM fnSplitCSV_WithId(@ItemId)	

		SET @currentId = 1
	END
END









