IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Opr_AddNewTAsk]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Opr_AddNewTAsk]
GO

	
/*
	--Author  : Sachin Bharti(7th March 2012)
	--Purpose :	To add new task under the Master Task and made new entry in trk_Subtasks
*/
CREATE Procedure [dbo].[Opr_AddNewTAsk]
@MasterTaskNo	INT ,
@SubTaskID	INT ,
@Description	VARCHAR(300) = NULL,
@StartDate		DATETIME,
@EstimatedEndDate	DATETIME,
@AssignedBy		INT = NULL,
@AssignedDate	DATETIME = NULL,
@Comment		VARCHAR(MAX),
@AllUsers		VARCHAR(MAX),
@Type			INT,
@IsAdded		NUMERIC(18,0) = -1 OUTPUT -- Used to set output value if new task is added in


AS
BEGIN
	
	DECLARE @UserId VARCHAR(50) 
    DECLARE @UserIdIndx VARCHAR(50)
	DECLARE @SubTaskNo		INT
	IF ( @Type =1 )
	BEGIN 
		INSERT INTO trk_SubTasks (MasterTaskNo,Descr,StartDate,EstimatedEndDate,AssignedBy,AssignedDate,Comment,IsActive ) 
			Values(@MasterTaskNo,@Description,@StartDate,@EstimatedEndDate,@AssignedBy,@AssignedDate,@Comment,1 )
		IF @@ROWCOUNT <> 0
			BEGIN
				SET @IsAdded = @MasterTaskNo
				SET @SubTaskNo = SCOPE_IDENTITY()
				SET @AllUsers =  @AllUsers + ',' 	  
					WHILE @AllUsers <> ''
						BEGIN
							SET @UserIdIndx = CHARINDEX(',' , @AllUsers)
							IF  @UserIdIndx > 0
								BEGIN 
									SET @UserId = LEFT(@AllUsers, @UserIdIndx-1)  
									SET @AllUsers = RIGHT(@AllUsers, LEN(@AllUsers)- @UserIdIndx)
									IF @UserId IS NOT NULL
										BEGIN
											INSERT INTO trk_TaskUsers ( TaskNo,TaskOwner) VALUES (@SubTaskNo,@UserId )
										END
								END
						END
			END	
	END
	ELSE IF (@Type = 2)
	BEGIN 
		UPDATE trk_SubTasks SET Descr = @Description,StartDate = @StartDate ,EstimatedEndDate =@EstimatedEndDate ,AssignedBy = @AssignedBy 
			,AssignedDate = @AssignedDate ,Comment = @Comment WHERE TaskNo = @SubTaskID
		IF @@ROWCOUNT <> 0
			BEGIN
				DELETE FROM trk_TaskUsers WHERE TaskNo = @SubTaskID
				SET @IsAdded = @MasterTaskNo
				SET @SubTaskNo = SCOPE_IDENTITY()
				SET @AllUsers =  @AllUsers + ',' 	  
					WHILE @AllUsers <> ''
						BEGIN
							SET @UserIdIndx = CHARINDEX(',' , @AllUsers)
							IF  @UserIdIndx > 0
								BEGIN 
									SET @UserId = LEFT(@AllUsers, @UserIdIndx-1)  
									SET @AllUsers = RIGHT(@AllUsers, LEN(@AllUsers)- @UserIdIndx)
									IF @UserId IS NOT NULL
										BEGIN
											INSERT INTO trk_TaskUsers ( TaskNo,TaskOwner) VALUES (@SubTaskID,@UserId )
										END
								END
						END
			END	
	END
	
END
