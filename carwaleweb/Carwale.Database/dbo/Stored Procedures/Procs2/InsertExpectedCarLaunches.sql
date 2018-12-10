IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertExpectedCarLaunches]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertExpectedCarLaunches]
GO
	
--THIS PROCEDURE IS FOR INSERTING RECORDS FOR Class TABLE

CREATE PROCEDURE [dbo].[InsertExpectedCarLaunches]
	@ID			NUMERIC,
	@CarMakeId		NUMERIC,
	@sort			INT,
	@ModelName		VARCHAR(250),
	@ExpectedLaunch	VARCHAR(250),	
	@EstimatedPrice	VARCHAR(250),
	@Description		VARCHAR(4000),
	@SpecificationData	VARCHAR(3000),
	@CarwaleViews		VARCHAR(1000),
	@PhotoName		VARCHAR(100),
	@DiscussionId		NUMERIC -- Blog Entry ID where this car is going to be discussed

 AS
	BEGIN


	IF @ID = -1

		BEGIN

			INSERT INTO ExpectedCarLaunches (CarMakeId,ModelName,ExpectedLaunch,EstimatedPrice,Description,SpecificationData,CarwaleViews,PhotoName,DiscussionId,Sort)
			VALUES(@CarMakeId,@ModelName,@ExpectedLaunch,@EstimatedPrice,@Description,@SpecificationData,@CarwaleViews,@PhotoName,@DiscussionId,@sort)
					
		END
	ELSE
		
		BEGIN
			UPDATE ExpectedCarLaunches SET 
			
			CarMakeId		=		@CarMakeId,
			ModelName		=		@ModelName,            
			ExpectedLaunch	=		@ExpectedLaunch, 
			EstimatedPrice		=		@EstimatedPrice,
			Description		=		@Description,
			SpecificationData 	=	 	@SpecificationData,
			CarwaleViews  		=  		@CarwaleViews,
			PhotoName   		= 		@PhotoName,
			DiscussionId  		=		@DiscussionId,
			Sort			=		@Sort
			WHERE 
					ID=@ID
	
		END
END
