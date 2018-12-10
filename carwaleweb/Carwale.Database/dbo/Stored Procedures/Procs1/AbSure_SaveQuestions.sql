IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveQuestions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveQuestions]
GO

	-- ===============================================
-- Author:		Yuga Hatolkar
-- Create date: 18/12/2014
-- Description:	Save AbSure Questions
-- ===============================================
CREATE PROCEDURE [dbo].[AbSure_SaveQuestions]

@CategoryId INT,
@SubCategoryId INT,
@PositionId INT,
@AreaId INT,
@WeightageId INT,
@Question VARCHAR(1000),
@TypeId INT,
@IsActive BIT,
@UpdatedBy INT,
@UpdatedOn DATETIME,
@Id INT

AS
	--Weightage 1 => 1
	--Weightage 2 => 2
	--Weightage 3 => CTQ-G
	--Weightage 4 => CTQ
	--Type 1      => Yes/No
	--Type 2      => Number

	BEGIN	
		IF (@Id = -1)
		BEGIN
		 INSERT INTO AbSure_Questions(Question, AbSure_QCategoryId, AbSure_QSubCategoryId, AbSure_QPositionId,
		 AbSure_QAreaId, Type, Weightage, IsActive, UpdatedBy, UpdatedOn) VALUES(@Question,
		 @CategoryId, @SubCategoryId, @PositionId, @AreaId,@TypeId, @WeightageId, @IsActive, @UpdatedBy, @UpdatedOn)
		 END

		 ELSE
		 UPDATE AbSure_Questions 
		 SET Question = @Question, AbSure_QCategoryId = @CategoryId, AbSure_QSubCategoryId = @SubCategoryId,
		 AbSure_QPositionId = @PositionId, AbSure_QAreaId = @AreaId, Type = @TypeId, Weightage = @WeightageId,
		 UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE AbSure_QuestionsId = @Id

END
