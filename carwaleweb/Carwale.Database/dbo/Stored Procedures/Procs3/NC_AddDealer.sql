IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NC_AddDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NC_AddDealer]
GO

	-- =============================================
-- Modified by:		Nilesh Utture
-- Modified date:	3-October-2012
-- Description:		Added parameters @ContactPerson,@MobileNo 
-- Modified by Raghu on <2/14/2014> : Added paramater @TcDealerId
--  Modified by vikram on <5/7/2014> : Added paramater @IsNewDealer
-- Moifier	:	Vaibhav K 3 Dec 2014 : Added new parameters for ShowroomStartTime, ShowroomEndTime, PrimaryMobileNo, SecondaryMobileNo, LandLineNo, DealerArea
-- =============================================
CREATE PROCEDURE [dbo].[NC_AddDealer]
	@Id				BIGINT,
	@MakeId			BIGINT, 
	@CityId			BIGINT,
	@Name			VARCHAR(100),
	@Address		VARCHAR(1000),
	@Pincode		VARCHAR(50),
	@ContactNo		VARCHAR(200),
	@FaxNo			VARCHAR(50),
	@EMailId		VARCHAR(100),
	@WebSite		VARCHAR(100),
	@WorkingHours	VARCHAR(50),
	@LastUpdated	DATETIME,
	@IsNCD			BIT,
	@IsActive		BIT,
	@ContactPerson	VARCHAR(200),
	@MobileNo		VARCHAR(50),
	@TcDealerId		BIGINT = -1,
	@IsNewDealer	BIT = null,
	@ShowroomStartTime	VARCHAR(30) = NULL,
	@ShowroomEndTime	VARCHAR(30) = NULL,
	@PrimaryMobileNo		VARCHAR(20) = NULL,
	@SecondaryMobileNo	VARCHAR(20) = NULL,
	@LandLineNo		VARCHAR(30) = NULL,
	@DealerArea		VARCHAR(100) = NULL,
	@Latitude		FLOAT = NULL,
	@Longitude		FLOAT = NULL,
	@Status			SMALLINT OUTPUT
 AS
	
BEGIN
	SET @Status = 0

	IF @Id = -1
		BEGIN

			SELECT ID FROM Dealer_NewCar WITH (NOLOCK) WHERE MakeId = @MakeId AND CityId = @CityId AND Name = @Name
			
			IF @@RowCount = 0
				BEGIN
					INSERT INTO Dealer_NewCar( MakeId, CityId, Name, Address, Pincode, ContactNo, FaxNo,
							EMailId, WebSite, WorkingHours, LastUpdated, IsActive,IsNCD,ContactPerson,Mobile,TcDealerId,IsNewDealer,
							ShowroomStartTime, ShowroomEndTime, PrimaryMobileNo, SecondaryMobileNo, LandLineNo, DealerArea, Latitude, Longitude) 
					VALUES( @MakeId, @CityId, @Name, @Address, @Pincode, @ContactNo, @FaxNo,
							@EMailId, @WebSite, @WorkingHours, @LastUpdated, @IsActive,@IsNCD,@ContactPerson,@MobileNo,@TcDealerId,@IsNewDealer,
							@ShowroomStartTime, @ShowroomEndTime, @PrimaryMobileNo, @SecondaryMobileNo, @LandLineNo, @DealerArea, @Latitude, @Longitude)
		
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
	ELSE
		BEGIN
			SELECT ID FROM Dealer_NewCar  WITH (NOLOCK) WHERE MakeId = @MakeId AND CityId = @CityId AND Name = @Name AND  ID <> @Id
			
			IF @@RowCount = 0

				BEGIN
					UPDATE  Dealer_NewCar SET MakeId = @MakeId, CityId = @CityId, Name = @Name, Address = @Address, 
						Pincode =  @Pincode, ContactNo = @ContactNo, FaxNo = @FaxNo, EMailId = @EMailId, 
						WebSite = @WebSite, WorkingHours = @WorkingHours, LastUpdated = @LastUpdated, IsActive = @IsActive, IsNCD = @IsNCD,
						ContactPerson = @ContactPerson, Mobile = @MobileNo,TcDealerId = @TcDealerId,IsNewDealer = @IsNewDealer,
						ShowroomStartTime = @ShowroomStartTime, ShowroomEndTime = @ShowroomEndTime,
						PrimaryMobileNo = @PrimaryMobileNo, SecondaryMobileNo = @SecondaryMobileNo, LandLineNo = @LandLineNo, DealerArea = @DealerArea,
						Latitude = @Latitude, Longitude = @Longitude
					 WHERE ID = @Id
				
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
END


------------------------------------------------------------------------------