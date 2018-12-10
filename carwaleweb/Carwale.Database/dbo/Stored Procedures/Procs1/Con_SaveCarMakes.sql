IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_SaveCarMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_SaveCarMakes]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Modified by:  Manish on 25-07-2014 commented sp for sync src_keyword table since this activity will be perform by Schedul job.
-- Modified By : Vinay Kumar Prajapati Return MakeId when New make Inserted.
-- =============================================
CREATE PROCEDURE [dbo].[Con_SaveCarMakes]
	-- Add the parameters for the stored procedure here
	@ID INT,
	@Name VARCHAR(MAX) = NULL,
	@MaUpdatedBy INT,
	@IsDeleted BIT = 0,
	@ReturnMakeId INT = -1  OUTPUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @CurDate Datetime;
    -- Insert statements for procedure here
	IF(@IsDeleted = 1)
	BEGIN
		set @CurDate=getdate();
		UPDATE CarMakes SET IsDeleted = 1,MaUpdatedOn = @CurDate, MaUpdatedBy = @MaUpdatedBy WHERE ID = @ID
	    SET @ReturnMakeId = @ID
		--EXEC [ac].[UpdateKeywordsByMakeID] @ID,3  -- Commented by Manish on 25-07-2014
		begin try
				exec SyncCarMakesWithMysqlUpdate @ID, null,1,null,@CurDate,@MaUpdatedBy,1;
		end try
		BEGIN CATCH
			INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
			VALUES('MysqlSync','Con_SaveCarMakes',ERROR_MESSAGE(),'SyncCarMakesWithMysqlUpdate',@Id,GETDATE(),1)
		END CATCH			
	END
	ELSE
	BEGIN
		IF(@ID = -1)
		BEGIN
			set @CurDate=getdate();
			INSERT INTO CarMakes( Name, IsDeleted,MaCreatedOn,MaUpdatedBy )
			VALUES (@Name, @IsDeleted, @CurDate, @MaUpdatedBy)
			SET @ReturnMakeId = SCOPE_IDENTITY()
		begin try
			-- sync with mysql 
			exec SyncCarMakesWithMysql @ReturnMakeId,@Name,@IsDeleted,@CurDate,@MaUpdatedBy;
		end try
		BEGIN CATCH
			INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
			VALUES('MysqlSync','Con_SaveCarMakes',ERROR_MESSAGE(),'SyncCarMakesWithMysql',@ReturnMakeId,GETDATE(),null)
		END CATCH	
		--	EXEC [ac].[UpdateKeywordsByMakeID] @@IDENTITY,1  -- Commented by Manish on 25-07-2014
		END
		ELSE
		BEGIN
			set @CurDate=getdate();
			UPDATE CarMakes SET
				Name = @Name,
				IsDeleted = @IsDeleted,
				MaCreatedOn = @CurDate,
				MaUpdatedBy = @MaUpdatedBy
			WHERE ID = @ID
			SET @ReturnMakeId = @ID
			--EXEC [ac].[UpdateKeywordsByMakeID] @ID,2 -- Commented by Manish on 25-07-2014
		begin try
			exec SyncCarMakesWithMysqlUpdate @ID, @Name,@IsDeleted,@CurDate,null,@MaUpdatedBy,2;
		end try
		BEGIN CATCH
			INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
			VALUES('MysqlSync','Con_SaveCarMakes',ERROR_MESSAGE(),'SyncCarMakesWithMysqlUpdate',@ID,GETDATE(),2)
		END CATCH	
		END
	END
END

