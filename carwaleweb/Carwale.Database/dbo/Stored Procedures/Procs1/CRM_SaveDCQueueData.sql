IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveDCQueueData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveDCQueueData]
GO

	-- =============================================
-- Author      : Chetan Navin		
-- Create date : 3-01-2013
-- Description : To Save DC queue data 
-- Module      : NEW CRM
-- EXEC CRM_SaveDCQueueData -1,5793,0,-1,GETDATE(),7,-1,GETDATE(),0,-1,GETDATE(),0
-- Vaibhav K (14-1-2013) : Initialize @ReturnId = -1 initially
-- =============================================
CREATE PROCEDURE [dbo].[CRM_SaveDCQueueData] 
	@Id               INT,
	@CBDId            BIGINT,
	@PQStatus         SMALLINT,
	@PQSubDisposition INT,
	@PQDate			  DATETIME,
	@TDStatus         SMALLINT,
	@TDSubDisposition INT,
	@TDDate			  DATETIME,
	@BLStatus         SMALLINT,
	@BLSubDisposition INT,
	@BLDate			  DATETIME,
	@RegisterWith     VARCHAR(50),
	@CarColor         VARCHAR(20),
	@Invoice          INT,
	@IsProcessed      BIT = 0,
	@CreatedBy        BIGINT,
	@UpdatedBy        BIGINT = NULL,
	@UpdatedOn        DATETIME = NULL,
	@ReturnId         BIGINT OUTPUT
AS

BEGIN
	SET @ReturnId = -1

	IF(@Id = -1)
		BEGIN
			INSERT INTO CRM_DCQueueData (CBDId,PQStatus,PQSubDisposition,PQDate,TDStatus,TDSubDisposition,TDDate,
			            BLStatus,BLSubDisposition,BLDate,RegisterWith,CarColor,Invoice,IsProcessed,UpdatedBy,UpdatedOn,CreatedBy)
			VALUES (@CBDId,@PQStatus,@PQSubDisposition,@PQDate,@TDStatus,@TDSubDisposition,@TDDate,@BLStatus,
					@BLSubDisposition,@BLDate,@RegisterWith,@CarColor,@Invoice,@IsProcessed,@UpdatedBy,@UpdatedOn,@CreatedBy)
		END
	ELSE
		BEGIN
			UPDATE CRM_DCQueueData SET IsProcessed = 1 WHERE Id = @Id 
		END
	SET @ReturnId = SCOPE_IDENTITY();
END

