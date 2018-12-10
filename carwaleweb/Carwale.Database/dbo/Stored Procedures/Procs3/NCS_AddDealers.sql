IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddDealers]
GO

	/** Modifier: Vinay Kumar Prajapati
    Purpose : Adding Three New Colomn "AreaId" "Latitude", "Longitude" To Show the Exact Location( Geographical Position) Of dealers 
	Modifier: Sachin Bharti(15th Oct 2013)
	Purpose : Added dealer type for registered dealers
	Modifier: Vinay Kumar Prajapati 6th jan 2015 
	Purpose  : To Add DisplayName and Save it
 **/ 
  
  
CREATE PROCEDURE [dbo].[NCS_AddDealers]  
 @Id    NUMERIC,  
 @Name   VARCHAR(200),
 @DealerTitle VARCHAR(200), 
 @AreaName VARCHAR(200),
 @PrincipleName VARCHAR(200), 
 @CityId   NUMERIC, 
 @AreaId NUMERIC,
 @Latitude DECIMAL(18,0), 
 @Longitude DECIMAL(18,0), 
 @LandlineNo  VARCHAR(50),  
 @Mobile   VARCHAR(50),  
 @PrincipleMobile   VARCHAR(50),
 @ContactPerson VARCHAR(50),  
 @Address  VARCHAR(500),  
 @EMail   VARCHAR(200),
 @PrincipleEMail   VARCHAR(200),  
 @DealerCode   VARCHAR(50),  
 @IsActive  BIT,  
 @IsNCDDealer  BIT,
 @EntryDateTime DATETIME,  
 @OrgId   NUMERIC,  
 @RMgrId   NUMERIC,
 @UpdatedBy   NUMERIC = NULL,
 @IsNCDFeedback BIT,
 @DealerId       Numeric OUTPUT,
 @DealerType	TINYINT = NULL
 AS  
   
BEGIN  
	SET @DealerId = -1  
	IF @Id = -1 --Insertion  
  
		BEGIN  
		   SELECT Id FROM NCS_Dealers   
		   WHERE Name = @Name AND CityId = @CityId  
  
			IF @@ROWCOUNT = 0  
				BEGIN  
					 INSERT INTO NCS_Dealers  
					 (   
					  Name,DealerTitle,AreaName, CityId, LandlineNo, Mobile,PrincipleMobile, ContactPerson, Address,  
					  EMail, PrincipleEmail,DealerCode, EntryDateTime, IsActive, IsNCDDealer ,PrincipleName,IsDealerFeedback,AreaId,Latitude,Longitude,DealerType 
					 )     
					 Values  
					 (   
					  @Name,@DealerTitle,@AreaName, @CityId, @LandlineNo, @Mobile, @PrincipleMobile,@ContactPerson, @Address,  
					  @EMail,@PrincipleEMail, @DealerCode, @EntryDateTime, @IsActive,@IsNCDDealer ,@PrincipleName ,@IsNCDFeedback ,@AreaId,@Latitude,@Longitude,@DealerType
					 )   
  
					SET @DealerId = SCOPE_IDENTITY()  
					
					IF @OrgId <> -1 AND @OrgId <> 0   
						INSERT INTO NCS_SubDealerOrganization (OId,DId) VALUES (@OrgId, @DealerId) 
				END
			ELSE  
					SET @DealerId = -1
		END 
	ELSE  
		BEGIN  
			UPDATE NCS_Dealers   
			SET Name = @Name, DealerTitle=@DealerTitle,AreaName=@AreaName, CityId = @CityId, LandlineNo = @LandlineNo,   
				Mobile = @Mobile, ContactPerson = @ContactPerson, PrincipleMobile=@PrincipleMobile,  
				Address = @Address, EMail = @EMail, DealerCode = @DealerCode, PrincipleEMail=@PrincipleEMail ,PrincipleName=@PrincipleName , 
				IsActive  = @IsActive, UpdatedBy = @UpdatedBy, UpdatedOn= GETDATE()
				,IsDealerFeedback=@IsNCDFeedback,AreaId=@AreaId,Latitude=@Latitude,Longitude=@Longitude,DealerType = @DealerType
			WHERE Id = @Id  
			IF @OrgId <> -1 AND @OrgId <> 0   
				UPDATE NCS_SubDealerOrganization SET OId = @OrgId WHERE DId = @Id 
			
			SET @DealerId = @Id  
				
		END  
END  
  
