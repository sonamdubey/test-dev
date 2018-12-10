IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[INS_InsertPremiumLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[INS_InsertPremiumLeads]
GO

	-- Modified By : Ashish G. Kamble on 3 July 2013
-- Modified : Added email, mobile and name fields to save the insurance leads.
-- Modified By : Akansha on 10.07.2013
-- Modified : Added StatusCode to save the Request Status in PushStatus Column.
CREATE PROCEDURE [dbo].[INS_InsertPremiumLeads]
    @Id            NUMERIC,
    @CustomerId    NUMERIC,
    @InsTypeNew    BIT,
    @VersionId     NUMERIC,
    @MakeYear      DATETIME,
    @CityId        NUMERIC,
    @Price         NUMERIC,
    @Displacement  NUMERIC,
    @RegistrationArea VARCHAR(100),
    @Premium          DECIMAL(18,2),
    @RequestDateTime  DATETIME,
    @Name VARCHAR(50),
    @Email VARCHAR(100),
    @Mobile VARCHAR(12),
    @RecordID         NUMERIC OUTPUT,
	@StatusCode VARCHAR(15),
	@ClientId tinyint
 AS
   
BEGIN
     IF @Id = -1
     	BEGIN
	     INSERT INTO INS_PremiumLeads
	             (
	                CustomerId, InsTypeNew, VersionId, MakeYear, CityId,
	                Price, Displacement, RegistrationArea, Premium, RequestDateTime, Name, Email, Mobile,PushStatus,ClientId
	             )
	      VALUES
	             (
	                @CustomerId, @InsTypeNew, @VersionId, @MakeYear, @CityId,
	                @Price, @Displacement, @RegistrationArea, @Premium, @RequestDateTime, @Name, @Email, @Mobile,@StatusCode,@ClientId
	             )

	      SET @RecordID = SCOPE_IDENTITY()
           
        END
    ELSE
        BEGIN
            SET @RecordID = 0
        END
END


/****** Object:  StoredProcedure [dbo].[GetRSLeadCount]    Script Date: 8/20/2013 5:46:03 PM ******/
SET ANSI_NULLS ON
