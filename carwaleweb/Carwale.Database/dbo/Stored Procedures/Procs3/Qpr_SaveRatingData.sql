IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Qpr_SaveRatingData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Qpr_SaveRatingData]
GO

	-- =============================================
-- Author:		Kartik Rathod 
-- Create date: 5th Jan 2016
-- Description:	to save the data of the QprRating form
-- =============================================
CREATE PROCEDURE [dbo].[Qpr_SaveRatingData] 
	@UserId					INT ,
	@ManagerId				INT				= NULL,
	@Mission				VARCHAR(1000)	= NULL,
	@StartTime				DATETIME	= NULL,
	@OutcomesTbl			[dbo].[Qpr_OutcomesTblTyp] READONLY,
	@QuestionResponseTbl	[dbo].[Qpr_QuestionsResponseTblTyp] READONLY,
	@IsSubmitted			BIT				= Null,
	@Type					SMALLINT		= Null,
	@EmployeeId				VARCHAR(20)		= NULL,
	@DepartmentId			INT				= NULL,
	@DesignationId			INT				= NULL,
	@ExtraAchieved			VARCHAR(1000)   =NULL,			
	@Result					BIT	OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @RowCnt INT,@RatingDataId INT;
	SET @Result = 0
	
	IF(SELECT UserId FROM Qpr_RatingData WITH(NOLOCK) WHERE UserId=@UserId AND Type=@Type) IS NULL
	BEGIN

		INSERT INTO Qpr_RatingData ( UserId , ManagerId , StartTime ,EndTime ,Mission ,IsSubmitted ,Type,EmployeeId ,DepartmentId,DesignationId,ExtraAchieved )
		VALUES					   ( @UserId, @ManagerId, @StartTime,GETDATE(),@Mission,@IsSubmitted, @Type,@EmployeeId,@DepartmentId,@DesignationId,@ExtraAchieved)

		SET @RowCnt  = @@ROWCOUNT

		SET @RatingDataId = SCOPE_IDENTITY()

		IF @RowCnt = 1
			BEGIN	
				INSERT INTO Qpr_Outcomes (Qpr_RatingDataId ,KRA ,KPI ,Weightage ,SelfScore )
				SELECT						@RatingDataId,KRA ,KPI ,Weightage ,SelfScore
				FROM		@OutcomesTbl	
					
					
				INSERT INTO Qpr_QuestionsResponse  (Qpr_RatingDataId,Question_Id ,Comments ,Qpr_ResponseValuesId)										
				SELECT								@RatingDataId,Question_Id ,Comments ,Qpr_ResponseValuesId
				FROM		@QuestionResponseTbl

				SET @Result = 1
				
			END
	END
	return @Result
END
