IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_SaveCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_SaveCarDetails]
GO

	
--CREATED ON 03 Nov 2009 BY SENTIL          
--PROCEDURE FOR Auction Engine Car Details Insert And Update
          
CREATE PROCEDURE [dbo].[AE_SaveCarDetails]           
(      
	@ID AS BIGINT = 0,
	@AERefNumber AS VARCHAR(50) = NULL,
	@VersionId AS NUMERIC(18,0)=0,
	@CarType AS SmallInt = 0,
	@SaleType AS SmallInt = 0,
	@YardID AS INT =0,
	@AuctionerId AS INT=0,
	@TBoard AS BIT = NULL,
	@MakeYear AS DATETIME = NULL,
	@Kms AS VARCHAR(50) = NULL,
	@BasePrice AS NUMERIC(18,0)=0,
	@ValuationPrice AS NUMERIC(18,0)=0,
	@ReservePrice AS NUMERIC(18,0)=0,
	@Color AS VARCHAR(50) = NULL,
	@RegistrationNumber AS VARCHAR(50) = NULL,
	@RegistrationPlace AS VARCHAR(50) = NULL,
	@ChasisNo AS VARCHAR(100)=NULL,
	@EngineNo AS VARCHAR(100)=NULL,
	@Owners AS INT = 0,
	@DocumentsAvailable AS BIT = 0,
	@StatusId AS SmallInt = 0,
	@Grade AS SMALLINT = 0,
	@Comments AS VARCHAR(1000) = NULL,
	@InsuranceType AS VARCHAR(50) = NULL,
	@InsuranceExpiryDate AS DATETIME = NULL,
	@FuelType AS SMALLINT = 0,
	@TransmissionType AS SMALLINT = 0,
	@PhotoAvailable AS BIT = 0,
	@VideoAvailable AS BIT = 0,
	@RepoDate AS DATETIME = NULL,
	@LastUpdatedOn AS DATETIME = NULL, 
	@LastUpdatedBy AS NUMERIC = 0,
	@Operation AS SMALLINT,        
	@carId AS BIGINT OUT           
)          
AS          
BEGIN      
      
	 IF  ( @Operation = 1 )
		 BEGIN
			 INSERT INTO AE_CarDetails          
					( AERefNumber, VersionId, CarType, SaleType, YardID, AuctionerId, TBoard, MakeYear, Kms, BasePrice, ValuationPrice, Color, 
					  RegistrationNumber, RegistrationPlace, ChasisNo, EngineNo, Owners, DocumentsAvailable, StatusId, Grade,
					  Comments, InsuranceType, InsuranceExpiryDate, FuelType, TransmissionType, PhotoAvailable, VideoAvailable,
					  RepoDate, UpdatedOn, UpdatedBy, ReservePrice )          
			  VALUES( @AERefNumber, @VersionId , @CarType, @SaleType, @YardID, @AuctionerId, @TBoard, @MakeYear, @Kms, @BasePrice, @ValuationPrice,
					  @Color, @RegistrationNumber, @RegistrationPlace, @ChasisNo, @EngineNo, @Owners, @DocumentsAvailable, @StatusId, @Grade,
					  @Comments, @InsuranceType,@InsuranceExpiryDate,	@FuelType, @TransmissionType, @PhotoAvailable,
					  @VideoAvailable, @RepoDate, @LastUpdatedOn, @LastUpdatedBy, @ReservePrice )
		  END        
	  IF  ( @Operation = 2 )
		  BEGIN
				--SELECT @VersionId = ISNULL(VersionId,NULL),@CarType = ISNULL(CarType,NULL)
				--FROM AE_CarDetails Where carId = @ID
				
				UPDATE AE_CarDetails 
				SET VersionId = @VersionId, CarType = @CarType, SaleType = @SaleType, YardID = @YardID, AuctionerId = @AuctionerId,
					TBoard = @TBoard, MakeYear = @MakeYear, Kms = @Kms, BasePrice = @BasePrice, ValuationPrice = @ValuationPrice,
					Color = @Color, RegistrationNumber = @RegistrationNumber, RegistrationPlace = @RegistrationPlace,
					ChasisNo = @ChasisNo, EngineNo = @EngineNo, DocumentsAvailable = @DocumentsAvailable, StatusId = @StatusId,
					Grade = @Grade, Comments = @Comments, InsuranceType = @InsuranceType,
					InsuranceExpiryDate = @InsuranceExpiryDate, FuelType = @FuelType, TransmissionType = @TransmissionType,
					PhotoAvailable = @PhotoAvailable, VideoAvailable = @VideoAvailable, UpdatedOn = @LastUpdatedOn,
					UpdatedBy = @LastUpdatedBy, AERefNumber = @AERefNumber, Owners = @Owners, RepoDate = @RepoDate, ReservePrice = @ReservePrice
				WHERE carId = @ID
		  END	  
	  
	  SET @carId = SCOPE_IDENTITY()          
          
--SELECT * FROM AE_CarDetails          
--TRUNCATE TABLE AE_CarDetails           
          
END
