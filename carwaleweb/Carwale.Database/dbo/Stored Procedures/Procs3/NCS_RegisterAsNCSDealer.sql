IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_RegisterAsNCSDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_RegisterAsNCSDealer]
GO

	-- =============================================
-- Author:		Nilesh Utture
-- Create date: 3rd October, 2012
-- Description:	Register user in Dealer_NewCar as NCS Dealer
-- =============================================
CREATE PROCEDURE [dbo].[NCS_RegisterAsNCSDealer] 
	-- Add the parameters for the stored procedure here
 @Id    NUMERIC(18,0),
 @EntryDateTime DATETIME, 
 @UpdatedBy INT, 
 @Status SMALLINT OUTPUT 
  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE
	@Name   VARCHAR(200), 
	@PrincipleName VARCHAR(200), 
	@CityId   NUMERIC,  
	@LandlineNo  VARCHAR(50),  
	@Mobile   VARCHAR(50),  
	@PrincipleMobile   VARCHAR(50)=NULL,
	@ContactPerson VARCHAR(50),  
	@Address  VARCHAR(500),  
	@EMail   VARCHAR(200),
	@PrincipleEMail   VARCHAR(200)=NULL,  
	@DealerCode   VARCHAR(50)=NULL,  
	@IsActive  BIT,  
	@IsNCDDealer  BIT,
	@NCSId NUMERIC(18,0),
	@MakeId NUMERIC,
	@TcDealerId NUMERIC

	SELECT @Name = Name, @CityId=CityId,@MakeId=MakeId,@LandlineNo=ContactNo, @Mobile=Mobile,@ContactPerson=ContactPerson, @Address=Address,  
		   @EMail=EMailId,@IsActive=IsActive,@IsNCDDealer=IsNCD, @TcDealerId=TcDealerId ,@NCSId=NCSId FROM Dealer_NewCar WHERE Id=@Id

	SELECT @NCSId = ND.Id FROM NCS_Dealers AS ND INNER JOIN NCS_DealerMakes DM ON ND.ID = DM.DealerId   
	WHERE DM.MakeId=@MakeId AND ND.CityId=@CityId AND ND.Name=@Name

	IF @NCSId IS NULL   --Insertion
		BEGIN  
			INSERT INTO NCS_Dealers  
			(   
			Name, CityId, LandlineNo, Mobile,PrincipleMobile, ContactPerson, Address,  
			EMail, PrincipleEmail,DealerCode, UpdatedOn, UpdatedBy, IsActive, IsNCDDealer ,PrincipleName ,EntryDateTime, TCDealerId 
			)     
			Values  
			(   
			@Name, @CityId, @LandlineNo, @Mobile, @PrincipleMobile,@ContactPerson, @Address,  
			@EMail,@PrincipleEMail, @DealerCode, @EntryDateTime, @UpdatedBy, @IsActive,@IsNCDDealer ,@PrincipleName , @EntryDateTime, @TcDealerId
			)   
			
			SET @NCSId = SCOPE_IDENTITY()  
			INSERT INTO NCS_DealerMakes(DealerId, MakeId) VALUES(@NCSId, @MakeId)
		END
	ELSE --Record Present in NCS_Dealers Table so GET NCSId to update it in Dealer_NewCars
		BEGIN 
			SELECT @NCSId = ND.Id FROM NCS_Dealers AS ND INNER JOIN NCS_DealerMakes DM ON ND.ID = DM.DealerId   
			WHERE DM.MakeId=@MakeId AND ND.CityId=@CityId AND ND.Name=@Name 
		END
		
	IF @NCSId IS NOT NULL --Update NCSId in Dealer_NewCars if It is not Null
		BEGIN
			UPDATE Dealer_NewCar SET NCSId=@NCSId WHERE Id=@Id
			SET @Status = 1 
		END
	ELSE
		BEGIN
			SET @Status = -1
		END
END
