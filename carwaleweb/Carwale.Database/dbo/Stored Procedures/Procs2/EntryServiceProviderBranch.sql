IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryServiceProviderBranch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryServiceProviderBranch]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR serviceproviderbranch table
--ID, ServiceProviderId, Address, AreaId, CityId, StateId, Telephone, Fax, ContactPerson, Mobile, Email1, Email2, ContactHours, LastUpdated, 
--PhotoUrl, IsMain, EntryDate

CREATE PROCEDURE [dbo].[EntryServiceProviderBranch]
	@ID			NUMERIC,		--ID. IF IT IS -1 THEN IT IS FOR INSERT, ELSE IT IS FOR UPDATE FOR THE ID 		        	
	@ServiceProviderId	NUMERIC,
	@Address		VARCHAR(250), 
	@AreaId		NUMERIC, 
	@CityId			NUMERIC, 
	@StateId		NUMERIC, 
	@Telephone		VARCHAR(100), 
	@Fax			VARCHAR(30), 
	@ContactPerson	VARCHAR(100), 
	@Mobile		VARCHAR(15), 
	@Email1		VARCHAR(100), 
	@Email2		VARCHAR(100), 
	@ContactHours		VARCHAR(50), 
	@LastUpdated		DATETIME, 
	@PhotoUrl		VARCHAR(100), 
	@IsMain		BIT, 
	@EntryDate		DATETIME,
	@BranchId		NUMERIC OUTPUT
AS
	
BEGIN
	
	IF @ID = -1
	BEGIN
		--IT IS FOR THE INSERT
		INSERT INTO ServiceProviderBranchs
			(	
				ServiceProviderId, 	Address, 	AreaId, 		CityId, 		StateId, 	Telephone, 
				Fax, 			ContactPerson, 	Mobile, 		Email1, 		Email2, 		ContactHours, 
				LastUpdated, 		PhotoUrl, 	IsMain, 		EntryDate
			
			) 
		VALUES
			(	
				@ServiceProviderId, 	@Address, 	@AreaId, 	@CityId, 	@StateId, 	@Telephone, 
				@Fax, 			@ContactPerson, @Mobile, 	@Email1, 	@Email2, 	@ContactHours, 
				@LastUpdated, 		@PhotoUrl, 	@IsMain, 	@EntryDate
			)
		
		SET @BranchId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		--IT IS FOR THE UPDATE
		UPDATE ServiceProviderBranchs SET 
			Address			= @Address, 	
			AreaId			= @AreaId, 		
			CityId			= @CityId, 		
			StateId			= @StateId, 	
			Telephone		= @Telephone, 
			Fax			= @Fax, 			
			ContactPerson		= @ContactPerson, 	
			Mobile			= @Mobile, 		
			Email1			= @Email1, 		
			Email2			= @Email2, 		
			ContactHours		= @ContactHours, 
			LastUpdated		= @LastUpdated, 		
			PhotoUrl		= @PhotoUrl, 	
			IsMain			= @IsMain
		 WHERE 
			ID 			= @ID
		
		SET @BranchId = @ID
	END
	
		
END
