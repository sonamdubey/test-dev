IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NanoDriveSaveData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NanoDriveSaveData]
GO

	
CREATE PROCEDURE [dbo].[NanoDriveSaveData]

	@Lattitude		DECIMAL(18,6),
	@Longitude		DECIMAL(18,6)
				
 AS
	
BEGIN
		SELECT ID FROM AP_Processes WHERE ID = 1 AND IsActive = 1
		IF @@ROWCOUNT <> 0
			BEGIN
				UPDATE NanoDrive SET IsCurrent = 0 WHERE IsCurrent = 1
				
				INSERT INTO NanoDrive
				(
					Lattitude, Longitude
				)
				VALUES
				(
					@Lattitude, @Longitude
				)
			END
END


