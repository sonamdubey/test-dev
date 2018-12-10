IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_TransferDealersToEXEC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_TransferDealersToEXEC]
GO

	
CREATE PROCEDURE [dbo].[DCRM_TransferDealersToEXEC]
	@DealerIds		AS VARCHAR(1000),
	@NewUserId      AS NUMERIC,
	@UpdatedBy      AS NUMERIC = 13,
	@RoleId         AS NUMERIC,
	@Result 		INT OUTPUT
AS
	
BEGIN
	DECLARE @DealerID VARCHAR(50) 
	DECLARE @DealerIndx VARCHAR(50)
	SET		@Result = -1
								
	IF @DealerIds <> ''
		BEGIN
			SET @DealerIds =  @DealerIds + ',' 	  
			WHILE @DealerIds <> ''
				BEGIN
					SET @DealerIndx = CHARINDEX(',' , @DealerIds)
					IF  @DealerIndx > 0
					   BEGIN 
							SET @DealerID = LEFT(@DealerIds, @DealerIndx-1)  
							SET @DealerIds = RIGHT(@DealerIds, LEN(@DealerIds)- @DealerIndx)
						  
							-- Transfer Dealer to field executrives either Sales ro Serivice
							EXEC DCRM_TransferToFieldExec @DealerID,@NewUserId,@UpdatedBy,@RoleId
							
							SET @Result = 1
						END
				END
		END
END
