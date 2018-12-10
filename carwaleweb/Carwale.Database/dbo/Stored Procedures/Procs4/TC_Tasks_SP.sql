IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Tasks_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Tasks_SP]
GO

	-- =============================================
-- ModifiedBy:		Binumon George
-- Create date: 08-11-2011
-- Description:	Added ModifiedBy parameter and modified date.
-- =============================================
-- =============================================
-- Author:		Binumon George
-- Create date: 14-06-2011
-- Description:	Task details of under a dealer.
-- =============================================
	CREATE PROCEDURE [dbo].[TC_Tasks_SP] 
	-- Add the parameters for the stored procedure here
	@Id  SMALLINT = NULL ,
	@TaskName VARCHAR(50),
	@TaskDescription VARCHAR(200),
	@ModifiedBy INT,
	@Status INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0 -- Default staus will be 0.Insertion or updation not happened status will be 0
    -- Insert statements for procedure here
	IF(@Id IS NULL)--IF id parameter is null, we inserting new task to TC_Tasks table
		BEGIN
			IF NOT EXISTS(SELECT top 1 Id FROM TC_Tasks WHERE TaskName = @TaskName)
				BEGIN
					INSERT INTO TC_Tasks(TaskName, TaskDescription, ModifiedBy)
					VALUES(@TaskName, @TaskDescription, @ModifiedBy)
					set @Status=1
				END
		END
	Else --IF id contaiing data, we updatig task information
		BEGIN
			UPDATE TC_Tasks set TaskName=@TaskName, TaskDescription=@TaskDescription, 
			ModifiedDate=GETDATE(),ModifiedBy=@ModifiedBy WHERE Id = @Id
			set @Status=2
		END	
END





