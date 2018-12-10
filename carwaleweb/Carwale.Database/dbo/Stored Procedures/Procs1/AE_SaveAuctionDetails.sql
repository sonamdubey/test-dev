IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_SaveAuctionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_SaveAuctionDetails]
GO

	

--Procedure Created By Sentil/Deepak On 9/11/2009
--This Procedure is used to for Insert and Update on table AE_AuctionDetails

CREATE PROCEDURE [dbo].[AE_SaveAuctionDetails]
(
	@Id AS BIGINT = 0,
	@Title AS VARCHAR(50) = NULL,
	@StartDate AS DATETIME = NULL,
	@EndDate AS DATETIME = NULL,
	@InspectionStartDate AS DATETIME = NULL,
	@InspectionEndDate AS DATETIME = NULL,
	@IsActive AS BIT = 0,
	@YardId AS NUMERIC(18,0) = 0,
	@IsCompleted AS BIT = NULL,
	@CreatedOn AS DATETIME = NULL,
	@UpdatedOn AS DATETIME = NULL,
	@UpdatedBy AS NUMERIC(18,0) = 0,
	@retID AS BIGINT OUT
)
AS
BEGIN

	IF(@ID = -1)
		BEGIN
			INSERT INTO 
			AE_AuctionDetails
			(
				Title, StartDate, EndDate, InspectionStartDate,	InspectionEndDate, IsActive, 
				YardId, IsCompleted, CreatedOn, UpdatedBy			
			)
			VALUES
			(
				@Title, @StartDate, @EndDate, @InspectionStartDate,	@InspectionEndDate, @IsActive, 
				@YardId, @IsCompleted, @CreatedOn, @UpdatedBy
			)
			
			SET @retID = SCOPE_IDENTITY()
		END
	ELSE	
		BEGIN
		
			UPDATE 
			AE_AuctionDetails 
			SET Title = @Title, StartDate = @StartDate, EndDate = @EndDate, 
				InspectionStartDate = @InspectionStartDate,	InspectionEndDate = @InspectionEndDate, IsActive = @IsActive, 
				YardId = @YardId, IsCompleted = @IsCompleted, 
				UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy
			WHERE Id =  @Id				
			
			SET @retID = @Id
		END
	
--SELECT * FROM AE_AuctionDetails
--TRUNCATE TABLE AE_AuctionDetails
	
END




