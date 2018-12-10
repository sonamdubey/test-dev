IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_AP_TeamCapacity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_AP_TeamCapacity]
GO

	



-- =============================================
-- Author:		Dilip
-- Create date: 25th May 2011
-- Description:	This AP for updating Daily Team Capacity Performance in Table "CRM_TeamCapacityLog"
-- =============================================
CREATE PROCEDURE [dbo].[CRM_AP_TeamCapacity]
	
	AS

	BEGIN
		DECLARE @NumberRecords AS INT
		DECLARE @RowCount AS INT
		DECLARE @TotalCount AS INT
		DECLARE @RefType AS INT	
		DECLARE @TotalCapacity AS NUMERIC
		DECLARE @TeamId	AS INT
		DECLARE @TempCapacity Table(RowID NUMERIC IDENTITY(1, 1), TeamId NUMERIC, TCapacity NUMERIC, RType INT)
		
		INSERT INTO @TempCapacity
		SELECT CAM.ID AS TeamId, TotalCapacity, RefType
		FROM CRM_ADM_MTeams AS CAM, CRM_ADM_TeamCapacity AS CAT
		WHERE CAM.ID = CAT.MTeamId ORDER BY RefType
		SET @NumberRecords = @@ROWCOUNT
		SET @RowCount = 1
		
		WHILE @RowCount <= @NumberRecords
		BEGIN
			SELECT @TeamId = TeamId, @RefType = RType, @TotalCapacity = TCapacity 
			FROM @TempCapacity WHERE RowID = @RowCount			
			
			SELECT @TotalCount = COUNT(ItemId) FROM CRM_EventLogs 
			WHERE EventDatePart = CONVERT(Date,GETDATE()) AND EventType = @RefType
			
			INSERT INTO CRM_TeamCapacityLog(Actual, Capacity, EntryDate, TeamId,RefType)
							VALUES(@TotalCount, @TotalCapacity, GETDATE(), @TeamId,@RefType)
			
			SET @RowCount = @RowCount + 1
		END	
		
	END



