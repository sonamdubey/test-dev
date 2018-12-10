IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CT_SaveDealersMigration]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CT_SaveDealersMigration]
GO

	-- =============================================
-- Author:		Kartik Rathod
-- Create date: 15 july 2016
-- Description:	this sp saves carwale dealer id for cartrade migration process eg set IsMigrated flag tu false
-- Modifier:	Vaibhav K 26 July 2016
-- get the ismigrated flag for the dealer before updation
-- if null update it to 0 and return the initial status
-- null:request taken for updation,0:request already queued,1:dealer already migrated
-- =============================================
CREATE PROCEDURE [dbo].[CT_SaveDealersMigration] 
	@CWDealerID BIGINT
AS
BEGIN
	/*declare @TransferID BIGINT 
	SELECT @TransferID = ID FROM CWCTDealerMapping WITH(NOLOCK) WHERE CWDealerID = @CWDealerID
	IF(@TransferID) IS NULL  -- @TransferID is null then only insert CWDealerId in CT_CWDealersTransfer Table
	BEGIN
		INSERT INTO CWCTDealerMapping (CWDealerId,IsMigrated)
		VALUES	(@CWDealerID,0)
	END
	
	SELECT CASE WHEN @TransferID IS NULL THEN 1 ELSE 0 END AS IsDealerAdded		-- @TransferID is null then @CWDealerID successfully inserted in CWCTDealerMapping table.
	*/
	-- Vaibhav K 26 July 2016 commented above code and built the logic below
	DECLARE @IsDealerMigrated SMALLINT = -1
	
	SELECT @IsDealerMigrated = IsMigrated FROM CWCTDealerMapping WITH(NOLOCK) WHERE CWDealerID = @CWDealerID
	IF @IsDealerMigrated IS NULL
		BEGIN
		declare @curDate datetime = Getdate();
			UPDATE CWCTDealerMapping SET IsMigrated = 0, MigrationRequestDate = @curDate WHERE CWDealerID = @CWDealerID
		begin try
			exec SyncCWCTDealerMappingWithMysqlUpdate @CWDealerID,null,@curDate,null,null,null,null,null,null,0,2;
		end try
		BEGIN CATCH
			INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
			VALUES('MysqlSync','CT_SaveDealersMigration',ERROR_MESSAGE(),'SyncCWCTDealerMappingWithMysqlUpdate',@CWDealerID,GETDATE(),2)
		END CATCH			
		END
	SELECT @IsDealerMigrated IsDealerMigrated
END

