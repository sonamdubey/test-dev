IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[INS_InsertPremiumLeads_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[INS_InsertPremiumLeads_V15]
GO

	



--Modified by Piyush Sahu on 10-13-2015 Added ClientIp Input Parameter
CREATE  PROCEDURE [dbo].[INS_InsertPremiumLeads_V15.10.1]
    @Id            INT,
    @CustomerId    INT,
    @InsTypeNew    BIT,
    @VersionId     INT,
    @MakeYear      VARCHAR(20),
    @CityId        INT,
    @Price         NUMERIC,
    @Displacement  NUMERIC,
    @RegistrationArea VARCHAR(100),
    @Premium          DECIMAL(18,2),
    @RequestDateTime  DATETIME,
    @Name VARCHAR(50),
    @Email VARCHAR(100),
    @Mobile VARCHAR(12),
	@LeadSource INT,
    @RecordID         INT OUTPUT,
	@StatusCode VARCHAR(15),
	@ClientId tinyint,
	@PolicyExpiryDate DATETIME,
	@CarRegistrationDate DATETIME,
	@NoClaimBonus INT,
	@ClientIp VARCHAR(30)
 AS
   
BEGIN
     IF @Id = -1
     	BEGIN
	     INSERT INTO INS_PremiumLeads
	             (
	                CustomerId, InsTypeNew, VersionId, MakeYear, CityId,
	                Price, Displacement, RegistrationArea, Premium, RequestDateTime, Name, Email, Mobile, LeadSource , PushStatus,ClientId,
					PolicyExpiryDate, CarRegistrationDate, NoClaimBonus,ClientIp
	             )
	      VALUES
	             (
	                @CustomerId, @InsTypeNew, @VersionId, @MakeYear, @CityId,
	                @Price, @Displacement, @RegistrationArea, @Premium, GETDATE(), @Name, @Email, @Mobile,@LeadSource , @StatusCode,@ClientId,
					@PolicyExpiryDate, @CarRegistrationDate, @NoClaimBonus,@ClientIp
	             )

	      SET @RecordID = SCOPE_IDENTITY()
           
        END
    ELSE
        BEGIN
            SET @RecordID = 0
        END
END
