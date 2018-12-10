IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_SaveAuctionCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_SaveAuctionCars]
GO

	
--Procedure Created By Sentil/Deepak On 9/11/2009
--This Procedure is used to for Insert and Update on table AE_AuctionCars 

CREATE PROCEDURE [dbo].[AE_SaveAuctionCars]
(
	@Id AS BIGINT = 0,
	@AuctionId AS NUMERIC(18,0) = 0,
	@CarId AS NUMERIC(18,0) = 0,
	@BasePrice AS NUMERIC(18,0) = 0,
	@ReservePrice AS NUMERIC(18,0) = 0,
	@StartDateTime AS DATETIME = NULL,
	@InitialClosingTime AS DATETIME = NULL,
	@FinalClosingTime AS DATETIME = NULL,
	@StatusId AS SMALLINT = NULL,
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
			AE_AuctionCars
			(
				AuctionId, CarId, BasePrice, StartDateTime, InitialClosingTime, 
				FinalClosingTime, StatusId, CreatedOn, UpdatedBy, HighestBid, ReservePrice
			)
			VALUES
			(
				@AuctionId, @CarId, @BasePrice, @StartDateTime, @InitialClosingTime, 
				@FinalClosingTime , @StatusId, @CreatedOn, @UpdatedBy, @BasePrice, @ReservePrice
			)
			
			SET @retID = SCOPE_IDENTITY()
		END
	ELSE	
		BEGIN
			UPDATE 
			AE_AuctionCars 
			SET 
				StartDateTime = @StartDateTime, InitialClosingTime = @InitialClosingTime, 
				FinalClosingTime = @FinalClosingTime, StatusId = @StatusId, 
				UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy, ReservePrice = @ReservePrice
			WHERE Id =  @Id				
			
			SET @retID = @Id	
		END
		
	
	
--SELECT * FROM AE_AuctionCars
--TRUNCATE TABLE AE_AuctionCars
	
END



