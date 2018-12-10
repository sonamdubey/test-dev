IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[INS_InsertPremiumLeads_V14101]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[INS_InsertPremiumLeads_V14101]
GO

	-- Modified By : Ashish G. Kamble on 3 July 2013
-- Modified : Added email, mobile and name fields to save the insurance leads.
-- Modified By : Akansha on 10.07.2013
-- Modified : Added StatusCode to save the Request Status in PushStatus Column.
-- Modified By : Ashish G. Kamble on 14 April 2015
-- Modified : Added new fields PolicyExpiryDate, CarRegistrationDate, NoClaimBonus
CREATE PROCEDURE [dbo].[INS_InsertPremiumLeads_V14101]
    @Id            NUMERIC,
    @CustomerId    NUMERIC,
    @InsTypeNew    BIT,
    @VersionId     NUMERIC,
    @MakeYear      NVARCHAR(20),
    @CityId        NUMERIC,
    @Price         NUMERIC,
    @Displacement  NUMERIC,
    @RegistrationArea VARCHAR(100),
    @Premium          DECIMAL(18,2),
    @RequestDateTime  DATETIME,
    @Name VARCHAR(50),
    @Email VARCHAR(100),
    @Mobile VARCHAR(12),
	@LeadSource INT,
    @RecordID         NUMERIC OUTPUT,
	@StatusCode VARCHAR(15),
	@ClientId tinyint,
	@PolicyExpiryDate DATETIME,
	@CarRegistrationDate DATETIME,
	@NoClaimBonus INT
 AS
   
BEGIN
     IF @Id = -1
     	BEGIN
	     INSERT INTO INS_PremiumLeads
	             (
	                CustomerId, InsTypeNew, VersionId, MakeYear, CityId,
	                Price, Displacement, RegistrationArea, Premium, RequestDateTime, Name, Email, Mobile, LeadSource , PushStatus,ClientId,
					PolicyExpiryDate, CarRegistrationDate, NoClaimBonus
	             )
	      VALUES
	             (
	                @CustomerId, @InsTypeNew, @VersionId, @MakeYear, @CityId,
	                @Price, @Displacement, @RegistrationArea, @Premium, GETDATE(), @Name, @Email, @Mobile,@LeadSource , @StatusCode,@ClientId,
					@PolicyExpiryDate, @CarRegistrationDate, @NoClaimBonus
	             )

	      SET @RecordID = SCOPE_IDENTITY()
           
        END
    ELSE
        BEGIN
            SET @RecordID = 0
        END
END