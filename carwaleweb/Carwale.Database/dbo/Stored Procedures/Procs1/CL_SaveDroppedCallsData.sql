IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CL_SaveDroppedCallsData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CL_SaveDroppedCallsData]
GO

	


CREATE PROCEDURE [dbo].[CL_SaveDroppedCallsData]
	
	@CalledNumber		VARCHAR(50),
	@StartTime			DATETIME,
	@NodeData			VARCHAR(250),
	@BusinessProcess	VARCHAR(50),
	@CallType			SMALLINT,
	@TypePath			VARCHAR(250),
	@NewDataId			Numeric OutPut	
				
 AS
	
BEGIN
	SET @NewDataId = -1
	BEGIN

		INSERT INTO CL_DroppedCalls
		(
			CalledNumber, StartTime, NodeData, BusinessProcess, CallType, TypePath
		)
		VALUES
		(
			@CalledNumber, @StartTime, @NodeData, @BusinessProcess, @CallType, @TypePath
		)
			
		SET @NewDataId = SCOPE_IDENTITY()
			
	END
END


