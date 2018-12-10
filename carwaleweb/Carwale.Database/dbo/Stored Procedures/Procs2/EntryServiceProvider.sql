IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryServiceProvider]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryServiceProvider]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR serviceprovider table

CREATE PROCEDURE [dbo].[EntryServiceProvider]
	@ID			NUMERIC,		--ID. IF IT IS -1 THEN IT IS FOR INSERT, ELSE IT IS FOR UPDATE FOR THE ID 		        	
	@Name			VARCHAR(150),		--name of the organization
	@WebsiteUrl		VARCHAR(100),		--web site url if any
	@ServiceProviderId	NUMERIC OUTPUT
 AS
	
BEGIN
	
	IF @ID = -1
	BEGIN
		--IT IS FOR THE INSERT
		INSERT INTO ServiceProviders
			(	Name, 		WebsiteUrl, 		IsActive) 
		VALUES
			(	@Name, 	@WebsiteUrl, 		1	)

		SET @ServiceProviderId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		--IT IS FOR THE UPDATE
		UPDATE ServiceProviders SET 
			Name		= @Name,
			WebsiteUrl	= @WebsiteUrl
		 WHERE 
			ID = @ID

		SET @ServiceProviderId = @ID
	END
	
		
END
